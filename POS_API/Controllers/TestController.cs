using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace POS_API.Controllers
{
    [ApiController, Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TestController> _logger;
        //private readonly IConfiguration _configuration;
        public TestController(ILogger<TestController> logger/*, IConfiguration configuration*/)
        {
            _logger = logger;
            //_configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        //[HttpGet("GetConStr/{key}")]
        //public string GetConStr(string key) => key != "Default" ? "" : _configuration.GetConnectionString(key);

    }
}
