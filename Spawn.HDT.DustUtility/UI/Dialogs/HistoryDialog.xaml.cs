using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.Offline;
using System.Collections.Generic;

namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class HistoryDialog
    {
        #region Ctor
        public HistoryDialog()
        {
            InitializeComponent();
        }

        public HistoryDialog(Account account)
            : this()
        {
            grid.GridItems.Clear();

            grid.AddDateColumn();

            List<CachedCardEx> lstHistory = HistoryManager.GetHistory(account);

            for (int i = 0; i < lstHistory.Count; i++)
            {
                grid.GridItems.Add(DataGridCardItemEx.FromCardWrapperEx(new Search.CardWrapperEx(lstHistory[i])));
            }

            Log.WriteLine($"Loaded history: {lstHistory.Count} entries", LogType.Debug);
        }
        #endregion
    }
}