#region Using
using Spawn.HDT.DustUtility.UI.ViewModels;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.Services.Providers
{
    public class WindowProvider : IWindowService
    {
        #region Member Variables
        private Window m_window;
        #endregion

        #region Show
        public void Show<T>(Window owner = null) where T : Window, new()
        {
            InitializeWindow<T>(owner);

            m_window?.Show();
        }
        #endregion

        #region ShowDialog
        public bool ShowDialog<T>(Window owner = null) where T : Window, new()
        {
            InitializeWindow<T>(owner);

            return (m_window.ShowDialog() ?? false);
        }
        #endregion

        #region GetResult
        public T GetResult<T>()
        {
            T retVal = default(T);

            if (m_window?.DataContext is IResultProvider<T> resultProvider)
            {
                retVal = resultProvider.GetResult();
            }
            else { }

            return retVal;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            m_window = null;
        }
        #endregion

        #region InitializeWindow
        private void InitializeWindow<T>(Window owner) where T : Window, new()
        {
            m_window = new T();

            if (owner != null)
            {
                m_window.Owner = owner;
            }
            else { }

            (m_window.DataContext as ViewModelBase).Initialize();
        }
        #endregion
    }
}