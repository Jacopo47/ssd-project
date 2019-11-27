using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ssdProject.Controllers
{
    [ApiController]
    [Route("orders")]
    public class DbTestConnection : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public DbTestConnection(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<String> GetAll()
        {
            return new Persistence().readOrdini().ToArray();
        }

        [HttpGet("{id}")]
        public IEnumerable<String> GetByID(string id)
        {
            return new Persistence().readOrdiniByID(id).ToArray();
        }

        [HttpGet("insert")]
        public ContentResult insert()
        {
            try {
                Order order = new Order("", "cust1", "0",  0);

                order.create();

                return Content("Successfully created!");
            } catch(Exception ex) {
                return Content("Error on create: " + ex.Message);
            }
        }

        [HttpGet("update/{id}")]
        public ContentResult update(string id)
        {
            try {
                Order order = Order.select(id);

                order.update();

                return Content("Successfully update!");
            } catch(Exception ex) {
                return Content("Error on update: " + ex.Message);
            }
        }

        [HttpGet("delete/{id}")]
        public ContentResult delete(string id)
        {
            try {
                Order order = Order.select(id);

                order.delete();

                return Content("Successfully deleted!");
            } catch(Exception ex) {
                return Content("Error on delete: " + ex.Message);
            }
            
        }

        [HttpGet("order/{id}")]
        public ContentResult getOrderByID(string id)
        {
            try {
                Order order = Order.select(id);

                return Content(order.ToString());
            } catch(Exception ex) {
                return Content("Error on get order: " + ex.Message);
            }
        }

    }
}
