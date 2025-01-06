using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportDictionaryAsp_v1.Api
{
    // класс, в котором есть обработчики, которые автоматически привязываются

    // атрибут для того, чтобы маршрутизироваться к методам контроллера
    // все методы контроллера, являющиеся обработчиками, будут в префиксе маршрута иметь это значение
    // в квадратные скобки будет подствляться наименование контроллера - можно убрать
    // [Route("api/[controller]")]
    [Route("api")]

    // добавление контроллеров в IoC-контейнер и для их привязки к обработчикам
    [ApiController]

    // наш класс наследуется от базового 'ControllerBase'
    public class ApplicationController : ControllerBase
    {
        // в самом контроллере можно сделать обработчики

        // атрибут означает, что к данному обработчику будет вести метод 'get'
        // запрос: get/api
        [HttpGet]
        public StringMessages Root()
        {
            return new StringMessages(Message: "server is running");
        }

        // в атрибут 'get' можно передать параметр
        // запрос: get/api/ping
        [HttpGet("ping")]
        public StringMessages Ping()
        {
            // 'HttpContext' в классах-контроллерах выступает как свойство базового класса 'ControllerBase'
            // HttpContext.Request
            // HttpContext.Response
            
            return new StringMessages(Message: "pong");
        }
    }
}
