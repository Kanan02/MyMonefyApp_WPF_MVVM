using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonefyWPF.Service.Command
{
    public class RelayCommand : ICommand
    {
        static Func<bool> defaultCanExecuteMethod = () => true;
        Func<bool> canExecuteMethod;

        Action<object> executeMethod;

        public event EventHandler CanExecuteChanged;

        public virtual void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public void RiseCanExecuteChange()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }
        public RelayCommand(Action<object> executeMethod) : this(executeMethod, defaultCanExecuteMethod)
        {

        }
        public RelayCommand(Action<object> executeMethod, Func<bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }
        public void Execute(object par)
        {
            executeMethod(par);
        }
        public bool CanExecute()
        {
            return canExecuteMethod();
        }

        bool ICommand.CanExecute(object parameter)
        {

            return CanExecute();
        }
    }
}
