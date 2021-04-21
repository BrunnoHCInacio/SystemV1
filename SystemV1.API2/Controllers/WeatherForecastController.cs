using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;

namespace API2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IApplicationServiceClient _applicationServiceClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IApplicationServiceClient applicationServiceClient)
        {
            _logger = logger;
            _applicationServiceClient = applicationServiceClient;
        }

        [HttpGet]
        public async Task<ActionResult<object>> Get()
        {
            var rng = new Random();

            var a = _applicationServiceClient.GetAllAsync(1, 10);
            return Ok(a);
        }
    }
}