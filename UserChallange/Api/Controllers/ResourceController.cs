using JwtToken.Api.JWT;
using Microsoft.AspNetCore.Authorization;
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
        // 1 // в 'headers' передаем заголовок 'Authorization' и прописываем: Bearer {token}
        // 2 // в 'Authorization' выбираем 'Bearer Token' (всегда) и в 'Token' вставялем сам токен 
        [HttpGet]
        [Authorize] // помечает обработчик как требуемый для защиты через аутентификацию
        // в него можно передавать различные параметры
        // здесь по умолчанию: isVIP: false
        public StringMessage Get()
        {
            return new StringMessage(Message: "you have access to some api-resource");
        }

        // можно помечать контроллеры специальным атрибутом - что он защищен
        // любой атрибут - это класс, который наследуется от базового атрибута

        [HttpGet("vip")]
        // здесь будут прописываться названия ролей, разделенных запятой
        // [Authorize(Roles = "vip")]
        [Authorize(Roles = JwtService.VIP_ROLE)]
        // здесь по умолчанию: isVIP: true
        public StringMessage GetVip()
        {
            return new StringMessage(Message: "you have access to some VIP api-resource");
        }
    }
}
