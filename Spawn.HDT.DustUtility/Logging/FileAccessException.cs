#region Using
using System;
#endregion

namespace Spawn.HDT.DustUtility.Logging
{
    public class FileAccessException : Exception
    {
        #region Ctor
        public FileAccessException(string Message, Exception InnerException) : base(Message, InnerException)
        {
        }
        #endregion
    }
}