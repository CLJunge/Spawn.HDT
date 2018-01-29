#region Using
using Spawn.HDT.DustUtility.UI.Models;
using System;
#endregion

namespace Spawn.HDT.DustUtility.UI
{
    public class CardItemEventArgs : EventArgs
    {
        #region Properties
        #region Item
        public CardItem Item { get; }
        #endregion

        #region RowIndex
        public int RowIndex { get; }
        #endregion
        #endregion

        #region Ctor
        public CardItemEventArgs(CardItem item, int rowIndex)
        {
            Item = item;

            RowIndex = rowIndex;
        }
        #endregion
    }
}