using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.Offline;
using Spawn.HDT.DustUtility.UI;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Spawn.HDT.DustUtility.ViewModel
{
    public class HistoryDialogViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
        #endregion

        #region Properties
        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion

        #region CardItems
        public ObservableCollection<DataGridCardItemEx> CardItems { get; set; }
        #endregion

        #region ClearHistoryCommand
        public ICommand ClearHistoryCommand => new RelayCommand(ClearHistory);
        #endregion
        #endregion

        #region Ctor
        public HistoryDialogViewModel()
        {
            WindowTitle = "Dust Utility - History";

            CardItems = new ObservableCollection<DataGridCardItemEx>();

            List<CachedCardEx> lstHistory = HistoryManager.GetHistory(DustUtilityPlugin.CurrentAccount);

            for (int i = 0; i < lstHistory.Count; i++)
            {
                CardItems.Add(DataGridCardItemEx.FromCardWrapperEx(new CardWrapperEx(lstHistory[i])));
            }

            Log.WriteLine($"Loaded history: {lstHistory.Count} entries", LogType.Debug);
        }
        #endregion

        #region RemoveItem
        public void RemoveItem(DataGridCardItemEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                HistoryManager.RemoveItemAt(DustUtilityPlugin.CurrentAccount, e.RowIndex);
            }
            else { }
        }
        #endregion

        #region ClearHistory
        private void ClearHistory()
        {
            CardItems.Clear();

            HistoryManager.ClearHistory(DustUtilityPlugin.CurrentAccount);
        }
        #endregion
    }
}