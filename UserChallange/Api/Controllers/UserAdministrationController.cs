using Microsoft.AspNetCore.Mvc;
using UserChallange.Api.Messages;
using UserChallange.Model.Users;
using UserChallange.Model.Exceptions;

namespace UserChallange.Api.Controllers
{
    // контроллер для администрирования пользователей 
    // в рамках разработки не защищен, но должен быть защищен в продакшене
    [Route("api/admin/user")]
    [ApiController]
    public class UserAdministrationController: ControllerBase
    {
        private readonly UserAdministrationScenarios _users;
        public UserAdministrationController(UserAdministrationScenarios users)
        {
            _users = users; 
        }

        // получить всех пользователей 
        [HttpGet]
        public async Task<List<User>> GetAllAsync()
        {
            return await _users.ListAllAsync();
        }

        // получить пользователя по UUID
        [HttpGet("{uuid:guid}")]
        public async Task<IActionResult> GetByUUIDAsync(Guid uuid)
        {
            try
            {
                // получаем юзера
                User user = await _users.GetByUUIDAsync(uuid);
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

        // получить пользователя по UUID
        [HttpGet("{email:alpha}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            try
            {
                // получаем юзера
                User user = await _users.GetByEmailAsync(email);
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

        // получить пользователя по UUID
        [HttpGet("login/{login:alpha}")]
        public async Task<IActionResult> GetAsync(string login)
        {
            try
            {
                // получаем юзера
                User user = await _users.GetByLoginAsync(login);
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

        // установить роль
        [HttpPatch("{uuid:guid}/set-vip")]
        public async Task<IActionResult> SetVipAsync(Guid uuid)
        {
            try
            {
                await _users.SetVIPAsync(uuid);
                // 204
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                // 404
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }

        // забрать роль
        [HttpPatch("{uuid:guid}/unset-vip")]
        public async Task<IActionResult> UnsetVipAsync(Guid uuid)
        {
            try
            {
                await _users.UnsetVIPAsync(uuid);
                // 204
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                // 404
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }

        // забрать роль
        [HttpPut]
        public async Task<IActionResult> AddAsync(User user)
        {
            try
            {
                await _users.InsertAsync(user);
                // 204
                return NoContent();
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
