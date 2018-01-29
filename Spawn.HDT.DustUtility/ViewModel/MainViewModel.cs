#region Using
using GalaSoft.MvvmLight.CommandWpf;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
        private Visibility m_historyButtonVisibility;
        private Visibility m_switchAccountButtonVisibility;
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
        public ObservableCollection<CardItem> CardItems { get; set; }
        #endregion

        #region HistoryButtonVisibility
        public Visibility HistoryButtonVisibility
        {
            get => m_historyButtonVisibility;
            set => Set(ref m_historyButtonVisibility, value);
        }
        #endregion

        #region SwitchAccountButtonVisibility
        public Visibility SwitchAccountButtonVisibility
        {
            get => m_switchAccountButtonVisibility;
            set => Set(ref m_switchAccountButtonVisibility, value);
        }
        #endregion

        #region SwitchAccountCommand
        public ICommand SwitchAccountCommand => new RelayCommand(SwitchAccount);
        #endregion

        #region ShowHistoryCommand
        public ICommand ShowHistoryCommand => new RelayCommand<Flyout>(ShowHistory);
        #endregion

        #region ShowDecksCommand
        public ICommand ShowDecksCommand => new RelayCommand(ShowDecks);
        #endregion

        #region ShowCollectionInfoCommand
        public ICommand ShowCollectionInfoCommand => new RelayCommand(ShowCollectionInfo);
        #endregion
        #endregion

        #region Ctor
        public MainViewModel()
        {
            CardItems = new ObservableCollection<CardItem>();

            Account account = DustUtilityPlugin.CurrentAccount;

            if (!account.IsEmpty)
            {
                WindowTitle = $"Dust Utility [{account.BattleTag.Name} ({account.Region})]";
            }
            else { }

            if (DustUtilityPlugin.IsOffline)
            {
                WindowTitle = $"{WindowTitle} [OFFLINE]";
            }
            else { }

            if (IsInDesignMode)
            {
                WindowTitle = $"{WindowTitle} (Design)";
            }
            else
            {
#if DEBUG
                WindowTitle = $"{WindowTitle} (Debug)";
#endif
            }

            HistoryButtonVisibility = Visibility.Collapsed;
            SwitchAccountButtonVisibility = Visibility.Collapsed;

            if (DustUtilityPlugin.Config.OfflineMode)
            {
                HistoryButtonVisibility = Visibility.Visible;

                if (DustUtilityPlugin.IsOffline && DustUtilityPlugin.GetAccounts().Length > 1)
                {
                    SwitchAccountButtonVisibility = Visibility.Visible;
                }
                else { }
            }
            else { }

            Log.WriteLine($"Account={account.AccountString}", LogType.Debug);
            Log.WriteLine($"OfflineMode={DustUtilityPlugin.IsOffline}", LogType.Debug);
        }
        #endregion

        #region SwitchAccount
        private void SwitchAccount()
        {
            DustUtilityPlugin.SwitchAccount();
        }
        #endregion

        #region ShowHistory
        private void ShowHistory(Flyout flyout)
        {
            if (!flyout.IsOpen)
            {
                ((FrameworkElement)flyout.Content).GetViewModel<HistoryFlyoutViewModel>().LoadHistory();

                flyout.IsOpen = true;
            }
            else { }
        }
        #endregion

        private void ShowDecks()
        {
        }

        private void ShowCollectionInfo()
        {
        }
    }
}