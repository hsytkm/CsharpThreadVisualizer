using CsharpThreadVisualizer.App.Common;
using CsharpThreadVisualizer.App.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CsharpThreadVisualizer.App
{
    class MainWindowViewModel : MinedableBase
    {
        private const int MaximumThreadMin = 1;
        private const int MaximumThreadMax = 255;
        private const int MaximumThreadDefault = 30;

        private const int MaximumMSecMin = 1;
        private const int MaximumMSecMax = 600_000;
        private const int MaximumMSecDefault = 5000;

        private const int TaskCountMin = 1;
        private const int TaskCountMax = 10_000;
        private const int TaskCountDefault = 100;

        private const int TaskDelayMsecMin = 1;
        private const int TaskDelayMsecMax = 10_000;
        private const int TaskDelayMsecDefault = 100;

        private readonly MyTasks _myTasks = App.Current.MyTasks;

        private static int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));

        public bool IsNotStarted
        {
            get => _isNotStarted;
            set => SetProperty(ref _isNotStarted, value);
        }
        private bool _isNotStarted = true;

        #region PlotModel
        [Required(ErrorMessage = "Required")]
        [Range(MaximumThreadMin, MaximumThreadMax, ErrorMessage = "Out of range.")]
        public int? MaximumThread
        {
            get => _maximumThread;
            set
            {
                if (!value.HasValue) return;

                int x = Clamp(value.Value, MaximumThreadMin, MaximumThreadMax);
                if (SetProperty(ref _maximumThread, x))
                {
                    MyModel.AxisThreadMax = x;
                }
            }
        }
        private int? _maximumThread;

        [Required(ErrorMessage = "Required")]
        [Range(MaximumMSecMin, MaximumMSecMax, ErrorMessage = "Out of range.")]
        public int? MaximumMSec
        {
            get => _maximumMSec;
            set
            {
                if (!value.HasValue) return;

                int x = Clamp(value.Value, MaximumMSecMin, MaximumMSecMax);
                if (SetProperty(ref _maximumMSec, x))
                {
                    MyModel.AxisMilliSecondMax = x;
                }
            }
        }
        private int? _maximumMSec;

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    MyModel.IsDarkTheme = value;
                }
            }
        }
        private bool _isDarkTheme;
        #endregion

        #region MyTasks
        [Required(ErrorMessage = "Required")]
        [Range(TaskCountMin, TaskCountMax, ErrorMessage = "Out of range.")]
        public int? TaskCount
        {
            get => _taskCount;
            set
            {
                if (!value.HasValue) return;

                int x = Clamp(value.Value, TaskCountMin, TaskCountMax);
                if (SetProperty(ref _taskCount, x))
                {
                    _myTasks.TaskCount = x;
                }
            }
        }
        private int? _taskCount;

        [Required(ErrorMessage = "Required")]
        [Range(TaskDelayMsecMin, TaskDelayMsecMax, ErrorMessage = "Out of range.")]
        public int? TaskDelayMsec
        {
            get => _taskDelayMsec;
            set
            {
                if (!value.HasValue) return;

                int x = Clamp(value.Value, TaskDelayMsecMin, TaskDelayMsecMax);
                if (SetProperty(ref _taskDelayMsec, x))
                {
                    _myTasks.TaskSleepMsec = x;
                }
            }
        }
        private int? _taskDelayMsec;
        #endregion

        #region Commands
        public ICommand RunTasksCommand => _runTasksCommand ??= new MyCommand(() =>
        {
            IsNotStarted = false;

            var schedule = TaskScheduler.FromCurrentSynchronizationContext();
            var baseTime = DateTime.Now;

            var tasks = _myTasks.GetTasks()
                .Select(task =>
                {
                    _ = task.ContinueWith(t => MyModel.AddArrow(baseTime, t.Result), schedule);
                    return task;
                })
                .ToArray();

            // 測定用のTask(Delay)の裏で、Arrow描画Task走るので測定に影響出てそう…
            foreach (var task in tasks)
            {
                task.Start();   // 投げっぱなし
            }
        });
        private ICommand _runTasksCommand = default!;

        public ICommand RunTasksAsyncCommand => _runTasksAsyncCommand ??= new MyCommand(async () =>
        {
            IsNotStarted = false;

            var baseTime = DateTime.Now;
            var tasks = _myTasks.GetTasks().ToArray();
            foreach (var task in tasks) task.Start();
            var logs = await Task.WhenAll(tasks);

            // 全完了後にグラフを更新
            MyModel.AddArrows(baseTime, logs);
        });
        private ICommand _runTasksAsyncCommand = default!;
        #endregion

        public MyPlotModel MyModel { get; } = new MyPlotModel();

        public int WorkerThreadsMin { get; }
        public int CompletionPortThreadsMin { get; }

        public MainWindowViewModel()
        {
            ThreadPool.GetMaxThreads(out var workerThreads0, out var completionPortThreads0);
            Debug.WriteLine($"ThreadPool のワーカースレッドの最大数 = {workerThreads0}");
            Debug.WriteLine($"ThreadPool の非同期 I/O スレッドの最大数 = {completionPortThreads0}");

            ThreadPool.GetAvailableThreads(out var workerThreads1, out var completionPortThreads1);
            Debug.WriteLine($"ThreadPool のワーカースレッドの利用可能数 = {workerThreads1}");
            Debug.WriteLine($"ThreadPool の非同期 I/O スレッドの利用可能数 = {completionPortThreads1}");

            ThreadPool.GetMinThreads(out var workerThreadsMin, out var completionPortThreadsMin);
            WorkerThreadsMin = workerThreadsMin;
            CompletionPortThreadsMin = completionPortThreadsMin;
            Debug.WriteLine($"ThreadPool のワーカースレッドの最小数 = {workerThreadsMin}");
            Debug.WriteLine($"ThreadPool の非同期 I/O スレッドの最小数 = {completionPortThreadsMin}");

            // Model Update
            MaximumThread = MaximumThreadDefault;
            MaximumMSec = MaximumMSecDefault;
            IsDarkTheme = true;

            TaskCount = TaskCountDefault;
            TaskDelayMsec = TaskDelayMsecDefault;
        }
    }
}
