using HearthDb.Enums;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Spawn.HDT.DustUtility.CardSets.Info;

namespace Spawn.HDT.DustUtility.UI.Windows
{
    public partial class CollectionInfoWindow
    {
        #region Member Variables
        private List<HearthMirror.Objects.Card> m_lstCollection;
        #endregion

        #region Ctor
        public CollectionInfoWindow()
        {
            InitializeComponent();
        }

        public CollectionInfoWindow(Account account)
            : this()
        {
            m_lstCollection = account.LoadCollection();
        }
        #endregion

        #region Events
        #region OnLoaded
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            List<CardSet> lstCardSets = new List<CardSet>(CardSets.All.Keys);

            for (int i = 0; i < lstCardSets.Count; i++)
            {
                CardSet cardSet = lstCardSets[i];

                if (HasCardSet(cardSet))
                {
                    AddCardSet(cardSet);
                }
                else { }
            }
        }
        #endregion
        #endregion

        #region AddCardSet
        private void AddCardSet(CardSet cardSet)
        {
            ListViewCardSetItem cardSetItem = new ListViewCardSetItem();

            InfoItem infoItem = Dictionary[cardSet];
        }
        #endregion

        #region HasCardSet
        private bool HasCardSet(CardSet cardSet)
        {
            bool blnRet = false;

            if (m_lstCollection.Count > 0)
            {
                blnRet = m_lstCollection.Find(c =>
                {
                    return HearthDb.Cards.All[c.Id].Set == cardSet;
                }) != null;
            }
            else { }

            return blnRet;
        }
        #endregion

        #region GetBanner
        public ImageSource GetBanner(CardSet cardSet)
        {
            string strPath = string.Empty;

            switch (cardSet)
            {
                case CardSet.EXPERT1:
                    strPath = "";
                    break;

                case CardSet.GVG:
                    break;

                case CardSet.TGT:
                    break;

                case CardSet.OG:
                    break;

                case CardSet.GANGS:
                    break;

                case CardSet.UNGORO:
                    break;

                case CardSet.ICECROWN:
                    break;

                case CardSet.LOOTAPALOOZA:
                    break;

                case CardSet.NAXX:
                    strPath = "/Spawn.HDT.DustUtility;component/Resources/naxx_logo.png";
                    break;

                case CardSet.BRM:
                    break;

                case CardSet.LOE:
                    break;

                case CardSet.KARA:
                    break;

                case CardSet.HOF:
                    break;
            }

            return new BitmapImage(new System.Uri(strPath));
        }
        #endregion
    }
}