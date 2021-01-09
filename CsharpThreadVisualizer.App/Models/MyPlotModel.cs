using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;

namespace CsharpThreadVisualizer.App.Models
{
    class MyPlotModel : PlotModel
    {
        public int AxisMilliSecondMax { get; } = 60_000;
        public int AxisThreadMax { get; } = 255;

        public MyPlotModel()
        {
            SetAxis();
        }

        private void SetAxis()
        {
            //this.Title = "Thread execution status";

            var linearAxisHori = new LinearAxis
            {
                Title = "Elapsed Time (msec)",
                Position = AxisPosition.Top,
                Minimum = 0,
                Maximum = AxisMilliSecondMax,
                MajorGridlineStyle = LineStyle.Solid,   // 軸の長い目盛り線の種類
                MinorGridlineStyle = LineStyle.Dot,     // 軸の短い目盛り線の種類
            };
            this.Axes.Add(linearAxisHori);

            var linearAxisVert = new LinearAxis
            {
                Title = "Thread Id",
                Minimum = 0,
                Maximum = AxisThreadMax,
                StartPosition = 1,
                EndPosition = 0,
                MajorGridlineStyle = LineStyle.Solid,   // 軸の長い目盛り線の種類
                MinorGridlineStyle = LineStyle.Dot,     // 軸の短い目盛り線の種類
            };
            this.Axes.Add(linearAxisVert);
        }

        public void AddArrows(IEnumerable<TaskLog> logs, in DateTime baseTime)
        {
            foreach (var log in logs)
            {
                var arrow = GetArrowAnnotation(log, baseTime);
                this.Annotations.Add(arrow);
            }
            this.InvalidatePlot(true);   // Refresh

            static ArrowAnnotation GetArrowAnnotation(TaskLog log, in DateTime baseTime)
            {
                var taskId = log.Id;
                var threadId = log.ThreadId;
                var start = (log.StartTime - baseTime).TotalMilliseconds;
                var end = (log.EndTime - baseTime).TotalMilliseconds;
                ref var color = ref OxyColorsExtension.GetOxyColor(threadId);

                return new ArrowAnnotation
                {
                    Color = color,
                    TextColor = color,
                    Text = "Task:" + taskId.ToString() + " Thread:" + threadId.ToString(),
                    //Text = OxyColorsExtension.GetName(threadId),
                    StartPoint = new DataPoint(start, threadId),
                    EndPoint = new DataPoint(end, threadId),
                };
            }
        }

    }
}
