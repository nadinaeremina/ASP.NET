using Microsoft.AspNetCore.Mvc;
using UserChallange.Api.Atributes;
using UserChallange.Api.Messages;

namespace UserChallange.Api.Controllers
{
    // ResourceController - некоторый ресурс API, который должен быть защищен
    // только зареганный пользователь сможет получать доступ к этим обработчикам
    [Route("api/resource")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        [HttpGet]
        [Protect]
        // здесь по умолчанию: isVIP: false
        public StringMessage Get()
        {
            return new StringMessage(Message: "you have access to some api-resource");
        }

        // можно помечать контроллеры специальным атрибутом - что он защищен
        // любой атрибут - это класс, который наследуется от базового атрибута

        [HttpGet("vip")]
        [Protect(isVIP:true)]
        // здесь по умолчанию: isVIP: true
        public StringMessage GetVip()
        {
            return new StringMessage(Message: "you have access to some VIP api-resource");
        }
    }
}
