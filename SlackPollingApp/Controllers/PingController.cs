using Microsoft.AspNetCore.Mvc;

namespace SlackPollingApp.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class PingController : ControllerBase
    {

        private const string PingResponse = "pong"; 
        
        [HttpGet]
        public string Ping()
        {
            return PingResponse;
        }
    }
}