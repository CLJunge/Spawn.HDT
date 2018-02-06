#region Using
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.UI.ViewModels;
using System.Collections.Generic;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.Services.Providers
{
    public class WindowProvider : IWindowService
    {
        #region Member Variables
        private Dictionary<int, Window> m_dWindows;
        private int m_nCurrentWindowKey;
        #endregion

        #region Ctor
        public WindowProvider()
        {
            m_dWindows = new Dictionary<int, Window>();

            m_nCurrentWindowKey = -1;
        }
        #endregion

        #region Show
        public void Show<T>(int nKey, Window owner = null) where T : Window, new()
        {
            if (!m_dWindows.ContainsKey(nKey))
            {
                InitializeWindow<T>(nKey, owner);
            }
            else { }

            m_dWindows[nKey]?.Show();
        }
        #endregion

        #region ShowDialog
        public bool ShowDialog<T>(int nKey, Window owner = null) where T : Window, new()
        {
            InitializeWindow<T>(nKey, owner);

            return (m_dWindows[nKey].ShowDialog() ?? false);
        }
        #endregion

        #region GetInstance
        public T GetInstance<T>(int nKey = -1) where T : Window
        {
            if (nKey == -1)
            {
                nKey = m_nCurrentWindowKey;
            }
            else { }

            return (T)m_dWindows[nKey];
        }
        #endregion

        #region GetResult
        public T GetResult<T>(int nKey = -1)
        {
            T retVal = default(T);

            if (nKey == -1)
            {
                nKey = m_nCurrentWindowKey;
            }
            else { }

            if (m_dWindows.ContainsKey(nKey) && m_dWindows[nKey].DataContext is IResultProvider<T> resultProvider)
            {
                retVal = resultProvider.GetResult();
            }
            else { }

            return retVal;
        }
        #endregion

        #region IsVisible
        public bool IsVisible(int nKey = -1)
        {
            bool blnRet = false;

            if (nKey == -1)
            {
                nKey = m_nCurrentWindowKey;
            }
            else { }

            if (m_dWindows.ContainsKey(nKey))
            {
                blnRet = m_dWindows[nKey].Visibility == Visibility.Visible;
            }
            else { }

            return blnRet;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (m_dWindows.ContainsKey(m_nCurrentWindowKey))
            {
                try
                {
                    m_dWindows[m_nCurrentWindowKey].Close();
                }
                catch (System.Exception e)
                {
                    Log.WriteLine($"Exception occured while trying to close window ({m_nCurrentWindowKey}): {e}", LogType.Debug);
                }

                m_dWindows.Remove(m_nCurrentWindowKey);

                m_nCurrentWindowKey = -1;
            }
            else { }
        }

        public void Dispose(int nKey)
        {
            int nCurrentKey = m_nCurrentWindowKey;

            m_nCurrentWindowKey = nKey;

            Dispose();

            m_nCurrentWindowKey = nCurrentKey;
        }
        #endregion

        #region InitializeWindow
        private void InitializeWindow<T>(int nKey, Window owner) where T : Window, new()
        {
            T window = new T();

            if (owner != null)
            {
                window.Owner = owner;
            }
            else { }

            (window.DataContext as ViewModelBase).Initialize();

            m_dWindows.Add(nKey, window);

            m_nCurrentWindowKey = nKey;
        }
        #endregion
    }
}