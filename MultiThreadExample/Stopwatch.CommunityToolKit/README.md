# Stopwatch.CommunityToolKit社区工具包
记录执行方法耗时工具包 
1. 帮助您在编写快速日志记录方法执行时间
需自行实现日志方法记录程序耗时
## Definition
Namespace: Stopwatch.CommunityToolKit
Assembly: Stopwatch.CommunityToolKit.dll



## Examples
```C#
// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;
using MultiThread.CommunityToolKit;
using Stopwatch.CommunityToolKit;

Console.WriteLine("Hello, World!");
var logger = new MyLogger();
logger.LogInformation("haha");

var list = new List<int>();
for (int i = 0; i < 100; i++)
{
 list.Add(i);   
}

StopwatchJobHelper.Do(() =>
{
    list.ForEach(
        r =>
        {
            Console.WriteLine(r);
            Thread.Sleep(100);//模拟每个人物耗时100ms
        }
        );
   
}, logger, "单线程");

Console.ReadLine();
//自行实现日志
class MyLogger : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (logLevel==LogLevel.Information)
        {
            Console.WriteLine(state);
        }
       
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }
}
```
## 测试效果

可以看到对执行程序做了执行时间记录

![](.\Images\1.png)