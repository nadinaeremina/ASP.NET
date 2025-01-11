using Microsoft.AspNetCore.Mvc;
using UserController.Api.Messages;

namespace UserController.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class ServerStatusController : ControllerBase
    {
        [HttpGet]
        public StringMessage GetStatus()
        {
            return new StringMessage("server is running");
        }

        [HttpGet("ping")]
        public StringMessage Ping()
        {
            return new StringMessage("pong");
        }
    }
}
