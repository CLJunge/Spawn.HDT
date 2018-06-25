#region Using
using System;
#endregion

namespace Spawn.HDT.DustUtility.Logging
{
    public class LogEventArgs : EventArgs
    {
        #region Properties
        #region Log
        public LogEntry Log { get; private set; }
        #endregion
        #endregion

        #region Ctor
        public LogEventArgs(LogEntry entry)
        {
            Log = entry;
        }
        #endregion
    }
}