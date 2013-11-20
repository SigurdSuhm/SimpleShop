#region Using Statements

using System;
using System.Windows.Input;

#endregion

namespace SimpleShop.Helpers
{
    /// <summary>
    /// Relay command class for binding UI commands to ViewModel methods.
    /// </summary>
    internal class RelayCommand : ICommand
    {
        #region Fields

        // Action to execute
        private readonly Action<object> execute;
        // Predicate for determining if the action can execute
        private readonly Predicate<object> canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        /// <param name="canExecute">Predicate for determining if the action can execute.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Returns a boolean value indicating if the action can execute.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True if the command can execute.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        /// <summary>
        /// Event for when the value of CanExecute changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the relay command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }

        #endregion
    }
}