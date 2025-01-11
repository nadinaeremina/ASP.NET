using Microsoft.AspNetCore.Mvc;
using UserController.Api.Messages;

namespace UserController.Api.Controllers
{
    // ResourceController - некоторый ресурс API
    [Route("api/resource")]
    [ApiController]
    public class ResourceController : ControllerBase
    {

        [HttpGet]
        public StringMessage Get()
        {
            return new StringMessage(Message: "you have access to some api-resource");
        }

        [HttpPost]
        public StringMessage Post()
        {
            return new StringMessage(Message: "you have access to some POST api-resource");
        }
    }
}
