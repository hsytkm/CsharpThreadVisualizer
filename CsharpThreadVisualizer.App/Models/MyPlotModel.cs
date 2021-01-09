using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using System;
using System.Linq;

namespace CsharpThreadVisualizer.App.Models
{
    class MyPlotModel : PlotModel
    {
        public int AxisMilliSecondMax
        {
            get => _axisMilliSecondMax;
            set
            {
                if (_axisMilliSecondMax != value)
                {
                    _axisMilliSecondMax = value;
                    var axis = this.Axes.FirstOrDefault(x => x.Position == AxisPosition.Top);
                    if (axis is not null)
                    {
                        axis.Maximum = value;
                        Refresh();
                    }
                }
            }
        }
        private int _axisMilliSecondMax;

        public int AxisThreadMax
        {
            get => _axisThreadMax;
            set
            {
                if (_axisThreadMax != value)
                {
                    _axisThreadMax = value;
                    var axis = this.Axes.FirstOrDefault(x => x.Position == AxisPosition.Left);
                    if (axis is not null)
                    {
                        axis.Maximum = value;
                        Refresh();
                    }
                }
            }
        }
        private int _axisThreadMax;

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (_isDarkTheme != value)
                {
                    _isDarkTheme = value;
                    this.PlotAreaBackground = value ? OxyColors.Gray : OxyColors.White;
                    Refresh();
                }
            }
        }
        private bool _isDarkTheme;

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
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = AxisThreadMax,
                StartPosition = 1,
                EndPosition = 0,
                MajorGridlineStyle = LineStyle.Solid,   // 軸の長い目盛り線の種類
                MinorGridlineStyle = LineStyle.Dot,     // 軸の短い目盛り線の種類
            };
            this.Axes.Add(linearAxisVert);
        }

        private void Refresh() => this.InvalidatePlot(true);   // Refresh

        public void AddArrow(in DateTime baseTime, TaskLog log)
        {
            var arrow = GetArrowAnnotation(baseTime, log);

            var lockObject = new object();
            lock (lockObject)
            {
                this.Annotations.Add(arrow);
            }
            Refresh();
        }

        public void AddArrows(in DateTime baseTime, TaskLog[] logs)
        {
            foreach (var log in logs)
            {
                var arrow = GetArrowAnnotation(baseTime, log);
                this.Annotations.Add(arrow);
            }
            Refresh();
        }

        private static ArrowAnnotation GetArrowAnnotation(in DateTime baseTime, TaskLog log)
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
                StartPoint = new DataPoint(start, threadId),
                EndPoint = new DataPoint(end, threadId),
                Text = "Task" + taskId.ToString() + "@" + threadId.ToString(),
                //Text = "Task:" + taskId.ToString() + " Thread:" + threadId.ToString() + OxyColorsExtension.GetName(threadId)
            };
        }

    }
}
