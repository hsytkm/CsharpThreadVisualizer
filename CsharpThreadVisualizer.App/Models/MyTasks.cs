using CsharpThreadVisualizer.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpThreadVisualizer.App.Models
{
    class MyTasks
    {
        public int TaskCount { get; set; } = 10;
        public int TaskSleepMsec { get; set; } = 2000;

        public IEnumerable<Task<TaskLog>> GetTasks()
            => Enumerable.Range(0, TaskCount).Select(x => SomeTask(x, TaskSleepMsec));

        private static Task<TaskLog> SomeTask(int taskId, int sleepMsec)
            => new Task<TaskLog>(() =>
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var startTime = DateTime.Now;

#if true
                Thread.Sleep(sleepMsec);
#else
                int sum = 0;
                for (var i = 0; i < 10_000_000; ++i)
                {
                    sum += (i % 10);
                }
#endif

                var endTime = DateTime.Now;
                return new TaskLog(taskId, threadId, startTime, endTime);
            });

    }
}
