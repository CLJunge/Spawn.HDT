#region Using
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.UI.Models;
using System;
#endregion

namespace Spawn.HDT.DustUtility.UI
{
    public class CardItemEventArgs : EventArgs
    {
        #region Properties
        #region Item
        public CardItemModel Item { get; }
        #endregion

        #region RowIndex
        public int RowIndex { get; }
        #endregion
        #endregion

        #region Ctor
        public CardItemEventArgs(CardItemModel item, int rowIndex)
        {
            Item = item;

            RowIndex = rowIndex;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'CardItemEventArgs' instance");
        }
        #endregion
    }
}