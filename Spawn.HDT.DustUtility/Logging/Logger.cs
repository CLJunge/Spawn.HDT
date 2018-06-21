#region Using
using System;
using System.IO;
#endregion

namespace Spawn.HDT.DustUtility.Logging
{
    public class Logger
    {
        #region Static Fields
        private static int s_nGlobalLoggerCount;
        #endregion

        #region Member Variables
        private DirectoryInfo m_logDirectory = null;
        private string m_strFilePath = string.Empty;
        private FileInfo m_logFile = null;
        private static object s_objLock = null;
        #endregion

        #region Properties
        #region Name
        public string Name { get; } = $"Logger_{s_nGlobalLoggerCount++}";
        #endregion

        #region LogFileDirectory
        public string LogFileDirectory => m_logDirectory.FullName;
        #endregion

        #region LogFileName
        public string LogFileName => m_logFile.FullName;
        #endregion

        #region IsDebugMode
        public bool IsDebugMode { get; set; } = false;
        #endregion

        #region WriteToConsole
        public bool WriteToConsole { get; set; } = true;
        #endregion

        #region [STATIC] Default
        private static Logger s_default = null;

        public static Logger Default => s_default ?? (s_default = new Logger("Default"));
        #endregion

        #region [STATIC] Debug
        private static Logger s_debug = null;

        public static Logger Debug => s_debug ?? (s_debug = new Logger("Debug") { IsDebugMode = true });
        #endregion
        #endregion

        #region Events
        public event EventHandler<LogEvent> Logging;

        private void OnLogging(LogEntry entry) => Logging?.Invoke(this, new LogEvent(entry));
        #endregion

        #region Ctor
        public Logger(string name)
        {
            if (Directory.Exists(Environment.CurrentDirectory))
            {
                s_objLock = new object();

                Name = name;
                m_logDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Logs"));

                if (!m_logDirectory.Exists)
                {
                    m_logDirectory.Create();
                }
                else { }

                SetLogFileName();
            }
            else
            {
                throw new ArgumentException("The directory doesn't exist!", "FilePath");
            }
        }
        #endregion

        #region Log
        public LogEntry Log(LogLevel level, string strMessage, params object[] vArgs)
        {
            return Log(level, string.Empty, strMessage, vArgs);
        }

        public LogEntry Log(LogLevel level, string strChannel, string strMessage, params object[] vArgs)
        {
            LogEntry retEntry = new LogEntry();

            lock (s_objLock)
            {
                SetLogFileName();

                try
                {
                    retEntry = new LogEntry(DateTime.Now, level, strChannel, strMessage, vArgs);

                    if (WriteToConsole)
                    {
                        if (IsDebugMode || level != LogLevel.Debug)
                        {
                            LogToConsole(retEntry);
                        }
                        else { }
                    }
                    else { }

                    if (IsDebugMode)
                    {
                        System.Diagnostics.Debug.WriteLine(retEntry.LogMessage);
                    }
                    else { }

                    using (StreamWriter writer = new StreamWriter(m_strFilePath, File.Exists(m_strFilePath)))
                    {
                        writer.WriteLine(retEntry.LogMessage);
                        writer.Flush();
                    }

                    OnLogging(retEntry);
                }
                catch (IOException ex)
                {
                    throw new FileAccessException("Can't access log file!", ex);
                }
            }

            return retEntry;
        }
        #endregion

        #region LogToConsole
        private void LogToConsole(LogEntry retEntry)
        {
            switch (retEntry.Level)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            Console.WriteLine(retEntry.LogMessage);

            Console.ForegroundColor = ConsoleColor.Gray;
        }
        #endregion

        #region SetLogFileName
        private void SetLogFileName()
        {
            m_strFilePath = Path.Combine(m_logDirectory.FullName, string.Format("{0}_{1}.txt", Name, DateTime.Now.ToShortDateString()));

            m_logFile = new FileInfo(m_strFilePath);

            if (!m_logFile.Exists)
            {
                using (m_logFile.Create()) { }
            }
            else { }
        }
        #endregion
    }
}