using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Flurl;
using Flurl.Http;

namespace ssdProject.Controllers
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]

    public class StatisticDataController : ControllerBase
    {

        private readonly ILogger<StatisticDataController> _logger;

        public StatisticDataController(ILogger<StatisticDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet("hello/{name}")]
        public async Task<IActionResult> hello(string name)
        {
            var result = await "http://localhost:7000/user/jacopo"
                                .GetJsonAsync();

            return Ok(result);
        }
        
        [HttpPost("prevision")]
        public async Task<IActionResult> prevision([FromBody] List<String> customers)
        {
            var result = await "http://localhost:7000/api/prevision"
                .PostJsonAsync(new
                {
                    customers
                }).ReceiveString();

            return Ok(result);
        }
        
        [HttpGet("prevision/{customer}")]
        public async Task<IActionResult> prevision(string customer)
        {
            var result = await ("http://localhost:7000/api/prevision/" + customer)
                .GetStringAsync();

            return Ok(result);
        }
    }
    
    

    public class Response
    {
        public String msg;

        public Response(String msg) {
            this.msg = msg;
        }
    }

}
