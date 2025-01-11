using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace UserChallange.Api.Jwt
{
    // класс сервиса для обслуживания jwt-токенов, нужен для предоставления
    // конфигурации jwt-токенов, а также для выдачи (генерации) jwt-токенов
    public class JwtService
    {
        // ConfigureJwtOptions - метод конфигурации jwt-схемы аутентификации
        public static void ConfigureJWTOptions(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                // издатель токена и валидировать ли издателя токена
                ValidateIssuer = true,
                VakidIssuer = JWT_ISSUER,
                // потребитель токена и валидировать ли потребителя токена
                ValidAudence = true,
                // параметры валидации времени жизни,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                // параметры валидации подписи т окена
                ValidateIssuerSugningKey = true,
                IssuerSigningKeyResolver = 
            };
        }

        private const string 
    }
}
