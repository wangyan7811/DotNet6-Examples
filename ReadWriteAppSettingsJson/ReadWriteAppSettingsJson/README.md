## 方法1：使用强类型
1. 复制appsettings.Development.json内容，将json粘贴为类
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

```
![](https://img2022.cnblogs.com/blog/599607/202206/599607-20220609085326236-1157359541.png)

2. 生成代码如下


```
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
        public string MicrosoftAspNetCore { get; set; }
    }
```
3. 强类型读取
```
var config = new ConfigurationBuilder()
                .SetBasePath(_basePath)
                .AddJsonFile(_appsettingsProductionJson)
                .Build();
var o = config.Get<Rootobject>();
            
```
**注意：**这里有个坑，json文件中key是可以带英文.的；如文件中Microsoft.AspNetCore项
但是生成的类字段是没有.的如对应生成类字段 MicrosoftAspNetCore ,直接读取会读不出值

![](https://img2022.cnblogs.com/blog/599607/202206/599607-20220609092030536-1348309867.png)

4. 使用字串key方式

```
var config = new ConfigurationBuilder()
                .SetBasePath(_basePath)
                .AddJsonFile(_appsettingsProductionJson)
                .Build();
 var p = config["Logging:LogLevel:Microsoft.AspNetCore"];
```

![](https://img2022.cnblogs.com/blog/599607/202206/599607-20220609093634700-1887335264.png)

5. 修改配置文件

```
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
```

