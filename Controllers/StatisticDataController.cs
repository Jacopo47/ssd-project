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
        
        [HttpGet("prevision")]
        public async Task<IActionResult> prevision()
        {
            var list = new List<String>();

            for (int i = 1; i < 10; i++)
            {
                list.Add("cust" + new Random().Next(50));
            }

            var result = await "http://localhost:7000/api/prevision"
                .PostJsonAsync(new
                {
                    customers = list
                }).ReceiveString();

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
