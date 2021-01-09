using CsharpThreadVisualizer.App.Common;
using CsharpThreadVisualizer.App.Models;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace CsharpThreadVisualizer.App
{
    class MainWindowViewModel : MinedableBase
    {
        private readonly MyTasks _myTasks = App.Current.MyTasks;

        public ICommand RunTasksCommand => _runTasksCommand ??= new MyCommand(async () =>
            {
                var baseTime = DateTime.Now;
                var logs = await _myTasks.RunTasksAsync();
                MyModel.AddArrows(logs, baseTime);
            });
        private ICommand _runTasksCommand = default!;

        public MyPlotModel MyModel { get; } = new MyPlotModel();

        public MainWindowViewModel()
        {
        }
    }
}
