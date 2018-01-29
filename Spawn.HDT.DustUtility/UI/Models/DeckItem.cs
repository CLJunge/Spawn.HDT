using GalaSoft.MvvmLight;
using System.Windows.Media.Imaging;

namespace Spawn.HDT.DustUtility.UI.Models
{
    public class DeckItem : ObservableObject
    {
        #region Member Variables
        private long m_nDeckId;
        private string m_strName;
        private int m_nCardCount;
        private int m_nCraftingCost;
        #endregion

        #region Properties
        #region DeckId
        public long DeckId
        {
            get => m_nDeckId;
            set => Set(ref m_nDeckId, value);
        }
        #endregion

        #region Name
        public string Name
        {
            get => m_strName;
            set => Set(ref m_strName, value);
        }
        #endregion

        #region CardCount
        public int CardCount
        {
            get => m_nCardCount;
            set => Set(ref m_nCardCount, value);
        }
        #endregion

        #region CraftingCost
        public int CraftingCost
        {
            get => m_nCraftingCost;
            set => Set(ref m_nCraftingCost, value);
        }
        #endregion

        #region HeroImage
        public BitmapImage HeroImage { get; set; }
        #endregion
        #endregion
    }
}