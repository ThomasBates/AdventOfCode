using System;
using System.Windows.Input;

namespace AoC.Main;

/// <summary>
/// A command whose sole purpose is to relay its functionality to other objects by invoking delegates.
/// The default return value for the CanExecute method is 'true'.
/// </summary>
/// <remarks>
/// This came from http://msdn.microsoft.com/en-us/magazine/dd419663.aspx: WPF Apps With The Model-View-ViewModel Design Pattern.
/// </remarks>
public class RelayCommand : ICommand
{
	readonly Action<object> execute;
	readonly Predicate<object> canExecute;

	public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
	{
		if (execute == null)
			throw new ArgumentNullException("execute");

		this.execute = execute;
		this.canExecute = canExecute;
	}

	public bool CanExecute(object parameters)
	{
		return canExecute == null || canExecute(parameters);
	}

	public event EventHandler CanExecuteChanged
	{
		add
		{
			CommandManager.RequerySuggested += value;
		}
		remove
		{
			CommandManager.RequerySuggested -= value;
		}
	}

	public void Execute(object parameter)
	{
		execute(parameter);
	}
}
