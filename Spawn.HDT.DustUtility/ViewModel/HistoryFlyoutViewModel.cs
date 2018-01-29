#region Using
using GalaSoft.MvvmLight.CommandWpf;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.UI;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.ViewModel
{
    public class HistoryFlyoutViewModel : ViewModelBase
    {
        #region Properties
        #region CardItems
        public ObservableCollection<CardItem> CardItems { get; set; }
        #endregion

        #region ClearHistoryCommand
        public ICommand ClearHistoryCommand => new RelayCommand(ClearHistory, (() => CardItems.Count > 0));
        #endregion
        #endregion

        #region Ctor
        public HistoryFlyoutViewModel()
        {
            CardItems = new ObservableCollection<CardItem>();
        }
        #endregion

        #region LoadHistory
        public void LoadHistory()
        {
            if (ReloadRequired || Core.Game.IsRunning)
            {
                CardItems.Clear();

                List<CachedHistoryCard> lstHistory = HistoryManager.GetHistory(DustUtilityPlugin.CurrentAccount);

                for (int i = 0; i < lstHistory.Count; i++)
                {
                    CardItems.Add(new CardItem(new CardWrapper(lstHistory[i])) { ColoredCount = true });
                }

                if (lstHistory.Count > 0)
                {
                    Log.WriteLine($"Loaded history: {lstHistory.Count} entries", LogType.Debug);
                }
                else
                {
                    Log.WriteLine($"No history available", LogType.Debug);
                }

                ReloadRequired = false;
            }
            else
            {
                Log.WriteLine($"No reloading required", LogType.Debug);
            }
        }
        #endregion

        #region RemoveCardItem
        public void RemoveCardItem(CardItemEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                CardItems.RemoveAt(e.RowIndex);

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