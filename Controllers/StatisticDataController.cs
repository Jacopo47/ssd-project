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
        public async Task<IActionResult> prevision([FromBody] List<string> customers)
        {
            var result = await "http://localhost:7000/api/prevision"
                .PostJsonAsync(new
                {
                    customers
                }).ReceiveString();

            return Ok(result);
        }
        
        [HttpGet("prevision/{customer}/arima")]
        public async Task<IActionResult> previsionArima(string customer)
        {
            var result = await ("http://localhost:7000/api/prevision/" + customer + "/arima")
                .GetJsonAsync();

            return Ok(result);
        }
        
        [HttpGet("prevision/{customer}/sarimax")]
        public async Task<IActionResult> previsionSarimax(string customer)
        {
            var result = await ("http://localhost:7000/api/prevision/" + customer + "/sarimax")
                .GetJsonAsync();

            return Ok(result);
        }
        
        [HttpGet("prevision/{customer}/ml")]
        public async Task<IActionResult> previsionLstmNeuralNetwork(string customer)
        {
            var result = await ("http://localhost:7000/api/prevision/" + customer + "/ml")
                .GetJsonAsync();

            return Ok(result);
        }
    }
    
    

    public class Response
    {
        public string Msg;

        public Response(string msg) {
            this.Msg = msg;
        }
    }

}
