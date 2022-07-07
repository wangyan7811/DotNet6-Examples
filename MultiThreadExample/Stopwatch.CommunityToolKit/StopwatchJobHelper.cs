using Microsoft.Extensions.Logging;

namespace Stopwatch.CommunityToolKit
{
    public static class StopwatchJobHelper
    {
        /// <summary>
        /// 面向切面日志记录action耗时
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="action">被记录的操作过程</param>
        /// <param name="logger">日志接口控制反转</param>
        /// <param name="msg">自定义消息</param>
        public static void Do(Action action, ILogger logger,string msg)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            var timeSpan = sw.Elapsed;
            logger.LogInformation("[TIME-COST]：{message} {hours}:{minutes}:{seconds}.{milliseconds}-{ticks}", msg, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds, timeSpan.Ticks * 100 / 1000 % 1000);
        }
    }
}