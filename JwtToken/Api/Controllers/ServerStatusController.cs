using Microsoft.AspNetCore.Mvc;
using UserChallange.Api.Messages;

namespace UserChallange.Api.Controllers
{
    // базовые обработчики
    [Route("api")]
    [ApiController]
    public class ServerStatusController : ControllerBase
    {
        [HttpGet]
        public StringMessage GetStatus()
        {
            return new StringMessage("server is running");
        }

        // достучимся к этому методу, если юзер не получил доступ
        //{
        //    "type": "UnauthorizedApiKeyHeader",
        //    "message": "X-Api-Key header is unauthorized"
        //}

        // если есть доступ - то:
        //{
        //    "message": "pong"
        //}

        [HttpGet("ping")]
        public StringMessage Ping()
        {
            return new StringMessage("pong");
        }

        // можно помечать контроллеры специальным атрибутом - что он защищен
        // любой атрибут - это класс, который наследуется от базового атрибута
    }
}
