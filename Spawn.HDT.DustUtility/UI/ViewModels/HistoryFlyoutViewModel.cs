#region Using
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
#if DEBUG
using Hearthstone_Deck_Tracker.Utility.Extensions;
#endif
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.Messaging;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class HistoryFlyoutViewModel : ViewModelBase
    {
        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => false;
        #endregion

        #region CardItems
        public ObservableCollection<CardItemModel> CardItems { get; set; }
        #endregion

        #region ClearHistoryCommand
        public ICommand ClearHistoryCommand => new RelayCommand(ClearHistory, (() => CardItems.Count > 0));
        #endregion
        #endregion

        #region Ctor
        public HistoryFlyoutViewModel()
        {
            CardItems = new ObservableCollection<CardItemModel>();

#if DEBUG
            if (IsInDesignMode)
            {
                InitializeAsync().Forget();
            }
            else { }
#endif

            Messenger.Default.Register<RemoveCardItemMessage>(this, RemoveCardItem);
        }
        #endregion

        #region Dtor
        ~HistoryFlyoutViewModel() => Messenger.Default.Unregister(this);
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            if (ReloadRequired)
            {
                CardItems.Clear();

                List<CachedHistoryCard> lstHistory = DustUtilityPlugin.CurrentAccount.GetHistory();

                for (int i = 0; i < lstHistory.Count; i++)
                {
                    CardWrapper wrapper = new CardWrapper(lstHistory[i]);

                    CardItemModel cardItem = new CardItemModel(wrapper)
                    {
                        ColoredCount = true
                    };

                    if (cardItem.Count > 0)
                    {
                        cardItem.Dust = wrapper.RawCard.GetCraftingCost();
                    }
                    else { }

                    CardItems.Add(cardItem);
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
                Log.WriteLine($"No reload required", LogType.Debug);
            }
        }
        #endregion

        #region RemoveCardItem
        private void RemoveCardItem(RemoveCardItemMessage message)
        {
            if ((message.FlyoutName?.Equals(DustUtilityPlugin.HistoryFlyoutName) ?? false) && message.EventArgs?.RowIndex > -1)
            {
                CardItems.RemoveAt(message.EventArgs.RowIndex);

                HistoryManager.RemoveItemAt(DustUtilityPlugin.CurrentAccount, message.EventArgs.RowIndex);
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