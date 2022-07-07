using System.Collections.Concurrent;

namespace MultiThread.CommunityToolKit
{
    public static class MultiThreadJobHelper
    {
        /// <summary>
        /// 多线程分解任务
        /// </summary>
        /// <typeparam name="T">任务数据类型</typeparam>
        /// <param name="taskCount">线程数</param>
        /// <param name="jobs">任务集合</param>
        /// <param name="func">任务处理方法</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Do<T>(int taskCount,IEnumerable<T> jobs, Func<IEnumerable<T>, IEnumerable<T>> func)
        {
            var result = new ConcurrentBag<T>();
            if (result == null) throw new ArgumentNullException(nameof(result));
            Task[] taskArray = new Task[taskCount];
            var enumerable = jobs.ToList();
            var jobCount = enumerable.Count() / taskCount == 0 ? enumerable.Count() : enumerable.Count() / taskCount;
            for (int i = 0; i < taskArray.Length; i++)
            {
                var perJobCount = jobCount;
                if (i == taskArray.Length - 1)//最后一个线程分配任务数
                    perJobCount = enumerable.Count() - i * jobCount;

                var perAlarm = enumerable.Skip(i * jobCount).Take(perJobCount);
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    var l = perAlarm.ToList();
                    foreach (var x1 in func(l))
                    {
                      result.Add(x1);  
                    }
                });
            }
            Task.WaitAll(taskArray);
            return result;
        }
    }
}