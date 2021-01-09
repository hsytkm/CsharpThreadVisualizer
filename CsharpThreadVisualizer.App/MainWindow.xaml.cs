using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CsharpThreadVisualizer.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var tasks = Enumerable.Range(0, 3).Select(x => SomeAsync(x));

            await Task.WhenAll(tasks);
        }

        private static Task SomeAsync(int taskId)
        {
            int totalMsec = 3000;

            return Task.Run(() =>
            {
                var startTime = DateTime.Now;
                var threadId = Thread.CurrentThread.ManagedThreadId;

                //Debug.WriteLine($"TaskId : {taskId} , ThreadId : {Thread.CurrentThread.ManagedThreadId}");

                Thread.Sleep(totalMsec);

                var endTime = DateTime.Now;
                var log = new TaskLog(taskId, threadId, startTime, endTime);
            });
        }
    }
}
