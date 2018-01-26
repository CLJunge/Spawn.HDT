using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace Spawn.HDT.DustUtility.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Member Variables
        private ICardsManager m_cardsManager;

        private Visibility m_historyButtonVisibility;
        private Visibility m_switchAccountButtonVisibility;
        #endregion

        #region Properties
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

        #region SwitchAccountsCommand
        public ICommand SwitchAccountsCommand => new RelayCommand(SwitchAccounts);
        #endregion

        #region ShowHistoryCommand
        public ICommand ShowHistoryCommand => new RelayCommand(ShowHistory);
        #endregion

        #region ShowDecksCommand
        public ICommand ShowDecksCommand => new RelayCommand(ShowDecks);
        #endregion

        #region ShowCollectionInfoCommand
        public ICommand ShowCollectionInfoCommand => new RelayCommand(ShowCollectionInfo);
        #endregion
        #endregion

        #region Ctor
        public MainWindowViewModel()
        {
            if (!DustUtilityPlugin.CurrentAccount.IsEmpty)
            {
                WindowTitle = $"Dust Utility [{DustUtilityPlugin.CurrentAccount.BattleTag.Name} ({DustUtilityPlugin.CurrentAccount.Region})]";
            }
            else { }

            if (DustUtilityPlugin.IsOffline)
            {
                WindowTitle = $"{WindowTitle} [OFFLINE]";
            }
            else { }

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

            Log.WriteLine($"Account={DustUtilityPlugin.CurrentAccount.AccountString}", LogType.Debug);
            Log.WriteLine($"OfflineMode={DustUtilityPlugin.IsOffline}", LogType.Debug);

            m_cardsManager = new CardsManager(DustUtilityPlugin.CurrentAccount);
        }
        #endregion

        #region SwitchAccounts
        private void SwitchAccounts()
        {
            DustUtilityPlugin.SwitchAccounts();
        }
        #endregion

        private void ShowHistory()
        {
        }

        private void ShowDecks()
        {
        }

        private void ShowCollectionInfo()
        {
        }
    }
}