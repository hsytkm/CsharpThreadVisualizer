using System;

namespace CsharpThreadVisualizer.App.Models
{
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
