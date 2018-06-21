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
                    string strAccDir = GetAccountBackupDirectory(account);

                    if (!Directory.Exists(strAccDir))
                    {
                        Directory.CreateDirectory(strAccDir);
                    }
                    else { }

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

                            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Created backup for {account.AccountString}");

                            blnRet = File.Exists(strFileName);
                        }
                        else { }
                    }
                    catch (Exception ex)
                    {
                        DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while creating backup \"{strFileName}\": {ex}");
                    }
                }
                else { }
            }
            else { }

            return blnRet;
        }
        #endregion

        #region RestoreFromBackup
        public static bool RestoreFromBackup(IAccount account, DateTime date)
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

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Restored backup for {account.AccountString}");

                    blnRet = true;
                }
                catch (Exception ex)
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while restoring backup \"{strFileName}\": {ex}");
                }
            }
            else { }

            return blnRet;
        }
        #endregion

        #region DeleteOldBackups
        public static void DeleteOldBackups(IAccount account)
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
                    else { }
                }
            }
            else { }
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