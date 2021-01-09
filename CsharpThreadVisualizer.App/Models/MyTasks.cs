using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpThreadVisualizer.App.Models
{
    class MyTasks
    {
        public int TaskCount { get; } = 10;
        public int TaskSleepMsec { get; } = 2000;

        public async Task<TaskLog[]> RunTasksAsync()
        {
            var tasks = Enumerable.Range(0, TaskCount)
                .Select(x => SomeAsync(x, TaskSleepMsec));

            return await Task.WhenAll(tasks);
        }

        private static Task<TaskLog> SomeAsync(int taskId, int sleepMsec)
        {
            return Task.Run(() =>
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var startTime = DateTime.Now;

                Thread.Sleep(sleepMsec);

                var endTime = DateTime.Now;
                return new TaskLog(taskId, threadId, startTime, endTime);
            });
        }

    }
}
