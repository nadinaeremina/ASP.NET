using JwtToken.Api.JWT;
using Microsoft.AspNetCore.Mvc;
using UserChallange.Api.Messages;

namespace JwtToken.Api.Controllers
{
    // контроллер для прохождения аутентификации
    [Route("api/auth")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly JwtService _jwt;
        public AuthController(JwtService jwt)
        {
            _jwt = jwt;
        }

        [HttpPost]
        public async Task<IActionResult> AuthAsync(ApiKeyMessage apiKey)
        {
            try
            {
                // обычно апи ключ передается в теле
                string token = await _jwt.GenerateTokenAsync(apiKey.ApiKey);
                // 200
                return Ok(new TokenMessage(Token: token));
            }
            catch (InvalidApiKeyException ex)
            {
                // 401 // если невалидный токен
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Unauthorized(error);
            }
        }
    }
}
