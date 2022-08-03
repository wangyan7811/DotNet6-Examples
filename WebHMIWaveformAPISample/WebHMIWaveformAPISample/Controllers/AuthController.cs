using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using WebHMIWaveformAPISample.Extensions;
using WebHMIWaveformAPISample.Services;

namespace WebHMIWaveformAPISample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PowerManagerClient _powerManagerClient;
        private readonly TokenAndCookieService _tokenAndCookieService;
        private readonly string _host = "10.10.1.225";
        //private static string _token = string.Empty;
        //private static string _cookie = string.Empty;
        private static HttpClient _httpClient;
        public AuthController(IHttpClientFactory httpClientFactory, PowerManagerClient powerManagerClient, TokenAndCookieService tokenAndCookieService)
        {
            _httpClientFactory = httpClientFactory;
            _powerManagerClient = powerManagerClient;
            _tokenAndCookieService= tokenAndCookieService;
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
                        _tokenAndCookieService.Token = match.Value;

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
            _httpClient.DefaultRequestHeaders.Add("anti-forgery-token", _tokenAndCookieService.Token);
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
            _tokenAndCookieService.Cookie = $@"{sbCookie.ToString()}";

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
                        { "anti-forgery-token", _tokenAndCookieService.Token },
                        { HeaderNames.Cookie, _tokenAndCookieService.Cookie },
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
                        _tokenAndCookieService.Token = match.Value;

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
