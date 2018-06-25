#region Using
using System;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    [Serializable]
    public class AutoDisenchantingException : Exception
    {
        #region Ctor
        public AutoDisenchantingException()
        {
        }

        public AutoDisenchantingException(string message) : base(message)
        {
        }

        public AutoDisenchantingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AutoDisenchantingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        #endregion
    }
}