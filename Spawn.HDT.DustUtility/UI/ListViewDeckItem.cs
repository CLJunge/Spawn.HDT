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
        public string CardCount { get; set; }
        #endregion

        #region Cost
        public string Cost { get; set; }
        #endregion
        #endregion
    }
}