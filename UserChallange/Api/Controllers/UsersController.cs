using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserController.Api.Messages;
using UserController.Model.Exceptions;
using UserController.Model.Users;
using ValidationException = UserController.Model.Exceptions.ValidationException;

namespace UserController.Api.Controllers
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

        [HttpGet]
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
    }
}
