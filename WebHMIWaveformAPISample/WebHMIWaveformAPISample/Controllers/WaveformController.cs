using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using ExternalSystem.API.Extension;
using WebHMIWaveformAPISample.Extensions;
using WebHMIWaveformAPISample.Services;

namespace WebHMIWaveformAPISample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WaveformController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PowerManagerClient _powerManagerClient;
        private readonly TokenAndCookieService _tokenAndCookieService;
        private readonly string _host = "10.10.1.225";
       
        private static HttpClient _httpClient;
        public WaveformController(IHttpClientFactory httpClientFactory, PowerManagerClient powerManagerClient, TokenAndCookieService tokenAndCookieService)
        {
            _httpClientFactory = httpClientFactory;
            _powerManagerClient = powerManagerClient;
            _tokenAndCookieService = tokenAndCookieService;
        }

        

        [HttpGet]
        [Route("GetAlarmIds2")]
        public async Task GetAlarmIds2()
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    $@"https://{_host}/PsoDataService/AlarmData")
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "*/*" },
                        { HeaderNames.Connection, "keep-alive" },
                        { HeaderNames.AcceptEncoding, "gzip, deflate, br" },
                        { HeaderNames.AcceptLanguage, "zh-CN" },
                        { "anti-forgery-token", _tokenAndCookieService.Token },
                        { HeaderNames.Cookie, _tokenAndCookieService.Cookie },
                        
                        { HeaderNames.UserAgent, "ApiPOST Runtime" }
                    }
                    ,Content = new StringContent(@"{
        }",
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
            };
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = delegate { return true; },
                    AutomaticDecompression = DecompressionMethods.GZip
                };
                var httpClient = new HttpClient(handler);
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();
                    

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("WaveformAPI")]
        public async Task WaveformAPI()
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    $@"https://{_host}/PsoDataService/WaveformData/Bundle")
                {
                    Headers =
                    {
                        //{ HeaderNames.Accept, "*/*" },
                        //{ HeaderNames.Connection, "keep-alive" },
                        //{ HeaderNames.AcceptEncoding, "gzip, deflate, br" },
                        //{ HeaderNames.AcceptLanguage, "zh-CN" },
                        { HeaderNames.Cookie, _tokenAndCookieService.Cookie },
                        { "anti-forgery-token", _tokenAndCookieService.Token },
                        //{ HeaderNames.UserAgent, "ApiPOST Runtime" }
                    }
                    ,
                    Content = new StringContent(@"{
  ""LookupType"": ""alarmid"",
                    ""Ids"": [
                    ""PLSDCluster.S6K_CD_CPL_A51_PTOC4_Op_dchg_637943959837810000""
                    ],
                    ""IncludeWaveformData"": true,
                    ""PageSize"": 0,
                    ""PageNumber"": 0
                }",
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
                };
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = delegate { return true; },
                    AutomaticDecompression = DecompressionMethods.GZip
                };
                var httpClient = new HttpClient(handler);
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        
    }
}
