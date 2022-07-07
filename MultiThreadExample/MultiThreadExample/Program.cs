// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;
using MultiThread.CommunityToolKit;
using Stopwatch.CommunityToolKit;

Console.WriteLine("Hello, World!");
var logger = new MyLogger();


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
            //Console.WriteLine(r);
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
                //Console.WriteLine(r);
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