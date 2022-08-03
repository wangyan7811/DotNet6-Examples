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
                //_token =
                //    @"FX257nMQdlmOAaaFg_8ENENWRVns2h79Qqiu8GSKJ7IHHE9AafHmhlZN8zFrEK-UzkXV_AkUEJsMCzF60eiSRjL_1tYptrticEibgzL2Wt01:QomG5DenwcPwZ7x2BhscNiK2eERvTLb2s2cb-hqz4WJ3Dl4UJN_H-BfLhhxhdMiEKDmTv7h5q8bPPZE4rozWB84lgPLNOANSa5FPlM-QdYZNcln1fQqqlaR7gd8_g8E6IRuVBRymOXn-fPEAYzvUNg2";
                //_cookie = @"forceLogout/WebHmi=false; .APPFRAMEWORK=9CD175595AEEDA23EE857A8FD73B0B8C145951E3FDF92732A14D0D788A0E4E39B5370F63CDF00143A07436E988306AD2BBBD1A320CFC61D31502D75AC4DF7EC8ADC3C362ECF458FE471DA21618418B69EDFECB42851F7708A9FDEA3D07A681EA8276BE06C93E462B34D0D24B87F2DF0EF785A8A3C7013340145FC50D24732024CA9E59385798751502F8A46C55CD70F93FA9135BC7D24BF924AC98BF02146F22BF6183D508B767331282514F05CC1CA3794203B410BDAFC8B6645225D7E79BF03980ABF0577E385A469476B8DFA7498B0E85D648EA8FE2CD5A8849E49810626A; lastApplicationActiveTime/WebHmi=1658907379011";

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


        [HttpGet]
        [Route("WebHMILogin")]
        public async Task WebHMILogin()
        {
           var r=await _powerManagerClient.WebLoginAsync();
        }


    }
}
