using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ssdProject.Model;

namespace ssdProject.Controllers
{
    namespace ssdProject.Controllers
    {
        [ApiController]
        [Route("api")]
        [Produces("application/json")]
        public class OptimizeDataController : ControllerBase
        {
            private readonly ILogger<StatisticDataController> _logger;

            public OptimizeDataController(ILogger<StatisticDataController> logger)
            {
                _logger = logger;
            }

            [HttpGet("optimize/{customer}/local")]
            public async Task<IActionResult> optimizeLocal(string customer)
            {
                var G = new GAPClass();

                new Persistence().readGAPinstance(G);


                var result = await ("http://localhost:7000/api/optimize/prevision/" + customer)
                    .GetJsonAsync<ForecastResponse>();

                G.req = result.forecasts.ToArray();
                
                return Ok(G.opt10(G.c));
            }
            
            [HttpGet("optimize/{customer}/tabusearch")]
            public async Task<IActionResult> optimizeTabuSearch(string customer, [FromQuery] int tenure = 30, [FromQuery] int iterations = 1000)
            {
                
                var G = new GAPClass();

                new Persistence().readGAPinstance(G);


                var result = await ("http://localhost:7000/api/optimize/prevision/" + customer)
                    .GetJsonAsync<ForecastResponse>();

                G.req = result.forecasts.ToArray();
                G.simpleContruct();
                G.opt10(G.c);
                return Ok(G.TabuSearch(tenure, iterations));
            }
        }


        public class ForecastResponse
        {
            public List<int> forecasts;

            public ForecastResponse(List<int> forecasts)
            {
                this.forecasts = forecasts;
            }
        }
    }
}