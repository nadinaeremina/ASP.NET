using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        public static void ConfigureJWTOptions(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                // это структура
                // издатель токена и валидировать ли издателя токена
                // можно задать информацию о том, кто выпустил токен

                // издатель токена и валидировать ли издателя токена
                ValidateIssuer = true,
                ValidIssuer = JWT_ISSUER,

                // потребитель токена и валидировать ли потребителя токена
                ValidateAudience = true,
                // если поставить 'false' - не будет валидироваться, проверяться, кто создал и кто принял
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

        private const string ISSUER_SIGNING_KEY_SEED = "seedseedseedseedseedseedseedseed";
        private static SecurityKey GetIssuerSigningKey()
        {
            byte[] seedBytes = Encoding.UTF8.GetBytes(ISSUER_SIGNING_KEY_SEED);
            return new SymmetricSecurityKey(seedBytes);
        }
    }
}
