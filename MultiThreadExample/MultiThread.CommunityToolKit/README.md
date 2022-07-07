# MultiThread.CommunityToolKit社区工具包
并行处理快速工具包 
1. 帮助您在编写快速编写并行算法
## Definition
Namespace: MultiThread.CommunityToolKit
Assembly: MultiThread.CommunityToolKit.dll



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


StopwatchJobHelper.Do(() =>
{
    var newList = MultiThreadJobHelper.Do(3, list, (r) =>
    {
        r.ToList().ForEach(
            r =>
            {
                Console.WriteLine(r);
                Thread.Sleep(100);//模拟每个人物耗时100ms
            }
        );
        return r.ToList();
    });
}, logger, "多线程");

Console.ReadLine();

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
100个任务每个任务耗时100ms开启3线程是单线程效率三倍

![](.\Images\1.png)