using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI
{
    public class ListViewDeckItem
    {
        #region Properties
        #region DeckId
        public long DeckId { get; set; }
        #endregion

        #region HeroImage
        public ImageSource HeroImage { get; set; }
        #endregion

        #region Name
        public string Name { get; set; }
        #endregion

        #region CardCount
        public int CardCount { get; set; }
        #endregion

        #region Cost
        public int Cost { get; set; }
        #endregion

        #region Tag
        public object Tag { get; set; }
        #endregion
        #endregion
    }
}