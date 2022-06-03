using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ReDocExample.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 天气查询接口
        /// </summary>
        /// <remarks>天气情况接口详细描述，巴拉巴拉</remarks>
        /// <returns></returns>
        [HttpGet(Name = "天气查询接口")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<WeatherForecast>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized,Type = typeof(Error))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Error))]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}