using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpThreadVisualizer.App
{
    //class TaskItem
    //{
    //    public int Id { get; }
    //    public int ThreadId { get; }
    //    public double Progress { get; private set; }
    //    public DateTime StartTime { get; }

    //    public TaskItem(int id)
    //    {
    //        Id = id;
    //        ThreadId = Thread.CurrentThread.ManagedThreadId;
    //        Progress = 0;
    //        StartTime = DateTime.Now;
    //    }

    //    //public TaskItem Create(int id) => new TaskItem(id);

    //    public void UpdateProgress(double p) => Progress = Math.Max(0, Math.Min(1, p));
    //}

    record TaskLog
    {
        public int Id { get; }
        public int ThreadId { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public TaskLog(int id, int threadId, DateTime startTime, DateTime endTime)
            => (Id, ThreadId, StartTime, EndTime) = (id, threadId, startTime, endTime);
    }
}
