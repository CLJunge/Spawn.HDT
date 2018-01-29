using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace Spawn.HDT.DustUtility.CardManagement.Offline
{
    public static class BackupManager
    {
        #region Create
        public static bool Create(Account account)
        {
            bool blnRet = false;

            if (DustUtilityPlugin.Config.OfflineMode && (!account.IsEmpty && account.IsValid && account.HasFiles))
            {
                DateTime date = DateTime.Now;

                if (!BackupExists(account, date))
                {
                    string strFileName = GetBackupFileName(account, date);

                    try
                    {
                        string[] vFiles = Directory.GetFiles(DustUtilityPlugin.AccountsDirectory, $"{account.AccountString}*");

                        if (vFiles.Length > 0)
                        {
                            string strDir = Path.Combine(DustUtilityPlugin.DataDirectory, "temp");

                            Directory.CreateDirectory(strDir);

                            for (int i = 0; i < vFiles.Length; i++)
                            {
                                string strTemp = Path.Combine(strDir, Path.GetFileName(vFiles[i]));

                                File.Copy(vFiles[i], strTemp);
                            }

                            ZipFile.CreateFromDirectory(strDir, strFileName);

                            Directory.Delete(strDir, true);

                            Log.WriteLine($"Created backup for {account.AccountString}", LogType.Debug);

                            blnRet = File.Exists(strFileName);
                        }
                        else { }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLine($"Exception occured while creating backup \"{strFileName}\": {ex}", LogType.Error);
                    }
                }
                else { }
            }
            else { }

            return blnRet;
        }
        #endregion

        #region Restore
        public static bool Restore(Account account, DateTime date)
        {
            bool blnRet = false;

            if ((!account.IsEmpty && account.IsValid) && BackupExists(account, date))
            {
                string strFileName = GetBackupFileName(account, date);

                try
                {
                    string strDir = Path.Combine(DustUtilityPlugin.DataDirectory, "extracted");

                    ZipFile.ExtractToDirectory(strFileName, strDir);

                    string[] vFiles = Directory.GetFiles(strDir);

                    for (int i = 0; i < vFiles.Length; i++)
                    {
                        string strTemp = Path.Combine(DustUtilityPlugin.AccountsDirectory, Path.GetFileName(vFiles[i]));

                        if (File.Exists(strTemp))
                        {
                            File.Delete(strTemp);
                        }
                        else { }

                        File.Move(vFiles[i], strTemp);
                    }

                    Directory.Delete(strDir);

                    Log.WriteLine($"Restored backup for {account.AccountString}", LogType.Debug);

                    blnRet = true;
                }
                catch (Exception ex)
                {
                    Log.WriteLine($"Exception occured while restoring backup \"{strFileName}\": {ex}", LogType.Error);
                }
            }
            else { }

            return blnRet;
        }
        #endregion

        #region DeleteOldBackups
        public static void DeleteOldBackups(Account account)
        {
            string strDirectory = GetAccountBackupDirectory(account);

            string[] vFiles = Directory.GetFiles(strDirectory);

            for (int i = 0; i < vFiles.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(vFiles[i]);

                if (fileInfo.CreationTime.Date < DateTime.Now.Date.AddMonths(-1))
                {
                    fileInfo.Delete();
                }
                else { }
            }
        }
        #endregion

        #region BackupExists
        public static bool BackupExists(Account account, DateTime date)
        {
            string strFileName = GetBackupFileName(account, date);

            return File.Exists(strFileName);
        }
        #endregion

        #region GetAccountBackupDirectory
        private static string GetAccountBackupDirectory(Account account)
        {
            string strRet = Path.Combine(DustUtilityPlugin.BackupsDirectory, account.AccountString);

            if (!Directory.Exists(strRet))
            {
                Directory.CreateDirectory(strRet);
            }
            else { }

            return strRet;
        }
        #endregion

        #region GetBackupFileName
        private static string GetBackupFileName(Account account, DateTime date)
        {
            string strFileName = $"backup_{date.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}.zip";

            return Path.Combine(GetAccountBackupDirectory(account), strFileName);
        }
        #endregion
    }
}