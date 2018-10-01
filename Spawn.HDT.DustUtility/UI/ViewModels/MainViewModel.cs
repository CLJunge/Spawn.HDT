#region Using
using CommonServiceLocator;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HearthMirror;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.Messaging;
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
        #region Constants
        private const string SyncingTag = " - Syncing...";
        private const string CollectionInfoFlyoutHeader = "Collection Info";
        private const string DecksFlyoutHeader = "Decks";
        #endregion

        #region Static Fields
        private static readonly string s_strSearchHelpText;
        #endregion

        #region Member Variables
        private string m_strWindowTitle;
        private Visibility m_historyButtonVisibility;
        private Visibility m_switchAccountButtonVisibility;
        private string m_strSearchQuery;
        private bool m_blnIsSyncing;
        private Visibility m_defaultViewVisibility;
        private Visibility m_splitViewVisibility;
        private bool m_blnDecksButtonEnabled;
        #endregion

        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => false;
        #endregion

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

        #region IsSyncing
        public bool IsSyncing
        {
            get => m_blnIsSyncing;
            set => Set(ref m_blnIsSyncing, value);
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
        public CardSelectionWindow SelectionWindow { get; private set; }
        #endregion

        #region CardSelection
        public CardSelectionManager CardSelection => DustUtilityPlugin.CardSelection;
        #endregion

        #region ClearSelectionCommand
        public ICommand ClearSelectionCommand => new RelayCommand(CardSelection.Clear, () => CardSelection.CardItems.Count > 0);
        #endregion

        #region ImportLatestPackCommand
        public ICommand ImportLatestPackCommand => new RelayCommand(CardSelection.ImportLatestPack, () => Reflection.GetPackCards()?.Count > 0);
        #endregion

        #region DisenchantSelectionCommand
        public ICommand DisenchantSelectionCommand => new RelayCommand(CardSelection.DisenchantSelection, () =>
        {
            return DustUtilityPlugin.Config.AutoDisenchanting && CardSelection.CardItems.Count > 0 && !DustUtilityPlugin.IsOffline;
        });
        #endregion

        #region DefaultViewVisibility
        public Visibility DefaultViewVisibility
        {
            get => m_defaultViewVisibility;
            set => Set(ref m_defaultViewVisibility, value);
        }
        #endregion

        #region SplitViewVisibility
        public Visibility SplitViewVisibility
        {
            get => m_splitViewVisibility;
            set => Set(ref m_splitViewVisibility, value);
        }
        #endregion

        #region DecksButtonEnabled
        public bool DecksButtonEnabled
        {
            get => m_blnDecksButtonEnabled;
            set => Set(ref m_blnDecksButtonEnabled, value);
        }
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

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Initialized 'MainViewModel'");
        }
        #endregion

        #region Ctor
        public MainViewModel()
        {
            CardItems = new ObservableCollection<CardItemModel>();

            CardsInfo = new CardsInfoModel();

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(IsSyncing)))
                {
                    if (IsSyncing)
                    {
                        WindowTitle = $"{WindowTitle}{SyncingTag}";
                    }
                    else
                    {
                        WindowTitle = WindowTitle.Replace(SyncingTag, string.Empty);
                    }
                }
            };

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'MainViewModel' instance");
        }
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Account={DustUtilityPlugin.CurrentAccount.DisplayString}");
            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"OfflineMode={DustUtilityPlugin.IsOffline}");

            await UpdateUIAsync();

            await Task.Run(() => BackupManager.CreateBackup(DustUtilityPlugin.CurrentAccount))
                .ContinueWith(t => BackupManager.DeleteOldBackups(DustUtilityPlugin.CurrentAccount));

            await DustUtilityPlugin.PerformUpdateCheckAsync();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Finished initializing");
        }
        #endregion

        #region SwitchAccount
#pragma warning disable S3168 // "async" methods should not return "void"
        private async void SwitchAccount()
#pragma warning restore S3168 // "async" methods should not return "void"
        {
            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString = SearchQuery;

            DustUtilityPlugin.CardSelection.SaveSelection();

            if (await DustUtilityPlugin.SwitchAccount())
            {
                SelectionWindow?.Close();
                SelectionWindow = null;
            }
        }
        #endregion

        #region OpenFlyout
#pragma warning disable S3168 // "async" methods should not return "void"
        public async void OpenFlyout(Flyout flyout)
#pragma warning restore S3168 // "async" methods should not return "void"
        {
            ViewModelBase viewModel = (((FrameworkElement)flyout.Content).DataContext as ViewModelBase);

            if (flyout.Content is Flyouts.CollectionInfoFlyoutView && viewModel.ReloadRequired)
            {
                flyout.Header = CollectionInfoFlyoutHeader;

                int nCollectionValue = CardsManager.GetTotalCollectionValue(DustUtilityPlugin.CurrentAccount);

                if (nCollectionValue > 0)
                {
                    flyout.Header += $" (Total: {nCollectionValue} Dust)";
                }
            }
            else if (flyout.Content is Flyouts.DecksFlyoutView && viewModel.ReloadRequired)
            {
                flyout.Header = $"{DecksFlyoutHeader} (Count: {DustUtilityPlugin.CurrentAccount.GetDecks().Count})";
            }

            if (!flyout.IsOpen)
            {
                flyout.IsOpen = true;
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Opened '{flyout.Name}'");

            await viewModel.InitializeAsync();
        }
        #endregion

        #region Search
#pragma warning disable S3168 // "async" methods should not return "void"
        private async void Search()
#pragma warning restore S3168 // "async" methods should not return "void"
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

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Got search result");
                }
                else
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Warning, "No search result available!");
                }
            }
        }
        #endregion

        #region ShowSearchHelp
#pragma warning disable S3168 // "async" methods should not return "void"
        private async void ShowSearchHelp() => await DustUtilityPlugin.MainWindow.ShowMessageAsync("Help", s_strSearchHelpText);
#pragma warning restore S3168 // "async" methods should not return "void"
        #endregion

        #region Clear
        private void Clear()
        {
            SearchQuery = string.Empty;

            ClearControls();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Cleared query and controls");
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

            ServiceLocator.Current.GetInstance<HistoryFlyoutViewModel>().ReloadRequired = true;
            ServiceLocator.Current.GetInstance<DecksFlyoutViewModel>().ReloadRequired = true;
            ServiceLocator.Current.GetInstance<CollectionInfoFlyoutViewModel>().ReloadRequired = true;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Reload flyouts");
        }
        #endregion

        #region UpdateCardItemsSortOrderAsync
        public async Task UpdateCardItemsSortOrderAsync()
        {
            await Task.Delay(1);

            List<CardItemModel> lstItems = OrderItems(CardItems).ToList();

            CardItems.Clear();

            for (int i = 0; i < lstItems.Count; i++)
            {
                CardItems.Add(lstItems[i]);
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Updated card items");
        }
        #endregion

        #region OpenCardSelectionWindow
#pragma warning disable S3168 // "async" methods should not return "void"
        public async void OpenCardSelectionWindow()
#pragma warning restore S3168 // "async" methods should not return "void"
        {
            if (SelectionWindow == null)
            {
                SelectionWindow = new CardSelectionWindow()
                {
                    Owner = DustUtilityPlugin.MainWindow
                };

                SelectionWindow.Closed += (s, e) => SelectionWindow = null;

                SelectionWindow.Show();
            }
            else
            {
                DustUtilityPlugin.BringWindowToFront(SelectionWindow);
            }

            await ServiceLocator.Current.GetInstance<CardSelectionWindowViewModel>().InitializeAsync();
            await CardSelection.InitializeAsync();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Opened selection window");
        }
        #endregion

        #region OnClosing
        public void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DustUtilityPlugin.Config.RememberQueryString)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString = SearchQuery;
            }
            else
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString = null;
            }

            CloseSelectionWindow();

            if (DustUtilityPlugin.HideMainWindowOnClose)
            {
                e.Cancel = true;

                (sender as Window).Hide();

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Hiding main window");
            }

            Messenger.Default.Send(new PopupMessage(true));
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

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Ordered card items");

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

        #region CloseSelectionWindow
        private void CloseSelectionWindow()
        {
            SelectionWindow?.Close();
            SelectionWindow = null;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Closed selection window");
        }
        #endregion

        #region UpdateUIAsync
        private async Task UpdateUIAsync()
        {
            SetWindowTitle();

            HistoryButtonVisibility = Visibility.Collapsed;
            SwitchAccountButtonVisibility = Visibility.Collapsed;

            if (DustUtilityPlugin.Config.OfflineMode)
            {
                if (DustUtilityPlugin.Config.EnableHistory)
                {
                    HistoryButtonVisibility = Visibility.Visible;

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Showing history button");
                }

                if (DustUtilityPlugin.IsOffline && DustUtilityPlugin.GetAccounts().Length > 1)
                {
                    SwitchAccountButtonVisibility = Visibility.Visible;

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Showing switch accounts button");
                }
            }

            ClearControls();

            ReloadFlyouts();

            TryUpdateDecksButton(DustUtilityPlugin.IsOffline);

            switch (DustUtilityPlugin.Config.ViewMode)
            {
                case ViewMode.Default:
                    DefaultViewVisibility = Visibility.Visible;
                    SplitViewVisibility = Visibility.Hidden;
                    break;

                case ViewMode.Split:
                    DefaultViewVisibility = Visibility.Hidden;
                    SplitViewVisibility = Visibility.Visible;

                    CloseSelectionWindow();

                    await CardSelection.InitializeAsync();
                    break;
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Current view mode: '{DustUtilityPlugin.Config.ViewMode}'");

            if (DustUtilityPlugin.Config.RememberQueryString
                && !string.IsNullOrEmpty(DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString))
            {
                SearchQuery = DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString;

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Restored last search query ('{SearchQuery}')");

                Search();
            }
            else
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.QueryString = SearchQuery = null;
            }
        }
        #endregion

        #region SetWindowTitle
        private void SetWindowTitle()
        {
            if (!DustUtilityPlugin.CurrentAccount.IsEmpty)
            {
                WindowTitle = $"Dust Utility [{DustUtilityPlugin.CurrentAccount.DisplayString})]";

                if (DustUtilityPlugin.Config.HideBattleTagId)
                {
                    WindowTitle = WindowTitle.Replace($"#{DustUtilityPlugin.CurrentAccount.BattleTag.Number}", string.Empty);
                }
            }
            else
            {
                WindowTitle = "Dust Utility";
            }

            if (DustUtilityPlugin.IsOffline)
            {
                WindowTitle = $"{WindowTitle} [OFFLINE]";
            }

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
        }
        #endregion

        #region TryUpdateDecksButton
        public void TryUpdateDecksButton(bool blnEnabled)
        {
            if (blnEnabled && DustUtilityPlugin.CurrentAccount.GetDecks().Count > 0)
            {
                DecksButtonEnabled = true;
                DustUtilityPlugin.MainWindow.DecksButton.ToolTip = "View a list of your current decks in Hearthstone.";
            }
            else
            {
                DecksButtonEnabled = false;

                string strToolTipText = "Open Hearthstone and visit the 'Play' menu in order to load your decks.";

                if (!DustUtilityPlugin.IsOffline)
                {
                    strToolTipText = "Visit the 'Play' menu in order to load your decks.";
                }

                DustUtilityPlugin.MainWindow.DecksButton.ToolTip = strToolTipText;
            }
        }
        #endregion
    }
}