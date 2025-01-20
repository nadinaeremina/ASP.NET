using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserChallange.Model.Exceptions;
using UserChallange.Model.Users;

namespace JwtToken.Api.JWT
{
    // класс сервиса для обслуживания Jwt-токена (для предоставления конфигурации jwt-токенов)
    // в будущем также - для выдачимы генерации jwt-токенов
    // мы не будем реализовывать Jwt-токен, мы будем его использовать
    public class JwtService
    {
        // метод конфигурации jwt-схемы аутентификации
        // нужен для того, чтобы здесь при добавлении сервисов выполнить конфигурацию

        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //                .AddJwtBearer(JwtService.ConfigureJWTOptions);

        // параметиры jwt-схемы и токена
        private const string JWT_ISSUER = "UserChallange_issuer";
        private const string JWT_AUDIENCE = "UserChallange_audience";
        private const int JWT_LIFE_TIME_MINUTES = 30;

        public static void ConfigureJWTOptions(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                // это структура
                // настроим лишь несколько параметров
                // здесь указывается то, что будет проверяться
                // издатель токена и валидировать ли издателя токена
                // можно задать информацию о том, кто выпустил токен

                // издатель токена и валидировать ли издателя токена
                ValidateIssuer = true, // если поставить 'false' - не будет проверяться, кто создал
                ValidIssuer = JWT_ISSUER,

                // потребитель токена и валидировать ли потребителя токена
                ValidateAudience = true,
                // если поставить 'false' - не будет валидироваться, проверяться, кто принял
                ValidAudience = JWT_AUDIENCE,

                // параметры валидации времени жизни
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                // сдвиг часов

                // параметры валидации подписи токена // нельзя будет подменить токен
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetIssuerSigningKey()
            };
        }
        // параметры авторизации 
        public const string VIP_ROLE = "vip"; // константы можно использовать в атрибутах
        private readonly UserScenarios _users;

        public JwtService(UserScenarios users)
        {
            _users = users;
        }

        // метод для генерации токена (будем генерировать по апи ключу, потому что апи ключи - это наши креды)
        // на вход получаем апи ключ пользователя - на выходе строку jwt-токена
        // исключения: 'InvalidApiKeyException'
        public async Task<string> GenerateTokenAsync(string apiKey)
        {
            try
            {
                // 1 //  получить пользователя по апи ключу
                User user = await _users.GetUserAsync(apiKey);
                // 2 // для сборки т окена нужно задать клаймы токена
                List<Claim> claims = new List<Claim>()
                {
                    // тип клайма и значение
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Email, user.Email)
                    // зашифровали данную инфу - потом ее можно доставать
                };
                if (user.IsVIP)
                {
                    // если пользователь VIP, то добавим ему роль
                    claims.Add(new Claim(ClaimTypes.Role, VIP_ROLE));
                }
                // 3. по
                // 3 // подготовить подпись токена, которая будет проверяться - за это отвечает:
                SigningCredentials signing = new SigningCredentials(
                    GetIssuerSigningKey(),
                    SecurityAlgorithms.HmacSha256
                    );
                // 4 // собрать токен
                JwtSecurityToken jwt = new JwtSecurityToken(
                    // зададим параметры токена
                    issuer: JWT_ISSUER,
                    audience: JWT_AUDIENCE,
                    claims: claims,
                    signingCredentials: signing,
                    expires: DateTime.UtcNow.AddMinutes(JWT_LIFE_TIME_MINUTES)
                    );
                // 5 // вернуть токен ввиде строки
                string jwtStr = new JwtSecurityTokenHandler().WriteToken(jwt);
                return jwtStr;
            }
            catch (UserNotFoundException ex)
            {
                // пользователя нет - следовательно апи ключ неправильный
                throw new InvalidApiKeyException();
            }
        }

        private const string ISSUER_SIGNING_KEY_SEED = "seedseedseedseedseedseedseedseed"; 
        // нужно, чтобы ключ был не менее 32 байт (32 символв)
        private static SecurityKey GetIssuerSigningKey()
        {
            // ключ нужно перевести в байты
            byte[] seedBytes = Encoding.UTF8.GetBytes(ISSUER_SIGNING_KEY_SEED);
            return new SymmetricSecurityKey(seedBytes);
        }

        // в заголовках: bearer {пробел} {token}
    }
}
