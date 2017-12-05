using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI
{
    public class ListViewCardSetItem
    {
        #region Properties
        #region Banner
        public ImageSource Banner { get; set; }
        #endregion

        #region Logo
        public ImageSource Logo { get; set; }
        #endregion

        #region Name
        public string Name { get; set; }
        #endregion

        #region TotalCount
        public int TotalCount { get; set; }
        #endregion

        #region CommonsCount
        public int CommonsCount { get; set; }
        #endregion

        #region RaresCount
        public int RaresCount { get; set; }
        #endregion

        #region EpicsCount
        public int EpicsCount { get; set; }
        #endregion

        #region LegendariesCount
        public int LegendariesCount { get; set; }
        #endregion

        #region DustValue
        public int DustValue { get; set; }
        #endregion
        #endregion
    }
}