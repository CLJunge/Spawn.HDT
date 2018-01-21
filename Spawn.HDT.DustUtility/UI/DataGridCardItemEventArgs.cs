using System;

namespace Spawn.HDT.DustUtility.UI
{
    public class DataGridCardItemEventArgs : EventArgs
    {
        #region Properties
        #region Item
        public DataGridCardItem Item { get; }
        #endregion

        #region RowIndex
        public int RowIndex { get; }
        #endregion
        #endregion

        #region Ctor
        public DataGridCardItemEventArgs(DataGridCardItem item, int rowIndex)
        {
            Item = item;

            RowIndex = rowIndex;
        }
        #endregion
    }
}