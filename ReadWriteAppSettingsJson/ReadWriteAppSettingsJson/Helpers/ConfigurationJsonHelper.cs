using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReadWriteAppSettingsJson.Models;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace ReadWriteAppSettingsJson.Helpers
{
    public class ConfigurationJsonHelper: ICanReadWriteJson
    {
        private readonly string _appsettingsProductionJson = "appsettings.Development.json";
        private readonly string _basePath;
        private readonly IConfigurationRoot config;
        public ConfigurationJsonHelper()
        {
            _basePath = AppDomain.CurrentDomain.BaseDirectory;
            config = new ConfigurationBuilder()
                .SetBasePath(_basePath)
                .AddJsonFile(_appsettingsProductionJson, optional: true, reloadOnChange: true)
                .Build();
        }
        public Rootobject Read<Rootobject>()
        {
            
            var o = config.Get<Rootobject>();
            
            var p = config["Logging:LogLevel:Microsoft.AspNetCore"];
            return o;
        }

        public bool Write<Rootobject>(Rootobject entity)
        {
            config["Logging:LogLevel:Microsoft.AspNetCore"]="Information";
            var jsonString = File.ReadAllText(Path.Combine(_basePath,_appsettingsProductionJson), Encoding.UTF8);
            var jsonObject = JObject.Parse(jsonString);
            jsonObject["Logging"]["LogLevel"]["Microsoft.AspNetCore"] = config["Logging:LogLevel:Microsoft.AspNetCore"];
           
            var convertString = Convert.ToString(jsonObject);
            File.WriteAllText(Path.Combine(_basePath, _appsettingsProductionJson), convertString);
            return true;
        }
    }
}
