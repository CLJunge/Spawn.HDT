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
using Spawn.HDT.DustUtility.UI.Windows;
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
        private static bool s_blnCheckedForUpdates;
        #endregion

        #region Member Variables
        private string m_strWindowTitle;
        private Visibility m_historyButtonVisibility;
        private Visibility m_switchAccountButtonVisibility;
        private string m_strSearchQuery;

        private CardSelectionWindow m_selectionWindow;
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

        #region OpenCardSelectionWindowCommand
        public ICommand OpenCardSelectionWindowCommand => new RelayCommand(OpenCardSelectionWindow);
        #endregion

        #region SelectionWindow
        public CardSelectionWindow SelectionWindow => m_selectionWindow;
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
                    s_blnCheckedForUpdates = !DustUtilityPlugin.Config.CheckForUpdates;
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

            HistoryButtonVisibility = Visibility.Visible;
            SwitchAccountButtonVisibility = Visibility.Visible;
#else
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
#endif

            BackupManager.Create(account);

            Log.WriteLine($"Account={account.AccountString}", LogType.Debug);
            Log.WriteLine($"OfflineMode={DustUtilityPlugin.IsOffline}", LogType.Debug);

            SearchQuery = account.Preferences.SearchParameters.QueryString;

            ClearControls();

            ReloadFlyouts();

            Task.Run(async () =>
            {
                BackupManager.DeleteOldBackups(account);

                if (!s_blnCheckedForUpdates)
                {
                    if ((DustUtilityPlugin.Config.CheckForUpdates && await UpdateManager.PerformUpdateCheckAsync()))
                    {
                        DustUtilityPlugin.MainWindow.Dispatcher.Invoke(() =>
                        {
                            OpenFlyout(DustUtilityPlugin.MainWindow.UpdateFlyout);
                        });
                    }
                    else { }

                    s_blnCheckedForUpdates = true;
                }
                else { }
            });
        }
        #endregion

        #region SwitchAccount
        private void SwitchAccount()
        {
            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString = SearchQuery;

            if (DustUtilityPlugin.SwitchAccount())
            {
                m_selectionWindow?.Close();
                m_selectionWindow = null;
            }
            else { }
        }
        #endregion

        #region OpenFlyout
        public void OpenFlyout(Flyout flyout)
        {
            ViewModelBase viewModel = (((FrameworkElement)flyout.Content).DataContext as ViewModelBase);

            if (flyout.Content is Flyouts.CollectionInfoFlyout && viewModel.ReloadRequired)
            {
                int nCollectionValue = CardsManager.GetCollectionValue(DustUtilityPlugin.CurrentAccount);

                flyout.Header = $"Collection Info ({nCollectionValue} Dust)";
            }
            else { }

            viewModel.Initialize();

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
            ClearControls();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString = SearchQuery;

                SearchResult result = await CardsManager.GetCardsAsync(DustUtilityPlugin.CurrentAccount);

                if (result != null)
                {
                    result.CardItems = OrderItems(result.CardItems).ToList();

                    result.CopyToCardsInfoModel(CardsInfo);

                    for (int i = 0; i < result.CardItems.Count; i++)
                    {
                        CardItems.Add(result.CardItems[i]);
                    }
                }
                else { }
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

            ClearControls();
        }
        #endregion

        #region ReloadFlyoutViews
        public void ReloadFlyouts()
        {
            if (DustUtilityPlugin.MainWindow != null)
            {
                DustUtilityPlugin.MainWindow.HistoryFlyout.IsOpen = false;
                DustUtilityPlugin.MainWindow.DecksFlyout.IsOpen = false;
                DustUtilityPlugin.MainWindow.CollectionInfoFlyout.IsOpen = false;
                DustUtilityPlugin.MainWindow.SearchParametersFlyout.IsOpen = false;
                DustUtilityPlugin.MainWindow.DeckListFlyout.IsOpen = false;
                DustUtilityPlugin.MainWindow.SortOrderFlyout.IsOpen = false;
            }
            else { }

            ServiceLocator.Current.GetInstance<HistoryFlyoutViewModel>().ReloadRequired = true;
            ServiceLocator.Current.GetInstance<DecksFlyoutViewModel>().ReloadRequired = true;
            ServiceLocator.Current.GetInstance<CollectionInfoFlyoutViewModel>().ReloadRequired = true;
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

        #region OpenCardSelectionWindow
        public void OpenCardSelectionWindow()
        {
            ServiceLocator.Current.GetInstance<CardSelectionWindowViewModel>().Initialize();

            if (m_selectionWindow == null)
            {
                m_selectionWindow = new CardSelectionWindow()
                {
                    Owner = DustUtilityPlugin.MainWindow
                };

                m_selectionWindow.Closed += (s, e) => m_selectionWindow = null;

                m_selectionWindow.Show();
            }
            else
            {
                DustUtilityPlugin.BringWindowToFront(m_selectionWindow);
            }
        }
        #endregion

        #region OnClosing
        public void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_selectionWindow?.Close();
            m_selectionWindow = null;

            if (DustUtilityPlugin.HideMainWindowOnClose)
            {
                e.Cancel = true;

                (sender as Window).Hide();
            }
            else { }

            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString = SearchQuery;
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

        #region ClearControls
        private void ClearControls()
        {
            CardItems.Clear();
            CardsInfo.Clear();
        }
        #endregion
    }
}