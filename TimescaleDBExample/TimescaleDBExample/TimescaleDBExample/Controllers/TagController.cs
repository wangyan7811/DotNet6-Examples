using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TimescaleDBExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly SwaggerClient _swaggerClient;
        public TagController(SwaggerClient client)
        {
            _swaggerClient = client;
        }
        [HttpPost]
        [Route("post-history")]
        public async Task Post()
        {
            var r = await _swaggerClient.TokenAsync("client_credentials", "admin", "admin");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tagName50_Latest.txt");
            var json = System.IO.File.ReadAllText(path).Trim('-');
            var e = JsonConvert.DeserializeObject<Rootobject>(json);
            //var tagHistory  =await _swaggerClient.GetHistory3Async(r.Access_token, new TagHistoryRequest()
            //{
            //    From = 123,
            //    To = 123,
            //    Tags =e.tags
            //});
            var list = new List<TagHistoryValue>()
            {
                new TagHistoryValue()
                {
                    TagName = "demo",
                    Val = 11.1,
                    Ts = DateTime.Now,
                    Quality = TagValueQuality.Good,
                }
            };
            //foreach (var tags in tagHistory.Tags)
            //{
            //    foreach (var record in tags.Records)
            //    {
            //        list.Add(new TagHistoryValue()
            //        {
            //            TagName = tags.Tag,
            //            Val = record.Val,
            //            Ts = record.Ts,
            //            Quality = record.Quality
            //        });
            //    }
            //}
            var a = await SqlSugarHelper.Db.Insertable(list).ExecuteCommandAsync();
        }
        public class Rootobject
        {
            public string[] tags { get; set; }
        }
    }
    

}
