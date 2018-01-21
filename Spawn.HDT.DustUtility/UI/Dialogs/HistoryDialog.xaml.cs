using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.Offline;
using System.Collections.Generic;

namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class HistoryDialog
    {
        #region Member Variables
        private Account m_account;
        #endregion

        #region Ctor
        public HistoryDialog()
        {
            InitializeComponent();
        }

        public HistoryDialog(Account account)
            : this()
        {
            m_account = account;

            grid.GridItems.Clear();

            grid.AddDateColumn();
        }
        #endregion

        #region Events
        #region OnWindowLoaded
        private void OnWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            List<CachedCardEx> lstHistory = HistoryManager.GetHistory(m_account);

            for (int i = 0; i < lstHistory.Count; i++)
            {
                grid.GridItems.Add(DataGridCardItemEx.FromCardWrapperEx(new Search.CardWrapperEx(lstHistory[i])));
            }

            Log.WriteLine($"Loaded history: {lstHistory.Count} entries", LogType.Debug);
        }
        #endregion

        #region OnClearClick
        private void OnClearClick(object sender, System.Windows.RoutedEventArgs e)
        {
            grid.GridItems.Clear();

            HistoryManager.ClearHistory(m_account);
        }
        #endregion

        #region OnRowDeleted
        private void OnRowDeleted(object sender, DataGridCardItemEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                HistoryManager.RemoveItemAt(m_account, e.RowIndex);
            }
            else { }
        }
        #endregion
        #endregion
    }
}