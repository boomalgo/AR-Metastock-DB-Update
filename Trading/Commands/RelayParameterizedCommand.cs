namespace Trading.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Executes a method that is passed as a parameter in the constructor
    /// </summary>
    public class RelayParameterizedCommand : ICommand
    {
        /// <summary>
        /// Reference to the method to execute
        /// </summary>
        private readonly Action<object> executeMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayParameterizedCommand"/> class.
        /// </summary>
        /// <param name="eventHandler">
        /// The event handler.
        /// </param>
        public RelayParameterizedCommand(Action<object> eventHandler)
        {
            this.executeMethod = eventHandler;
        }

        /// <summary>
        /// Notifies whether CanExecute has changed
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Always can be executed
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// Always returns true
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Invokes the command
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            this.executeMethod.Invoke(parameter);
        }
    }
}
