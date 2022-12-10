﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AoC.Main
{
	/// <summary>
	/// A command whose sole purpose is to relay its functionality to other objects by invoking delegates.
	/// The default return value for the CanExecute method is 'true'.
	/// </summary>
	/// <remarks>
	/// This came from http://msdn.microsoft.com/en-us/magazine/dd419663.aspx: WPF Apps With The Model-View-ViewModel Design Pattern.
	/// </remarks>
	public class RelayCommand : ICommand
	{
		#region Fields

		readonly Action<object> _execute;
		readonly Predicate<object> _canExecute;

		#endregion // Fields
		#region Constructors

		/// <summary>
		/// Creates a new command that can always execute.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		public RelayCommand(Action<object> execute)
			: this(execute, null)
		{
		}

		/// <summary>
		/// Creates a new command.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = execute;
			_canExecute = canExecute;
		}

		#endregion // Constructors
		#region ICommand Members

		//[DebuggerStepThrough]
		/// <summary>
		/// Can Execute?
		/// </summary>
		/// <param name="parameters">parameters</param>
		/// <returns><c>true</c> if the command can execute; otherwise <c>false</c>.</returns>
		public bool CanExecute(object parameters)
		{
			return _canExecute == null ? true : _canExecute(parameters);
		}

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
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

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command.  If the command does not require data to be passed, this object can be set to null.
		/// </param>
		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		#endregion // ICommand Members
	}
}
