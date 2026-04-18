using System.Windows.Input;

namespace ViewModel;

public class RelayCommand : ICommand
{
    private readonly Action _action;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Action action, Func<bool>? canExecute = null)
    {
        _action = action;
        _canExecute = canExecute;
    }
    public bool CanExecute(object? parameter) => _canExecute == null ||  _canExecute();
    public void Execute(object? parameter) => _action();
    public event EventHandler? CanExecuteChanged;
    
}