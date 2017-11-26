using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.Offline;
using Spawn.HDT.DustUtility.UI;
using Spawn.HDT.DustUtility.UI.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Spawn.HDT.DustUtility
{
    public class DustUtilityPlugin : IPlugin
    {
        #region Static Variables
        public static string DataDirectory = Path.Combine(Hearthstone_Deck_Tracker.Config.Instance.DataDir, "DustUtility");
        #endregion

        #region Member Variables
        private MenuItem m_menuItem;
        private Window m_window;

        private Account m_account;
        #endregion

        #region Properties
        #region Name
        public string Name => "Dust Utility";
        #endregion

        #region Description
        public string Description => "Enter the amount of dust you want to get and check which cards are currently not used in any deck in order to see which can be disenchanted.";
        #endregion

        #region ButtonText
        public string ButtonText => "Settings...";
        #endregion

        #region Author
        public string Author => "CLJunge";
        #endregion

        #region Version
        public Version Version => new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
        #endregion

        #region MenuItem
        public MenuItem MenuItem => m_menuItem;
        #endregion

        #region HasMultipleAccounts
        public bool HasMultipleAccounts => GetAccountList().Count > 1;
        #endregion
        #endregion

        #region Ctor
        public DustUtilityPlugin()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }
        #endregion

        #region IPlugin Methods
        #region OnLoad
        public void OnLoad()
        {
            UpdateApp();

            CreateMenuItem();

            if (Settings.OfflineMode && Core.Game.IsRunning)
            {
                Cache.StartTimer();
            }
            else { }
        }
        #endregion

        #region OnButtonPress
        public void OnButtonPress()
        {
            Log.WriteLine("Opening settings dialog", LogType.Debug);

            //SettingsDialog dialog = new SettingsDialog(Directory.Exists(Path.Combine(DataDirectory, HearthstoneCardImageManager.CacheFolderName)));
            SettingsDialog dialog = new SettingsDialog();

            if (dialog.ShowDialog().Value)
            {
                if (Settings.OfflineMode && Core.Game.IsRunning)
                {
                    if (!Cache.TimerEnabled)
                    {
                        Cache.StartTimer();
                    }
                    else
                    {
                        //Reinitialize timer with new interval
                        Cache.StopTimer();

                        Cache.StartTimer();
                    }
                }
                else if (!Settings.OfflineMode && Cache.TimerEnabled)
                {
                    Cache.StopTimer();
                }
                else { }
            }
            else { }
        }
        #endregion

        #region OnUnload
        public void OnUnload()
        {
            if (Cache.TimerEnabled)
            {
                Cache.StopTimer();
            }
            else { }
        }
        #endregion

        #region OnUpdate
        public void OnUpdate()
        {
            if (Core.Game.IsRunning && !Cache.TimerEnabled)
            {
                Cache.StartTimer();
            }
            else if (!Core.Game.IsRunning && Cache.TimerEnabled)
            {
                Cache.StopTimer();
            }
            else { }
        }
        #endregion

        #region OnClick
        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (Core.Game.IsRunning || Settings.OfflineMode)
            {
                if (m_account == null || (m_account != null && m_account.IsEmpty))
                {
                    ObtainAccount(false);
                }
                else { }

                if (m_account != null)
                {
                    OpenMainWindow();
                }
                else { }
            }
            else if (!Settings.OfflineMode)
            {
                MessageBox.Show("Hearthstone isn't running!", Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else { }
        }
        #endregion
        #endregion

        #region OpenMainWindow
        private void OpenMainWindow()
        {
            if (m_window == null)
            {
                Log.WriteLine($"Opening main window for {m_account.AccountString}", LogType.Info);

                m_window = new MainWindow(this, m_account, !Core.Game.IsRunning && Settings.OfflineMode);

                m_window.Closed += new EventHandler((s, e) => m_window = null);

                m_window.Show();
            }
            else { }
        }
        #endregion

        #region ObtainAccount
        private void ObtainAccount(bool blnIsSwitching)
        {
            if (Core.Game.IsRunning && !blnIsSwitching)
            {
                m_account = Account.Current;
            }
            else
            {
                List<Account> lstAccounts = GetAccountList();

                if (lstAccounts.Count == 1)
                {
                    m_account = lstAccounts[0];
                }
                else if (lstAccounts.Count > 1)
                {
                    AccountSelectorDialog accSelectorDialog = new AccountSelectorDialog(lstAccounts);

                    if (accSelectorDialog.ShowDialog().Value)
                    {
                        m_account = Account.Parse(accSelectorDialog.SelectedAccount);

                        Settings.LastSelectedAccount = m_account.AccountString;
                    }
                    else { }
                }
                else
                {
                    m_account = Account.Empty;
                }
            }

            Log.WriteLine($"Account: {m_account?.AccountString}", LogType.Debug);
        }
        #endregion

        #region GetAccountList
        private List<Account> GetAccountList()
        {
            List<Account> lstRet = new List<Account>();

            if (Directory.Exists(DataDirectory))
            {
                string[] vFiles = Directory.GetFiles(DataDirectory, $"*_{Cache.CollectionString}.xml");

                for (int i = 0; i < vFiles.Length; i++)
                {
                    string strCollectionFileName = vFiles[i];

                    string strDecksFileName = strCollectionFileName.Replace($"_{Cache.CollectionString}", $"_{Cache.DecksString}");

                    if (File.Exists(strDecksFileName))
                    {
                        string strAccountString = Path.GetFileNameWithoutExtension(strCollectionFileName).Replace($"_{Cache.CollectionString}", string.Empty);

                        lstRet.Add(Account.Parse(strAccountString));
                    }
                    else { }
                }
            }
            else { }

            return lstRet;
        }
        #endregion

        #region SwitchAccounts
        public void SwitchAccounts()
        {
            if (m_window != null)
            {
                Log.WriteLine("Switching accounts...", LogType.Debug);

                Account oldAcc = m_account;

                m_account = null;

                ObtainAccount(true);

                if (m_account == null)
                {
                    m_account = oldAcc;
                }
                else { }

                if (!m_account.Equals(oldAcc))
                {
                    m_window.Close();
                }
                else { }

                OpenMainWindow();

                Log.WriteLine($"Switched accounts: Old={oldAcc.AccountString} New={m_account.AccountString}", LogType.Debug);
            }
            else { }
        }
        #endregion

        #region CreateMenuItem
        private void CreateMenuItem()
        {
            m_menuItem = new MenuItem()
            {
                Header = Name,
                Icon = new Image()
                {
                    Source = new BitmapImage(new Uri("/Spawn.HDT.DustUtility;component/Resources/icon.png", UriKind.Relative))
                }
            };

            m_menuItem.Click += OnClick;
        }
        #endregion

        #region GetFullFileName
        public static string GetFullFileName(Account account, string strType)
        {
            string strRet = string.Empty;

            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }
            else { }

            if (!account.IsEmpty)
            {
                strRet = Path.Combine(DataDirectory, $"{account.AccountString}_{strType}.xml");
            }
            else
            {
                strRet = Path.Combine(DataDirectory, $"{strType}.xml");
            }

            return strRet;
        }
        #endregion

        #region OnAssemblyResolve
        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly retVal = null;

            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            string strPath = $"{assemblyName.Name}.dll";

            using (Stream stream = executingAssembly.GetManifestResourceStream(strPath))
            {
                if (stream != null)
                {
                    byte[] vRawBytes = new byte[stream.Length];

                    stream.Read(vRawBytes, 0, vRawBytes.Length);

                    retVal = Assembly.Load(vRawBytes);
                }
                else { }
            }

            return retVal;
        }
        #endregion

        #region Update Stuff
        #region UpdateApp
        public void UpdateApp()
        {
            //Renamed sort order items
            if (Properties.Settings.Default.Version <= 1)
            {
                UpdateSortOrderSetting();
            }
            else { }

            //Added timestamp to history
            if (Properties.Settings.Default.Version <= 2)
            {
                UpdateHistoryFiles();
            }
            else { }
        }
        #endregion

        #region UpdateSortOrderSetting
        private void UpdateSortOrderSetting()
        {
            string strSortOrder = Properties.Settings.Default.SortOrder;

            if (!strSortOrder.Contains("ManaCost"))
            {
                strSortOrder = strSortOrder.Replace("Cost", "ManaCost");
            }
            else { }

            if (!strSortOrder.Contains("CardClass"))
            {
                strSortOrder = strSortOrder.Replace("Class", "CardClass");
            }
            else { }

            Properties.Settings.Default.SortOrder = strSortOrder;
            Properties.Settings.Default.Version = 2;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region UpdateHistoryFiles
        private void UpdateHistoryFiles()
        {
            //Create backup for each account
            List<Account> lstAccounts = GetAccountList();

            for (int i = 0; i < lstAccounts.Count; i++)
            {
                BackupManager.Create(lstAccounts[i]);
            }

            //Modify files
            string[] vFiles = Directory.GetFiles(DataDirectory, $"*_{HistoryManager.HistoryString}.xml");

            for (int i = 0; i < vFiles.Length; i++)
            {
                List<CachedCard> lstHistory = FileManager.Load<List<CachedCard>>(vFiles[i]);

                List<CachedCardEx> lstNewHistory = new List<CachedCardEx>(lstHistory.Count);

                for (int j = 0; j < lstHistory.Count; j++)
                {
                    CachedCard card = lstHistory[j];

                    lstNewHistory.Add(new CachedCardEx
                    {
                        Id = card.Id,
                        Count = card.Count,
                        IsGolden = card.IsGolden,
                        Timestamp = DateTime.Now
                    });
                }

                FileManager.Write(vFiles[i], lstNewHistory);
            }

            Properties.Settings.Default.Version = 3;
            Properties.Settings.Default.Save();
        }
        #endregion
        #endregion
    }
}