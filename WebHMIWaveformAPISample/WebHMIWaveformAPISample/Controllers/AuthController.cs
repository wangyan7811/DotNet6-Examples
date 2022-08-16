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
        //private readonly PowerManagerClient _powerManagerClient;
        private readonly TokenAndCookieService _tokenAndCookieService;

        private static HttpClient _httpClient;
        public AuthController(IHttpClientFactory httpClientFactory, PowerManagerClient powerManagerClient, TokenAndCookieService tokenAndCookieService)
        {
            _httpClientFactory = httpClientFactory;
            //_powerManagerClient = powerManagerClient;
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
                    $@"https://{_tokenAndCookieService.Host}/WebHmi/Auth?ReturnUrl=%2fWebHmi%2f%23Alarms")
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
            var str = string.Format(@"
                {{
	            ""userName"": ""{0}"",
                ""password"": ""{1}"",
                ""returnUrl"": "" / WebHmi/#Alarms"",
                ""setAuthCookie"": true
                }}
                ", _tokenAndCookieService.User, _tokenAndCookieService.Password);
            var todoItemJson = new StringContent(
                str,
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            _httpClient.DefaultRequestHeaders.Add("anti-forgery-token", _tokenAndCookieService.Token);
            var httpResponseMessage = await _httpClient.PostAsync($@"https://{_tokenAndCookieService.Host}/PsoDataService/Security/ValidateCredentials", todoItemJson);

            var uri = new Uri($@"https://{_tokenAndCookieService.Host}/PsoDataService/Security/ValidateCredentials");
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
                    $@"https://{_tokenAndCookieService.Host}/WebHmi/#Alarms/lib/49101144-a686-4e39-8365-c41f33275986")
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

        [HttpGet]
        [Route("Login")]
        public async Task Login(string ip = "10.10.1.225", string user = "admin", string password = "admin")
        {
            _tokenAndCookieService.Host=ip;
            _tokenAndCookieService.User=user;
            _tokenAndCookieService.Password=password;
            await GetToken();
            await GetCookie();
            await GetToken2();
        }
       
    }
}
