using Newtonsoft.Json;

namespace ReadWriteAppSettingsJson.Models
{
    
    public class Rootobject
    {
        public Logging Logging { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        [JsonProperty("Microsoft.AspNetCore")]
        public string MicrosoftAspNetCore { get; set; }
    }

}
