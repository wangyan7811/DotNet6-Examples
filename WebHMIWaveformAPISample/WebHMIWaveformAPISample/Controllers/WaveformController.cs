using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using ExternalSystem.API.Extension;
using WebHMIWaveformAPISample.Extensions;

namespace WebHMIWaveformAPISample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WaveformController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PowerManagerClient _powerManagerClient;
        private readonly string _host = "10.10.1.225";
        private static string _token = string.Empty;
        private static string _cookie = string.Empty;
        private static  HttpClient _httpClient;
        public WaveformController(IHttpClientFactory httpClientFactory, PowerManagerClient powerManagerClient)
        {
            _httpClientFactory = httpClientFactory;
            _powerManagerClient = powerManagerClient;
        }

        [HttpGet]
        [Route("GetToken")]
        public async Task GetToken()
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Get,
                    $@"https://{_host}/WebHmi/Auth?ReturnUrl=%2fWebHmi%2f%23HmiApplication%2f%23lib%2fiec5foverview2etgml")
                {
                    Headers =
                {
                    { HeaderNames.Accept, "application/vnd.github.v3+json" },
                    { HeaderNames.UserAgent, "HttpRequestsSample" }
                }
                };
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = delegate { return true; }
                };
                var httpClient = new HttpClient(handler);
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();
                    Regex re = new Regex("(?<txt>(?<=Anti-Forgery-Token\": \").+(?=\"))");
                    var match = re.Match(content);
                    if (match.Success)
                        _token = match.Value;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("GetCookie")]
        public async Task GetCookie()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = delegate { return true; },
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };
            _httpClient = new HttpClient(handler);

            var todoItemJson = new StringContent(
                @"
                {
	            ""userName"": ""admin"",
                ""password"": ""admin"",
                ""returnUrl"": "" / WebHmi/#Alarms"",
                ""setAuthCookie"": true
                }
                ",
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            _httpClient.DefaultRequestHeaders.Add("anti-forgery-token", _token);
            var httpResponseMessage = await _httpClient.PostAsync($@"https://{_host}/PsoDataService/Security/ValidateCredentials", todoItemJson);

            var uri = new Uri($@"https://{_host}/PsoDataService/Security/ValidateCredentials");
            List<Cookie> cookies = handler.CookieContainer.GetCookies(uri).Cast<Cookie>().ToList();
            var sbCookie = new StringBuilder();

            foreach (var item in cookies)
            {
                sbCookie.Append(item.Name);
                sbCookie.Append("=");
                sbCookie.Append(item.Value);
                sbCookie.Append(";");
                
            }
            _cookie = $@"{sbCookie.ToString()}";

        }


        [HttpGet]
        [Route("GetToken2")]
        public async Task GetToken2()
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Get,
                    $@"https://{_host}/WebHmi/#HmiApplication/#lib/iec5foverview2etgml")
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "application/vnd.github.v3+json" },
                        { HeaderNames.UserAgent, "HttpRequestsSample" },
                        { HeaderNames.Cookie, _cookie },
                        { "anti-forgery-token", _token },
                    }
                };
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = delegate { return true; }
                };
                var httpClient = new HttpClient(handler);
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();
                    Regex re = new Regex("(?<txt>(?<=antiForgeryToken=').+(?=';))");
                    var match = re.Match(content);
                    if (match.Success)
                        _token = match.Value;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpGet]
        [Route("GetAlarmIds")]
        public async Task GetAlarmIds()
        {
            //var handler = new HttpClientHandler
            //{
            //    ServerCertificateCustomValidationCallback = delegate { return true; }
            //};
            //var httpClient = new HttpClient(handler);

            var todoItemJson = new StringContent(@"{}",
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            if (1==1)
            {
                _token =
                    @"FX257nMQdlmOAaaFg_8ENENWRVns2h79Qqiu8GSKJ7IHHE9AafHmhlZN8zFrEK-UzkXV_AkUEJsMCzF60eiSRjL_1tYptrticEibgzL2Wt01:QomG5DenwcPwZ7x2BhscNiK2eERvTLb2s2cb-hqz4WJ3Dl4UJN_H-BfLhhxhdMiEKDmTv7h5q8bPPZE4rozWB84lgPLNOANSa5FPlM-QdYZNcln1fQqqlaR7gd8_g8E6IRuVBRymOXn-fPEAYzvUNg2";
                _cookie = @"forceLogout/WebHmi=false; .APPFRAMEWORK=9CD175595AEEDA23EE857A8FD73B0B8C145951E3FDF92732A14D0D788A0E4E39B5370F63CDF00143A07436E988306AD2BBBD1A320CFC61D31502D75AC4DF7EC8ADC3C362ECF458FE471DA21618418B69EDFECB42851F7708A9FDEA3D07A681EA8276BE06C93E462B34D0D24B87F2DF0EF785A8A3C7013340145FC50D24732024CA9E59385798751502F8A46C55CD70F93FA9135BC7D24BF924AC98BF02146F22BF6183D508B767331282514F05CC1CA3794203B410BDAFC8B6645225D7E79BF03980ABF0577E385A469476B8DFA7498B0E85D648EA8FE2CD5A8849E49810626A; lastApplicationActiveTime/WebHmi=1658907379011";

            }
            else
            {
                var r = await _powerManagerClient.WebLoginAsync();
                //_token=r.ExecuteAsync()
            }



            _httpClient.DefaultRequestHeaders.Add("anti-forgery-token", _token);
            _httpClient.DefaultRequestHeaders.Add("cookie", _cookie);

            //todoItemJson.Headers.TryAddWithoutValidation("Content-Type", "application/json");
            //todoItemJson.Headers.TryAddWithoutValidation("Anti-Forgery-Token", _token);
            //todoItemJson.Headers.TryAddWithoutValidation("Cookie", _cookie);


            //todoItemJson.Headers.TryAddWithoutValidation("anti-forgery-token", _token);
            //todoItemJson.Headers.TryAddWithoutValidation("cookie", _cookie);
            //todoItemJson.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            //todoItemJson.Headers.ContentLanguage.Add("zh-CN");
            //todoItemJson.Headers.ContentEncoding.Add("gzip");
            //todoItemJson.Headers.ContentEncoding.Add("deflate");
            //todoItemJson.Headers.TryAddWithoutValidation("user-agent", "ApiPOST Runtime +https://www.apipost.cn");


            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
            _httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");

            var httpResponseMessage = await _httpClient.PostAsync($@"https://{_host}/PsoDataService/AlarmData", todoItemJson);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
            }
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
                        { HeaderNames.Cookie, _cookie },
                        { "anti-forgery-token", _token },
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
                        { HeaderNames.Cookie, _cookie },
                        { "anti-forgery-token", _token },
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
