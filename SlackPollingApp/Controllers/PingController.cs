using Microsoft.AspNetCore.Mvc;

namespace SlackPollingApp.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public string ping()
        {
            return "pong";
        }
    }
}