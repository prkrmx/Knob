using System;
using System.Windows.Input;

namespace KnobUI.Helpers
{
    public class RelayCommand : ICommand
    {
        Action _TargetExecuteMethod;
        Action<object> _TargetExecuteMethodOB;

        Func<bool> _TargetCanExecuteMethod;

        public RelayCommand(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public RelayCommand(Action<object> executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethodOB = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public RelayCommand(Action<object> executeMethod)
        {
            _TargetExecuteMethodOB = executeMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);

        }

        bool ICommand.CanExecute(object parameter)
        {

            if (_TargetCanExecuteMethod != null)
            {
                return _TargetCanExecuteMethod();
            }

            if (_TargetExecuteMethod != null || _TargetExecuteMethodOB != null)
            {
                return true;
            }

            return false;
        }

        // Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command

        // Prism commands solve this in their implementation public event 
        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod();
            }
            if (_TargetExecuteMethodOB != null)
            {
                _TargetExecuteMethodOB(parameter);
            }
        }
    }
}
