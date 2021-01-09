using System;
using System.Windows.Input;

namespace CsharpThreadVisualizer.App.Common
{
    class MyCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;
        public event EventHandler? CanExecuteChanged;

        public MyCommand(Action execute, Func<bool>? canExecute = null)
            => (_execute, _canExecute) = (execute, canExecute);

        public void Execute(object? parameter) => _execute();

        public bool CanExecute(object? parameter) => (_canExecute is null) || _canExecute();

        public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
