using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiThread.CommunityToolKit;
using Newtonsoft.Json;
using TimescaleDBExample.Helpers;
using TimescaleDBExample.models;

namespace TimescaleDBExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
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
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tagName14165_History.txt");
            var json = System.IO.File.ReadAllText(path).Trim('-');
            var e = JsonConvert.DeserializeObject<Rootobject>(json);
            var sum = 0;
            //for (int i = 0; i < 13; i++)
            //{
            //    var from = new DateTimeOffset(DateTime.UtcNow.AddDays(-(i + 1) * 29));
            //    var to = new DateTimeOffset(DateTime.UtcNow.AddDays(-i * 29));
            //    var tagHistory = await _swaggerClient.GetHistory3Async(r.Access_token, new TagHistoryRequest()
            //    {
            //        From = from.ToUnixTimeMilliseconds(),
            //        To = to.ToUnixTimeMilliseconds(),
            //        Tags = e.tags
            //    });
            //var list = new List<TagHistoryValue>();
            //foreach (var tags in tagHistory.Tags)
            //{
            //    foreach (var record in tags.Records)
            //    {
            //        list.Add(new TagHistoryValue()
            //        {
            //            TagName = tags.Tag,
            //            Val = record.Val,
            //            Ts  = TimeSpanToDateTime(record.Ts.Value),
            //            Quality = record.Quality
            //        });
            //    }
            //}

            var ran = new Random();
            //e.tags.ToList().ForEach(t =>
            //{
            //    for (int i = 1; i <= 365; i++)
            //    {
            //        var list = new List<TagHistoryValue>();
            //        for (int j = 0; j < 86400 ; j++)
            //        {
            //            list.Add(new TagHistoryValue()
            //            {
            //                TagName = t,
            //                Val =Math.Round(ran.NextDouble() * 100,2),
            //                Ts = DateTime.Now.AddSeconds(-j*i),
            //                Quality = TagValueQuality.Good
            //            });
            //        }
            //        var a = SqlSugarHelper.Db.Insertable(list).ExecuteCommand();
            //        Console.WriteLine($@"tag:{t} insert:{a}");
            //        sum += a;
            //    }

            //});

            var exclude = new List<string>()
            {
                "c1.H_1AH_TH110_1\\TTMP1\\Bus\\tmpa",
                "c1.H_1AH_TH110_2\\TTMP1\\Bus\\tmpb",
                "c1.L_2AL3_NSXMicE_2\\MMXU1\\PF\\phsA",
                "c1.L_2AL3_NSXMicE_2\\MMXU1\\PF\\phsB",
                "c1.L_2AL3_NSXMicE_2\\MMXU1\\TotPF",
                "c1.L_3AL6_NSXMicE_2\\MMXU1\\A\\neut",
                "c1.L_3AL6_NSXMicE_2\\MMXU1\\A\\zgnd1",
                "c1.L_3AL6_NSXMicE_2\\MSTA1\\AvW",
                "Cluster Name.Expression"
            };
            MultiThreadJobHelper.Do(6, e.tags.ToList(), (r) =>
           {
               r.ToList().ForEach(t =>
               {
                   if (exclude.Contains(t))
                   {
                   }
                   else
                   {
                       for (int i = 1; i <= 30; i++)
                       {
                           var list = new List<TagHistoryValue>();
                           for (int j = 0; j < 17280; j++)
                           {
                               list.Add(new TagHistoryValue()
                               {
                                   TagName = t,
                                   Val = Math.Round((ran.NextDouble() + 0.03) * 100, 2),
                                   Ts = DateTime.Now.AddSeconds(-j * i * 5),
                                   Quality = TagValueQuality.Good
                               });
                           }
                           var a = SqlSugarHelper122.Db.Insertable(list).ExecuteCommand();
                           Console.WriteLine($@"tag:{t} insert:{a}");
                           sum += a;
                       }
                   }
               });
               return default;
           });


            //    sum += a;
            //Console.WriteLine($@"from:{from} to:{to} insert:{a} tag values");
            //}
            Console.WriteLine($@"sum insert:{ sum}");
        }

        [HttpPost]
        [Route("post-history-122")]
        public async Task Post122()
        {
            var tokenResponse = await _swaggerClient.TokenAsync("client_credentials", "admin", "admin");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tagName14165_History.txt");
            var json = System.IO.File.ReadAllText(path).Trim('-');
            var e = JsonConvert.DeserializeObject<Rootobject>(json);
            var sum = 0;
            var tagNames = (from t in e.tags.Distinct()
                            select new Tag()
                {
                    TagName = t
                }).ToList();
            var tags = SqlSugarHelper122.Db.Queryable<Tag>().ToList();
            if ( tags.Count==0)
                await SqlSugarHelper122.Db.Insertable<Tag>(tagNames).ExecuteCommandAsync(); 
            tags = SqlSugarHelper122.Db.Queryable<Tag>().ToList();
            var ran = new Random();
            _ = MultiThreadJobHelper.Do(6, e.tags.ToList(), (r) =>
            {
                r.ToList().ForEach(t =>
                {
                    for (int i = 1; i <= 30; i++)
                    {
                        var list = new List<TagHistoryValue122>();
                        for (int j = 0; j < 17280; j++)
                        {
                            list.Add(new TagHistoryValue122()
                            {
                                TagId = (from tag in tags where tag.TagName.Equals(t) select tag.Id).First(),
                                Val = Math.Round((ran.NextDouble() + 0.03) * 100, 2),
                                Ts = DateTime.Now.AddSeconds(-j * i * 5),
                                Quality = TagValueQuality.Good
                            });
                        }
                        var a = SqlSugarHelper122.Db.Insertable(list).ExecuteCommand();
                        Console.WriteLine($@"tag:{t} insert:{a}");
                        sum += a;
                    }
                });
                return default;
            });
            Console.WriteLine($@"sum insert:{sum}");
        }

        private static DateTime TimeSpanToDateTime(long span)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            time = startTime.AddMilliseconds(span);
            return time;
        }
    
        public class Rootobject
        {
            public string[] tags { get; set; }
        }
    }
    

}
