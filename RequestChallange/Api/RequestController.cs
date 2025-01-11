using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using RequestChallange.Model;

namespace RequestChallange.Api
{
    // 'RequestController' - контроллер для работы с запросами

    // ресурс этого контроллера - это запросы
    [Route("api/request")]
    [ApiController]
    // сделаем его наследником 'ControllerBase'
    public class RequestController : ControllerBase
    {
        // затаскиваем сервис для работы с запросами как зависимость
        private readonly RequestService _requests;
        public RequestController(RequestService requests)
        {
            _requests = requests;
        }

        // обработчики

        // 1 // получение списка с информацией обо всех запросах, хранящихся в системе
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            // получаем список стран
            return Ok(await _requests.ListAllAsync());
        }

        // 2 // очистка данных о всех запросах в системе
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {
            await _requests.DeleteAllDataAsync();
            return NoContent();
        }
    }
}