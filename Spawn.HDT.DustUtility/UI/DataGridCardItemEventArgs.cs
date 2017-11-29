using System;

namespace Spawn.HDT.DustUtility.UI
{
    public class DataGridCardItemEventArgs : EventArgs
    {
        #region Properties
        public DataGridCardItem Item { get; }
        #endregion

        #region Ctor
        public DataGridCardItemEventArgs(DataGridCardItem item)
        {
            Item = item;
        }
        #endregion
    }
}