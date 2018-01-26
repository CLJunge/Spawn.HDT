using System.Windows;

namespace Spawn.HDT.DustUtility.Services.Providers
{
    public class DialogServiceProvider : IDialogService
    {
        #region Member Variables
        private Window m_dialog;
        #endregion

        #region ShowDialog
        public bool ShowDialog<T>(Window owner = null) where T : Window, new()
        {
            m_dialog = new T();

            if (owner != null)
            {
                m_dialog.Owner = owner;
            }
            else { }

            return (m_dialog?.ShowDialog().Value ?? false);
        }
        #endregion

        #region GetDialogResult
        public T GetDialogResult<T>()
        {
            T retVal = default(T);

            if (m_dialog?.DataContext is IDialogResultService<T> resultProvider)
            {
                retVal = resultProvider.GetDialogResult();
            }
            else { }

            return retVal;
        }
        #endregion
    }
}