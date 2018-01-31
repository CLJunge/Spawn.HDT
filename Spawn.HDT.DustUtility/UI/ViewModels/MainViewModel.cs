#region Using
using GalaSoft.MvvmLight.CommandWpf;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.ServiceLocation;
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.Net;
using Spawn.HDT.DustUtility.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Static Fields
        private static string s_strSearchHelpText;
        private static bool s_blnHasCheckedForUpdates;
        #endregion

        #region Member Variables
        private string m_strWindowTitle;
        private Visibility m_historyButtonVisibility;
        private Visibility m_switchAccountButtonVisibility;
        private string m_strSearchQuery;
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
        public ObservableCollection<CardItemModel> CardItems { get; set; }
        #endregion

        #region CardsInfo
        public CardsInfoModel CardsInfo { get; set; }
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

        #region SearchQuery
        public string SearchQuery
        {
            get => m_strSearchQuery;
            set => Set(ref m_strSearchQuery, value);
        }
        #endregion

        #region SwitchAccountCommand
        public ICommand SwitchAccountCommand => new RelayCommand(SwitchAccount);
        #endregion

        #region OpenFlyoutCommand
        public ICommand OpenFlyoutCommand => new RelayCommand<Flyout>(OpenFlyout);
        #endregion

        #region SearchCommand
        public ICommand SearchCommand => new RelayCommand(Search, () => !DustUtilityPlugin.MainWindow.SearchParametersFlyout.IsOpen);
        #endregion

        #region ShowSearchHelpCommand
        public ICommand ShowSearchHelpCommand => new RelayCommand(ShowSearchHelp);
        #endregion

        #region ClearCommand
        public ICommand ClearCommand => new RelayCommand(Clear, () => !string.IsNullOrEmpty(SearchQuery) || CardItems.Count > 0);
        #endregion
        #endregion

        #region Static Ctor
        static MainViewModel()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Possible search terms:").Append(Environment.NewLine)
                .Append("- Dust amount (e.g. 500)").Append(Environment.NewLine)
                .Append("- Card name (e.g. Aya Blackpaw, Aya, Black)").Append(Environment.NewLine)
                .Append("- Card tribe (e.g. Dragon, Elemental, etc.)").Append(Environment.NewLine)
                .Append("- Card mechanics (e.g. Battlecry, Taunt, etc.)").Append(Environment.NewLine)
                .Append("- Card set (e.g. Un'goro, Gadgetzan, Goblins, etc.)").Append(Environment.NewLine)
                .Append("- Card type (e.g. Minion, Weapon, etc.)").Append(Environment.NewLine);

            s_strSearchHelpText = sb.ToString();

            DustUtilityPlugin.Config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(DustUtilityPlugin.Config.CheckForUpdates)))
                {
                    s_blnHasCheckedForUpdates = !DustUtilityPlugin.Config.CheckForUpdates;
                }
                else { }
            };
        }
        #endregion

        #region Ctor
        public MainViewModel()
        {
            CardItems = new ObservableCollection<CardItemModel>();

            CardsInfo = new CardsInfoModel();
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            IAccount account = DustUtilityPlugin.CurrentAccount;

            if (!account.IsEmpty)
            {
                WindowTitle = $"Dust Utility [{account.BattleTag.Name} ({account.Region})]";
            }
            else
            {
                WindowTitle = "Dust Utility";
            }

            if (DustUtilityPlugin.IsOffline)
            {
                WindowTitle = $"{WindowTitle} [OFFLINE]";
            }
            else { }

#if DEBUG
            if (IsInDesignMode)
            {
                WindowTitle = $"{WindowTitle} (Design)";
            }
            else
            {
                WindowTitle = $"{WindowTitle} (Debug)";
            }
#endif

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

            BackupManager.Create(account);

            Log.WriteLine($"Account={account.AccountString}", LogType.Debug);
            Log.WriteLine($"OfflineMode={DustUtilityPlugin.IsOffline}", LogType.Debug);

            if (!string.IsNullOrEmpty(account.Preferences.SearchParameters.QueryString))
            {
                SearchQuery = account.Preferences.SearchParameters.QueryString;
            }
            else
            {
                SearchQuery = string.Empty;
            }

            ClearContainer();

            Task.Run(async () =>
            {
                BackupManager.DeleteOldBackups(account);

                if (!s_blnHasCheckedForUpdates)
                {
                    if ((DustUtilityPlugin.Config.CheckForUpdates && await GitHubUpdateManager.PerformUpdateCheckAsync()))
                    {
                        DustUtilityPlugin.MainWindow.Dispatcher.Invoke(() =>
                        {
                            OpenFlyout(DustUtilityPlugin.MainWindow.UpdateFlyout);
                        });
                    }
                    else { }

                    s_blnHasCheckedForUpdates = true;
                }
                else { }
            });
        }
        #endregion

        #region SwitchAccount
        private void SwitchAccount()
        {
            DustUtilityPlugin.SwitchAccount();

            ReloadFlyouts();
        }
        #endregion

        #region OpenFlyout
        public void OpenFlyout(Flyout flyout)
        {
            (((FrameworkElement)flyout.Content).DataContext as ViewModelBase).Initialize();

            if (!flyout.IsOpen)
            {
                flyout.IsOpen = true;
            }
            else { }
        }
        #endregion

        #region Search
        private async void Search()
        {
            ClearContainer();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString = SearchQuery;

                ICardsManager cardsManager = ServiceLocator.Current.GetInstance<ICardsManager>();

                SearchResult result = await cardsManager.GetSearchResultAsync(DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters);

                result.CardItems = OrderItems(result.CardItems).ToList();

                result.CopyToCardsInfoModel(CardsInfo);

                for (int i = 0; i < result.CardItems.Count; i++)
                {
                    CardItems.Add(result.CardItems[i]);
                }
            }
            else { }
        }
        #endregion

        #region ShowSearchHelp
        private async void ShowSearchHelp()
        {
            await DustUtilityPlugin.MainWindow.ShowMessageAsync("Help", s_strSearchHelpText);
        }
        #endregion

        #region Clear
        private void Clear()
        {
            SearchQuery = string.Empty;

            ClearContainer();
        }
        #endregion

        #region ReloadFlyoutViews
        public void ReloadFlyouts()
        {
            ServiceLocator.Current.GetInstance<HistoryFlyoutViewModel>().ReloadRequired = true;

            if (DustUtilityPlugin.MainWindow.HistoryFlyout.IsOpen)
            {
                ServiceLocator.Current.GetInstance<HistoryFlyoutViewModel>().Initialize();
            }
            else { }

            ServiceLocator.Current.GetInstance<DecksFlyoutViewModel>().ReloadRequired = true;

            if (DustUtilityPlugin.MainWindow.DecksFlyout.IsOpen)
            {
                ServiceLocator.Current.GetInstance<DecksFlyoutViewModel>().Initialize();
            }
            else { }

            ServiceLocator.Current.GetInstance<SearchParametersFlyoutViewModel>().ReloadRequired = true;

            if (DustUtilityPlugin.MainWindow.SearchParametersFlyout.IsOpen)
            {
                ServiceLocator.Current.GetInstance<SearchParametersFlyoutViewModel>().Initialize();
            }
            else { }

            DustUtilityPlugin.MainWindow.DeckListFlyout.IsOpen = false;
        }
        #endregion

        #region UpdateCardItemsSortOrderAsync
        public async Task UpdateCardItemsSortOrderAsync()
        {
            await Task.Delay(1); //return to ui thread

            List<CardItemModel> orderedList = OrderItems(CardItems).ToList();

            CardItems.Clear();

            for (int i = 0; i < orderedList.Count; i++)
            {
                CardItems.Add(orderedList[i]);
            }
        }
        #endregion

        #region OrderItems
        private IEnumerable<CardItemModel> OrderItems(IEnumerable<CardItemModel> items)
        {
            IEnumerable<CardItemModel> retVal = null;

            SortOrder sortOrder = SortOrder.Parse(DustUtilityPlugin.Config.SortOrder);

            if (sortOrder != null && sortOrder.Count > 0)
            {
                IQueryable<CardItemModel> query = items.AsQueryable();

                for (int i = 0; i < sortOrder.Count; i++)
                {
                    query = query.OrderBy(sortOrder[i].Value.ToString(), i);
                }

                retVal = query.ToList();
            }
            else
            {
                retVal = items;
            }

            return retVal;
        }
        #endregion

        #region ClearContainer
        private void ClearContainer()
        {
            CardItems.Clear();
            CardsInfo.Clear();
        }
        #endregion
    }
}