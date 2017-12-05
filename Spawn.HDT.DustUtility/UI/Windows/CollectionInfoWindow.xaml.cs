using HearthDb.Enums;
using System;
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

        public CollectionInfoWindow(Account account, int collectionValue)
            : this()
        {
            m_lstCollection = account.LoadCollection();

            Title = $"{Title} ({collectionValue} Dust)";
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
            InfoItem infoItem = Dictionary[cardSet];

            ListViewCardSetItem cardSetItem = new ListViewCardSetItem
            {
                Logo = GetLogo(cardSet),
                Banner = GetBanner(cardSet),
                Name = CardSets.AllFullName[cardSet],
                CommonsCount = GetCountForRarity(cardSet, Rarity.COMMON),
                RaresCount = GetCountForRarity(cardSet, Rarity.RARE),
                EpicsCount = GetCountForRarity(cardSet, Rarity.EPIC),
                LegendariesCount = GetCountForRarity(cardSet, Rarity.LEGENDARY),
                DustValue = GetDustValue(cardSet)
            };

            cardSetItem.TotalCount = cardSetItem.CommonsCount + cardSetItem.RaresCount + cardSetItem.EpicsCount + cardSetItem.LegendariesCount;

            listView.Items.Add(cardSetItem);
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

        #region GetLogo
        public ImageSource GetLogo(CardSet cardSet)
        {
            string strSource = string.Empty;

            switch (cardSet)
            {
                case CardSet.EXPERT1:
                case CardSet.HOF://TODO add own logo or readd card set names
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/hearthstone_logo.png";
                    break;

                case CardSet.GVG:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/goblins_logo.png";
                    break;

                case CardSet.TGT:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/tournament_logo.png";
                    break;

                case CardSet.OG:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/oldgods_logo.png";
                    break;

                case CardSet.GANGS:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/gadgetzan_logo.png";
                    break;

                case CardSet.UNGORO:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/ungoro_logo.png";
                    break;

                case CardSet.ICECROWN:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/frozenthrone_logo.png";
                    break;

                case CardSet.LOOTAPALOOZA:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/kobolds_logo.png";
                    break;

                case CardSet.NAXX:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/naxx_logo.png";
                    break;

                case CardSet.BRM:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/mountain_logo.png";
                    break;

                case CardSet.LOE:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/league_logo.png";
                    break;

                case CardSet.KARA:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/karazhan_logo.png";
                    break;
            }

            return new BitmapImage(new Uri(strSource, UriKind.Relative));
        }
        #endregion

        #region GetBanner
        public ImageSource GetBanner(CardSet cardSet)
        {
            string strSource = string.Empty;

            switch (cardSet)
            {
                case CardSet.EXPERT1:
                case CardSet.HOF://TODO add own banner or readd card set names
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/hearthstone_banner.jpg";
                    break;

                case CardSet.GVG:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/goblins_banner.jpg";
                    break;

                case CardSet.TGT:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/tournament_banner.jpg";
                    break;

                case CardSet.OG:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/oldgods_banner.jpg";
                    break;

                case CardSet.GANGS:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/gadgetzan_banner.jpg";
                    break;

                case CardSet.UNGORO:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/ungoro_banner.jpg";
                    break;

                case CardSet.ICECROWN:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/frozenthrone_banner.jpg";
                    break;

                case CardSet.LOOTAPALOOZA:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/kobolds_banner.jpg";
                    break;

                case CardSet.NAXX:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/naxx_banner.jpg";
                    break;

                case CardSet.BRM:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/mountain_banner.jpg";
                    break;

                case CardSet.LOE:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/league_banner.jpg";
                    break;

                case CardSet.KARA:
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/karazhan_banner.jpg";
                    break;
            }

            return new BitmapImage(new Uri(strSource, UriKind.Relative));
        }
        #endregion

        #region GetCountForRarity
        private int GetCountForRarity(CardSet cardSet, Rarity rarity)
        {
            int nRet = 0;

            List<HearthMirror.Objects.Card> lstChunk = m_lstCollection.FindAll(c =>
            {
                HearthDb.Card card = HearthDb.Cards.Collectible[c.Id];

                return card.Set == cardSet && card.Rarity == rarity;
            });

            for (int i = 0; i < lstChunk.Count; i++)
            {
                nRet += lstChunk[i].Count;
            }

            return nRet;
        }
        #endregion

        #region GetDustValue
        private int GetDustValue(CardSet cardSet)
        {
            int nRet = 0;

            List<HearthMirror.Objects.Card> lstChunk = m_lstCollection.FindAll(c => HearthDb.Cards.Collectible[c.Id].Set == cardSet);

            for (int i = 0; i < lstChunk.Count; i++)
            {
                nRet += lstChunk[i].GetDustValue();
            }

            return nRet;
        }
        #endregion
    }
}