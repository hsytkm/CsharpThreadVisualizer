using CsharpThreadVisualizer.App.Common;
using CsharpThreadVisualizer.App.Models;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CsharpThreadVisualizer.App
{
    class MainWindowViewModel : MinedableBase
    {
        private const int _taskCount = 3;
        private const int _taskSleepMsec = 3000;

        private const int _axisSecondMax = 60;
        private const int _axisThreadMax = 255;

        public PlotModel MyModel { get; }

        public ICommand RunTasksCommand => _runTasksCommand ??= new MyCommand(async () =>
            {
                var baseTime = DateTime.Now;
                var tasks = Enumerable.Range(0, _taskCount).Select(x => SomeAsync(x));
                var logs = await Task.WhenAll(tasks);

                AddArrows(logs);

                static Task<TaskLog> SomeAsync(int taskId)
                {
                    return Task.Run(() =>
                    {
                        var startTime = DateTime.Now;
                        var threadId = Thread.CurrentThread.ManagedThreadId;

                        Thread.Sleep(_taskSleepMsec);

                        var endTime = DateTime.Now;
                        return new TaskLog(taskId, threadId, startTime, endTime);
                    });
                }
            });
        private ICommand _runTasksCommand = default!;

        public MainWindowViewModel()
        {
            MyModel = CreatePlotModel();
        }

        private void AddArrows(IEnumerable<TaskLog> logs)
        {
            if (MyModel is null) return;

            foreach (var log in logs)
            {
                var arrow = GetArrowAnnotation(log);
                MyModel.Annotations.Add(arrow);
            }

            MyModel.InvalidatePlot(true);   // Refresh
        }

        private static ArrowAnnotation GetArrowAnnotation(TaskLog taskLog)
        {
            var taskId = taskLog.Id;
            var threadId = taskLog.ThreadId;
            var start = 5;
            var end = 8;

            return new ArrowAnnotation
            {
                Color = OxyColorsExtension.FromIndex(threadId),
                StartPoint = new DataPoint(start, threadId),
                EndPoint = new DataPoint(end, threadId),
                Text = "Task=" + taskId.ToString() + "Thread=" + threadId.ToString(),
            };
        }

        private static PlotModel CreatePlotModel()
        {
            var pm = new PlotModel
            {
                Title = "Thread execution status",
            };

            var linearAxis1 = new LinearAxis
            {
                Title = "Elapsed Time (sec)",
                Position = AxisPosition.Top,
                Minimum = 0,
                Maximum = _axisSecondMax,
            };
            pm.Axes.Add(linearAxis1);

            var linearAxis2 = new LinearAxis
            {
                Title = "Thread Id",
                Minimum = 0,
                Maximum = _axisThreadMax,
                StartPosition = 1,
                EndPosition = 0
            };
            pm.Axes.Add(linearAxis2);

            return pm;
        }
    }
}
