using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace UseStaticFiles.Controllers
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

        [HttpGet(Name = "GetWeatherForecast")]
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

        [HttpGet("download")]
        public async Task<FileStreamResult> Download(
            [FromQuery] string device
            , [FromQuery] DateTime startDate
            , [FromQuery] DateTime endDate)
        {
            try
            {
                //根据设备名和日期范围寻找cfg和dat文档 打包zip
                

                var fileName = @"c1\pm8000\cmt0__00000.cfg";
                //po默认存储故障卢默目录
                string rootPath = @"C:\ProgramData\Schneider Electric\Power Operation\v2021\Data\waveformdb";
                string filePath = Path.Combine(rootPath, fileName);
                var stream = System.IO.File.OpenRead(filePath);
                string contentType = "application/x-msdownload";
                return File(stream, contentType, fileName);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

       

    }
}