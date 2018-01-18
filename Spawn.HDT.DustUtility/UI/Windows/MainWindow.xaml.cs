using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls.Dialogs;
using Spawn.HDT.DustUtility.Net;
using Spawn.HDT.DustUtility.Offline;
using Spawn.HDT.DustUtility.Search;
using Spawn.HDT.DustUtility.UI.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Spawn.HDT.DustUtility.UI.Windows
{
    public partial class MainWindow
    {
        #region Static Stuff
        private static string s_strSearchHelpText;

        static MainWindow()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Search terms:").Append(Environment.NewLine)
                .Append("- Dust amount (e.g. 500)").Append(Environment.NewLine)
                .Append("- Card name (e.g. Aya Blackpaw, Aya, Black)").Append(Environment.NewLine)
                .Append("- Card tribe (e.g. Dragon, Elemental, etc.)").Append(Environment.NewLine)
                .Append("- Card mechanics (e.g. Battlecry, Taunt, etc.)").Append(Environment.NewLine)
                .Append("- Card set (e.g. Un'goro, Gadgetzan, Goblins, etc.)").Append(Environment.NewLine)
                .Append("- Card type (e.g. Minion, Weapon, etc.)").Append(Environment.NewLine);

            s_strSearchHelpText = sb.ToString();
        }
        #endregion

        #region Member Variables
        private Account m_account;

        private CardSelectionWindow m_selectionWindow;
        private DecksInfoWindow m_decksWindow;
        private CollectionInfoWindow m_collectionWindow;

        private CardManager m_cardManager;
        private Parameters m_parameters;
        #endregion

        #region Ctor
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Account account, bool multipleAccountsAvailable)
            : this()
        {
            m_account = account;

            m_cardManager = new CardManager(this, m_account);

            if (Settings.SearchParameters == null)
            {
                m_parameters = new Parameters(true);
            }
            else
            {
                m_parameters = Settings.SearchParameters.DeepClone();
            }

            if (!m_account.IsEmpty)
            {
                Title = $"{Title} [{m_account.BattleTag.Name} ({m_account.Region})]";
            }
            else { }

            if (DustUtilityPlugin.IsOffline)
            {
                Title = $"{Title} [OFFLINE]";
            }
            else { }

            if (Settings.OfflineMode)
            {
                historyButton.Visibility = System.Windows.Visibility.Visible;

                if (DustUtilityPlugin.IsOffline && multipleAccountsAvailable)
                {
                    switchAccountButton.Visibility = System.Windows.Visibility.Visible;
                }
                else { }
            }
            else { }

            Log.WriteLine($"Account={m_account.AccountString}", LogType.Debug);
            Log.WriteLine($"OfflineMode={DustUtilityPlugin.IsOffline}", LogType.Debug);
        }
        #endregion

        #region Events
        #region OnWindowLoaded
        private async void OnWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //Create backup
            BackupManager.Create(m_account);

            //Delete old backups
            BackupManager.DeleteOldBackups(m_account);

            //Perform update check
            if (Settings.CheckForUpdate && await GitHubUpdateManager.CheckForUpdateAsync())
            {
                StringBuilder sb = new StringBuilder();

                sb.Append($"Update {GitHubUpdateManager.NewVersion.ToString(3)} has been released.").Append(Environment.NewLine + Environment.NewLine)
                    .Append("Release Notes:").Append(Environment.NewLine)
                    .Append(GitHubUpdateManager.ReleaseNotes).Append(Environment.NewLine + Environment.NewLine)
                    .Append("Would you like to download it?");

                MessageDialogResult result = await this.ShowMessageAsync("New update available", sb.ToString(), MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    //System.Diagnostics.Process.Start(GitHubUpdateManager.LatestReleaseUrl);

                    //Log.WriteLine("Opening github release page...", LogType.Debug);

                    DownloadProgressDialog dialog = new DownloadProgressDialog();

                    dialog.ShowDialog();
                }
                else { }
            }
            else { }
        }
        #endregion

        #region OnSearchClick
        private async void OnSearchClick(object sender, System.Windows.RoutedEventArgs e)
        {
            await SearchAsync();
        }
        #endregion

        #region OnFiltersClick
        private void OnFiltersClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (m_cardManager != null && m_parameters != null)
            {
                ParametersDialog dialog = new ParametersDialog(m_parameters.DeepClone())
                {
                    Owner = this
                };

                if (dialog.ShowDialog().Value)
                {
                    m_parameters = dialog.Parameters.DeepClone();

                    Settings.SearchParameters = dialog.Parameters.DeepClone();
                }
                else { }
            }
            else { }
        }
        #endregion

        #region OnCollectionInfoClick
        private void OnCollectionInfoClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //await this.ShowMessageAsync("Collection Value", $"Your collection is worth: {m_cardCollector.GetTotalDustValueForAllCards()} Dust");

            if (m_collectionWindow == null)
            {
                m_collectionWindow = new CollectionInfoWindow(m_account)
                {
                    Owner = this
                };

                m_collectionWindow.Closed += (s, args) => m_collectionWindow = null;

                m_collectionWindow.Show();
            }
            else
            {
                DustUtilityPlugin.BringWindowToFront(m_collectionWindow);
            }
        }
        #endregion

        #region OnSearchHelpClick
        private async void OnSearchHelpClick(object sender, System.Windows.RoutedEventArgs e)
        {
            await this.ShowMessageAsync("Help", s_strSearchHelpText);
        }
        #endregion

        #region OnSortOrderClick
        private void OnSortOrderClick(object sender, System.Windows.RoutedEventArgs e)
        {
            SortOrderDialog dialog = new SortOrderDialog()
            {
                Owner = this
            };

            if (dialog.ShowDialog().Value)
            {
                //dataGrid.ItemsSource = OrderItems(dataGrid.ItemsSource as IEnumerable<GridItem>);

                SearchResultContainer container = GetSearchResultContainerComponent();

                container.SetGridItems(OrderItems(container.GridItems));
            }
            else { }
        }
        #endregion

        #region OnClearGridClick
        private void OnClearGridClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ClearGrid();
        }
        #endregion

        #region OnOpenSelectionClick
        private void OnOpenSelectionClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (m_selectionWindow == null)
            {
                List<DataGridCardItem> lstSelection = m_account.AccountPreferences.CardSelection.ConvertAll(c =>
                {
                    CardWrapper wrapper = new CardWrapper(new HearthMirror.Objects.Card(c.Id, c.Count, c.IsGolden));

                    return DataGridCardItem.FromCardWrapper(wrapper);
                });

                m_selectionWindow = new CardSelectionWindow(lstSelection)
                {
                    Owner = this
                };

                m_selectionWindow.Closed += (s, args) =>
                {
                    m_account.AccountPreferences.CardSelection.Clear();

                    if (m_selectionWindow.SaveSelection)
                    {
                        m_account.AccountPreferences.CardSelection = m_selectionWindow.CurrentItems.ConvertAll(i =>
                        {
                            return new CachedCard
                            {
                                Id = i.Tag.Card.Id,
                                Count = i.Tag.Card.Count,
                                IsGolden = i.Tag.Card.Premium
                            };
                        });

                        m_account.SavePreferenes();
                    }
                    else { }

                    openSelectionButton.IsEnabled = true;

                    m_selectionWindow = null;
                };

                m_selectionWindow.Show();

                openSelectionButton.IsEnabled = false;
            }
            else
            {
                DustUtilityPlugin.BringWindowToFront(m_selectionWindow);
            }
        }
        #endregion

        #region OnInputBoxPreviewKeyDown
        private async void OnInputBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && searchButton.IsEnabled)
            {
                await SearchAsync();
            }
            else { }
        }
        #endregion

        #region OnInputBoxTextChanged
        private void OnInputBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            searchButton.IsEnabled = inputBox.Text.Length > 0;
        }
        #endregion

        #region OnInputBoxGotFocus
        private void OnInputBoxGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
        #endregion

        #region OnSwitchAccountClick
        private void OnSwitchAccountClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DustUtilityPlugin.SwitchAccounts();
        }
        #endregion

        #region OnHistoryClick
        private void OnHistoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Log.WriteLine("Opening history dialog...", LogType.Debug);

            HistoryDialog dialog = new HistoryDialog(m_account)
            {
                Owner = this
            };

            dialog.ShowDialog();
        }
        #endregion

        #region OnDecksClick
        private void OnDecksClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (m_decksWindow == null)
            {
                m_decksWindow = new DecksInfoWindow(m_account)
                {
                    Owner = this
                };

                m_decksWindow.Closed += (s, args) =>
                {
                    m_decksWindow = null;

                    m_account.SavePreferenes();
                };

                m_decksWindow.Show();
            }
            else
            {
                DustUtilityPlugin.BringWindowToFront(m_decksWindow);
            }
        }
        #endregion
        #endregion

        #region UpdateUIState
        private void UpdateUIState(bool blnIsEnabled)
        {
            Log.WriteLine($"Updating UI state: Enabled={blnIsEnabled}", LogType.Debug);

            if (blnIsEnabled)
            {
                searchButton.Content = "GO!";
            }
            else
            {
                searchButton.Content = "...";
            }

            searchButton.IsEnabled = blnIsEnabled;
            inputBox.IsEnabled = blnIsEnabled;
            filterButton.IsEnabled = blnIsEnabled;
            sortOrderButton.IsEnabled = blnIsEnabled;
        }
        #endregion
    }
}