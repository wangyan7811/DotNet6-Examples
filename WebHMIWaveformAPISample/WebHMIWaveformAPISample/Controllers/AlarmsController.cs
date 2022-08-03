using System.Net;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using WebHMIWaveformAPISample.Extensions;
using WebHMIWaveformAPISample.Services;

namespace WebHMIWaveformAPISample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PowerManagerClient _powerManagerClient;
        private readonly TokenAndCookieService _tokenAndCookieService;
        private static HttpClient _httpClient;
        public AlarmsController(IHttpClientFactory httpClientFactory, PowerManagerClient powerManagerClient, TokenAndCookieService tokenAndCookieService)
        {
            _httpClientFactory = httpClientFactory;
            _powerManagerClient = powerManagerClient;
            _tokenAndCookieService = tokenAndCookieService;
        }

        [HttpGet]
        
        public async Task Get()
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    $@"https://{_tokenAndCookieService.Host}/PsoDataService/AlarmDefinitionData")
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
                    ,
                    Content = new StringContent(
                        string.Format(@"{{
                    ""ActiveStatus"": {0},
                    ""AcknowledgementStatus"": 0,
                    ""DisableStatus"": 2,
                    ""ShelveStatus"": 2,
                    ""StatusCompareAnd"": true,
                    ""AlarmTypes"": [
                    ""ALM_ARC_FAULT"",
                    ""ALM_ARC_FLASH"",
                    ""ALM_BACKUPPOWER"",
                    ""ALM_CLOCK"",
                    ""ALM_COMMLOSS"",
                    ""ALM_CONTROL_EVENT"",
                    ""ALM_DEVICE_STATUS"",
                    ""ALM_ENERGYMGMT_AIR"",
                    ""ALM_ENERGYMGMT_DEMAND"",
                    ""ALM_ENERGYMGMT_ELECTRICITY"",
                    ""ALM_ENERGYMGMT_GAS"",
                    ""ALM_ENERGYMGMT_STEAM"",
                    ""ALM_ENERGYMGMT_WATER"",
                    ""ALM_FLICKER_VOLTAGE"",
                    ""ALM_FREQUENCY_VOLTAGE"",
                    ""ALM_GENERIC_SETPOINT"",
                    ""ALM_HARMONIC"",
                    ""ALM_HARMONIC_CURRENT"",
                    ""ALM_HARMONIC_POWER"",
                    ""ALM_HARMONIC_VOLTAGE"",
                    ""ALM_INTERRUPTION_VOLTAGE"",
                    ""ALM_OVER_CURRENT"",
                    ""ALM_OVER_VOLTAGE"",
                    ""ALM_POWER_FACTOR"",
                    ""ALM_PROTECTION"",
                    ""ALM_SAG_CURRENT"",
                    ""ALM_SAG_VOLTAGE"",
                    ""ALM_SWELL_CURRENT"",
                    ""ALM_SWELL_VOLTAGE"",
                    ""ALM_SYSTEM"",
                    ""ALM_THERMAL_MONITOR"",
                    ""ALM_TRANSIENT_VOLTAGE"",
                    ""ALM_UNBALANCE"",
                    ""ALM_UNBALANCE_CURRENT"",
                    ""ALM_UNBALANCE_VOLTAGE"",
                    ""ALM_UNCLASSIFIED_PQ"",
                    ""ALM_UNDER_CURRENT"",
                    ""ALM_UNDER_VOLTAGE""
                        ],
                    ""DeviceFilter"": {{
                    ""SelectedDevices"": []
                }},
                ""TeamIds"": [],
                ""PriorityFilter"": [
                ""high"",
                ""low"",
                ""med""
                    ],
                ""MaxRecords"": 1000,
                ""ModifiedSinceUtc"": ""{1}""
                }}",0, "2022-08-03T11:08:49.175Z"),
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
