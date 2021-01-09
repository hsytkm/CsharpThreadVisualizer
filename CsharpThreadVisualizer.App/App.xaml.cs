using CsharpThreadVisualizer.App.Models;
using System.Windows;

namespace CsharpThreadVisualizer.App
{
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;

        internal MyTasks MyTasks { get; } = new MyTasks();
    }
}
