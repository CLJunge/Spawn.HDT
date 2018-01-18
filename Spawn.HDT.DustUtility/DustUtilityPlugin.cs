using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.Offline;
using Spawn.HDT.DustUtility.UI.Dialogs;
using Spawn.HDT.DustUtility.UI.Windows;
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
        #region Static Properties
        public static string DataDirectory => GetDataDirectory();
        public static bool IsOffline { get; private set; }
        #endregion

        #region Static Variables
        private static Window s_window;

        private static Account s_account;
        #endregion

        #region Member Variables
        private MenuItem m_menuItem;
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
            UpdatePlugin();

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
            IsOffline = !Core.Game.IsRunning && Settings.OfflineMode;

            if (Settings.OfflineMode && (Core.Game.IsRunning && !Cache.TimerEnabled))
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
                if (s_account == null || (s_account != null && s_account.IsEmpty))
                {
                    SelectAccount(false);
                }
                else { }

                if (s_account != null)
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

        #region Update Stuff
        #region UpdatePlugin
        public void UpdatePlugin()
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

            Log.WriteLine($"Converted sort order string successfuly", LogType.Debug);

            Properties.Settings.Default.SortOrder = strSortOrder;
            Properties.Settings.Default.Version = 2;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region UpdateHistoryFiles
        private void UpdateHistoryFiles()
        {
            //Create backup for each account
            Account[] vAccounts = GetAccounts();

            for (int i = 0; i < vAccounts.Length; i++)
            {
                BackupManager.Create(vAccounts[i]);
            }

            //Modify files
            string[] vFiles = Directory.GetFiles(DataDirectory, $"*_{HistoryManager.HistoryString}.xml");

            for (int i = 0; i < vFiles.Length; i++)
            {
                bool blnHasNewFormat = false;

                try
                {
                    FileManager.Load<List<CachedCardEx>>(vFiles[i]);

                    blnHasNewFormat = true;
                }
                catch
                {
                    Log.WriteLine($"New format not available, converting history file \"{vFiles[i]}\"", LogType.Debug);
                }

                if (!blnHasNewFormat)
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

                    Log.WriteLine($"Converted history file \"{vFiles[i]}\" successfuly", LogType.Debug);
                }
                else { }
            }

            Properties.Settings.Default.Version = 3;
            Properties.Settings.Default.Save();
        }
        #endregion
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

        #region Static Methods
        #region OpenMainWindow
        private static void OpenMainWindow()
        {
            if (s_window == null)
            {
                Log.WriteLine($"Opening main window for {s_account.AccountString}", LogType.Info);

                s_window = new MainWindow(s_account, GetAccounts().Length > 1);

                s_window.Closed += (s, e) =>
                {
                    s_account.SavePreferenes();

                    s_window = null;
                };

                s_window.Show();
            }
            else
            {
                BringWindowToFront(s_window);
            }
        }
        #endregion

        #region SelectAccount
        private static void SelectAccount(bool blnIsSwitching)
        {
            if (Core.Game.IsRunning && !blnIsSwitching)
            {
                s_account = Account.Current;
            }
            else
            {
                Account[] vAccounts = GetAccounts();

                if (vAccounts.Length == 1)
                {
                    s_account = vAccounts[0];
                }
                else if (vAccounts.Length > 1)
                {
                    AccountSelectorDialog accSelectorDialog = new AccountSelectorDialog(vAccounts);

                    if (accSelectorDialog.ShowDialog().Value)
                    {
                        s_account = Account.Parse(accSelectorDialog.SelectedAccount);

                        Settings.LastSelectedAccount = s_account.AccountString;
                    }
                    else { }
                }
                else
                {
                    s_account = Account.Empty;
                }
            }

            Log.WriteLine($"Account: {s_account?.AccountString}", LogType.Debug);
        }
        #endregion

        #region GetAccounts
        private static Account[] GetAccounts()
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

            return lstRet.ToArray();
        }
        #endregion

        #region SwitchAccounts
        public static void SwitchAccounts()
        {
            if (s_window != null)
            {
                Log.WriteLine("Switching accounts...", LogType.Debug);

                Account oldAcc = s_account;

                s_account = null;

                SelectAccount(true);

                if (s_account == null)
                {
                    s_account = oldAcc;
                }
                else { }

                if (!s_account.Equals(oldAcc))
                {
                    s_window.Close();
                }
                else { }

                OpenMainWindow();

                Log.WriteLine($"Switched accounts: Old={oldAcc.AccountString} New={s_account.AccountString}", LogType.Debug);
            }
            else { }
        }
        #endregion

        #region GetDataDirectory
        private static string GetDataDirectory()
        {
            string strRet = Path.Combine(Hearthstone_Deck_Tracker.Config.Instance.DataDir, "DustUtility");

            if (!Directory.Exists(strRet))
            {
                Directory.CreateDirectory(strRet);
            }
            else { }

            return strRet;
        }
        #endregion

        #region GetFullFileName
        public static string GetFullFileName(Account account, string strType)
        {
            string strRet = string.Empty;

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

        #region BringWindowToFront
        public static void BringWindowToFront(Window window)
        {
            window.Activate();
            window.Topmost = true;
            window.Topmost = false;
            window.Focus();
        }
        #endregion
        #endregion
    }
}