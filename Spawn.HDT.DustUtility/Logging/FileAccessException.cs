#region Using
using System;
#endregion

namespace Spawn.HDT.DustUtility.Logging
{
    [Serializable]
    public class FileAccessException : Exception
    {
        #region Ctor
        public FileAccessException()
        {
        }
        public FileAccessException(string message) : base(message)
        {
        }
        public FileAccessException(string message, Exception inner) : base(message, inner)
        {
        }
        protected FileAccessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        #endregion
    }
}