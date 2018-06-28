#region Using
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
#endregion

namespace Spawn.HDT.DustUtility.AccountManagement
{
    public static class BackupManager
    {
        #region CreateBackup
        public static bool CreateBackup(IAccount account)
        {
            bool blnRet = false;

            if (DustUtilityPlugin.Config.OfflineMode && (!account.IsEmpty && account.IsValid && account.HasFiles))
            {
                DateTime date = DateTime.Now;

                if (!BackupExists(account, date))
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Creating backup for {account.DisplayString}...");

                    string strAccDir = GetAccountBackupDirectory(account);

                    if (!Directory.Exists(strAccDir))
                    {
                        Directory.CreateDirectory(strAccDir);
                    }

                    string strFileName = GetBackupFileName(account, date);

                    try
                    {
                        string[] vFiles = Directory.GetFiles(DustUtilityPlugin.AccountsDirectory, $"{account.DisplayString}*");

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

                            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Created backup for {account.DisplayString}");

                            blnRet = File.Exists(strFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while creating backup '{strFileName}': {ex}");
                    }
                }
                else
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Backup for today already exists ({account.DisplayString})");
                }
            }

            return blnRet;
        }
        #endregion

        #region RestoreFromBackup
        public static bool RestoreFromBackup(IAccount account, DateTime date)
        {
            bool blnRet = false;

            if ((!account.IsEmpty && account.IsValid) && BackupExists(account, date))
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Restoring backup from the {date.ToLongDateString()} for {account.DisplayString}");

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

                        File.Move(vFiles[i], strTemp);
                    }

                    Directory.Delete(strDir);

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Restored backup for {account.DisplayString}");

                    blnRet = true;
                }
                catch (Exception ex)
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while restoring backup '{strFileName}': {ex}");
                }
            }
            else
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Warning, $"No backup from the {date.ToLongDateString()} for {account.DisplayString} exists!");
            }

            return blnRet;
        }
        #endregion

        #region DeleteOldBackups
        public static void DeleteOldBackups(IAccount account)
        {
            if (!account.IsEmpty && account.IsValid)
            {
                string strDir = GetAccountBackupDirectory(account);

                if (Directory.Exists(strDir))
                {
                    string[] vFiles = Directory.GetFiles(strDir);

                    for (int i = 0; i < vFiles.Length; i++)
                    {
                        FileInfo fileInfo = new FileInfo(vFiles[i]);

                        if (fileInfo.CreationTime.Date < DateTime.Now.Date.AddMonths(-1))
                        {
                            fileInfo.Delete();
                        }
                    }

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Deleted backups which are older than one month for {account.DisplayString}");
                }
            }
        }
        #endregion

        #region BackupExists
        public static bool BackupExists(IAccount account, DateTime date)
        {
            string strFileName = GetBackupFileName(account, date);

            return File.Exists(strFileName);
        }
        #endregion

        #region GetAccountBackupDirectory
        private static string GetAccountBackupDirectory(IAccount account) => Path.Combine(DustUtilityPlugin.BackupsDirectory, account.AccountString);
        #endregion

        #region GetBackupFileName
        private static string GetBackupFileName(IAccount account, DateTime date)
        {
            string strFileName = $"backup_{date.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}.zip";

            return Path.Combine(GetAccountBackupDirectory(account), strFileName);
        }
        #endregion
    }
}