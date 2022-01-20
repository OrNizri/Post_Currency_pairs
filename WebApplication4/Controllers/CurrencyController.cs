using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {

        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ILogger<CurrencyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Currency> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Currency
            {
                Date = DateTime.Now.AddDays(index),
            })
            .ToArray();
        }
    }
}
