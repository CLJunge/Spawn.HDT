#region Using
using System;
#endregion

namespace Spawn.HDT.DustUtility.Logging
{
    public class LogEvent : EventArgs
    {
        #region Properties
        #region Log
        public LogEntry Log { get; private set; }
        #endregion
        #endregion

        #region Ctor
        public LogEvent(LogEntry entry)
        {
            Log = entry;
        }
        #endregion
    }
}