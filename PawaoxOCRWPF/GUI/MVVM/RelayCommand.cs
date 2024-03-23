using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PawaoxOCRWPF.GUI.MVVM
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Returns true if the command can be executed
        /// </summary>
        /// <param name="parameter">Is always ignored</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter">Is always ignored</param>
        public void Execute(object parameter = null)
        {
            if (CanExecute(parameter) && _execute != null)
                _execute.Invoke();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            if (parameter != null && !(parameter is T))
                return false;

            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter = null)
        {
            if (CanExecute(parameter) && _execute != null && (parameter == null || parameter is T))
                _execute.Invoke((T)parameter);
        }
    }
}
