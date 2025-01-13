using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserChallange.Api.Atributes;
using UserChallange.Api.Messages;
using UserChallange.Model.Exceptions;
using UserChallange.Model.Users;

namespace UserChallange.Api.Controllers
{
    // UserController - контроллер для работы с пользователями
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserScenarios _users;

        public UserController(UserScenarios users)
        {
            _users = users;
        }

        //{
        //    "login": "johndoe1",
        //    "email": "jhon19647@gmail.com"
        //}

        // зарегать пользователя
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegistrationMessage data)
        {
            try
            {
                string apiKey = await _users.RegisterAsync(data.Login, data.Email);
                // 200
                return Ok(new ApiKeyMessage(ApiKey: apiKey));
            }
            catch (ValidationException ex)
            {
                // 400
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
            catch (DuplicationException ex)
            {
                // 409
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Conflict(error);
            }
        }
        // "apiKey": "c84f75c89eade38bb413d0b41f9b9df9"
        // в заголовке принимает "X-Api-Key" - находит, либо не находит пользователя
        [HttpGet]
        [Protect]
        public async Task<IActionResult> GetInfoAsync([FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            try
            {
                User user = await _users.GetUserAsync(apiKey);
                // 200
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                // 404
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }

        //{
        //    "uuid": "ab47bcb8-f505-44f4-b7c8-25f07e8afe3d",
        //    "login": "johndoe1",
        //    "email": "jhon19647@gmail.com",
        //    "registeredAt": "2025-01-12T06:35:34.5705378Z"
        //}

        // но при перезапуске программы все данные исчезнут!!!!!!!!!!!!!!!!!!!!!!!!
    }
}
