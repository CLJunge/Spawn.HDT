using System;
using System.Windows.Input;

namespace Spawn.HDT.DustUtility.Mvvm
{
    public class RelayCommand : ICommand
    {
        #region Member Variables
        private Action m_targetMethod;
        private Func<bool> m_canExecuteMethod;
        #endregion

        #region Ctor
        public RelayCommand(Action method)
        {
            m_targetMethod = method;
        }

        public RelayCommand(Action method, Func<bool> canExecuteMethod)
            : this(method)
        {
            m_canExecuteMethod = canExecuteMethod;
        }
        #endregion

        #region CanExecuteChanged
        public event EventHandler CanExecuteChanged;
        #endregion

        #region CanExecute
        public bool CanExecute(object parameter)
        {
            bool blnRet = m_targetMethod != null;

            if (m_canExecuteMethod != null)
            {
                blnRet = m_canExecuteMethod();
            }
            else { }

            return blnRet;
        }
        #endregion

        #region Execute
        public void Execute(object parameter)
        {
            m_targetMethod?.Invoke();
        }
        #endregion

        #region RaiseCanExecuteChanged
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
        #endregion
    }

    public class RelayCommand<T> : ICommand
    {
        #region Member Variables
        private Action<T> m_targetMethod;
        private Func<T, bool> m_canExecuteMethod;
        #endregion

        #region Ctor
        public RelayCommand(Action<T> method)
        {
            m_targetMethod = method;
        }

        public RelayCommand(Action<T> method, Func<T, bool> canExecuteMethod)
            : this(method)
        {
            m_canExecuteMethod = canExecuteMethod;
        }
        #endregion

        #region CanExecuteChanged
        public event EventHandler CanExecuteChanged;
        #endregion

        #region CanExecute
        public bool CanExecute(object parameter)
        {
            bool blnRet = m_targetMethod != null;

            if (m_canExecuteMethod != null)
            {
                blnRet = m_canExecuteMethod((T)parameter);
            }
            else { }

            return blnRet;
        }
        #endregion

        #region Execute
        public void Execute(object parameter)
        {
            m_targetMethod?.Invoke((T)parameter);
        }
        #endregion

        #region RaiseCanExecuteChanged
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
        #endregion
    }
}