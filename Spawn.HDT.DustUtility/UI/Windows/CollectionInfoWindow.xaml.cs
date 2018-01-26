using HearthDb;
using HearthDb.Enums;
using Spawn.HDT.DustUtility.Search;
using Spawn.HDT.DustUtility.UI.Components;
using Spawn.HDT.DustUtility.UI.Converters;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Spawn.HDT.DustUtility.UI.Windows
{
    public partial class CollectionInfoWindow
    {
        #region Constants
        private const string TotalCountConverterKey = "totalCountConverter";
        private const string CommonsCountConverterKey = "commonsCountConverter";
        private const string RaresCountConverterKey = "raresCountConverter";
        private const string EpicsCountConverterKey = "epicsCountConverter";
        private const string LegendariesCountConverterKey = "legendariesCountConverter";

        private const string ResourceBasePath = "/Spawn.HDT.DustUtility;component/Resources/";
        #endregion

        #region Member Variables
        private List<HearthMirror.Objects.Card> m_lstCollection;
        #endregion

        #region Ctor
        public CollectionInfoWindow()
        {
            InitializeComponent();

            listView.ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;
        }

        public CollectionInfoWindow(Account account)
            : this()
        {
            m_lstCollection = account.GetCollection();

            Title = $"{Title} ({CardManager.GetCollectionValue(account)} Dust)";
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

        #region OnItemContainerGeneratorStatusChanged
        private void OnItemContainerGeneratorStatusChanged(object sender, EventArgs e)
        {
            if (listView.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                List<CardSetInfoContainer> lstContainers = new List<CardSetInfoContainer>(FindVisualChildren<CardSetInfoContainer>(listView));

                for (int i = 0; i < lstContainers.Count; i++)
                {
                    UpdateContainer(lstContainers[i], listView.Items[i] as ListViewCardSetItem);
                }
            }
            else { }
        }
        #endregion
        #endregion

        #region AddCardSet
        private void AddCardSet(CardSet cardSet)
        {
            ListViewCardSetItem cardSetItem = new ListViewCardSetItem
            {
                Logo = GetLogo(cardSet),
                Banner = GetBanner(cardSet),
                Name = CardSets.AllFullName[cardSet],
                CommonsCount = GetCountForRarity(cardSet, Rarity.COMMON),
                RaresCount = GetCountForRarity(cardSet, Rarity.RARE),
                EpicsCount = GetCountForRarity(cardSet, Rarity.EPIC),
                LegendariesCount = GetCountForRarity(cardSet, Rarity.LEGENDARY),
                DustValue = GetDustValue(m_lstCollection, cardSet),
                Tag = cardSet
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
                    return Cards.All[c.Id].Set == cardSet;
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
                case CardSet.HOF:
                    strSource = $"{ResourceBasePath}hearthstone_logo.png";
                    break;

                case CardSet.GVG:
                    strSource = $"{ResourceBasePath}goblins_logo.png";
                    break;

                case CardSet.TGT:
                    strSource = $"{ResourceBasePath}tournament_logo.png";
                    break;

                case CardSet.OG:
                    strSource = $"{ResourceBasePath}oldgods_logo.png";
                    break;

                case CardSet.GANGS:
                    strSource = $"{ResourceBasePath}gadgetzan_logo.png";
                    break;

                case CardSet.UNGORO:
                    strSource = $"{ResourceBasePath}ungoro_logo.png";
                    break;

                case CardSet.ICECROWN:
                    strSource = $"{ResourceBasePath}frozenthrone_logo.png";
                    break;

                case CardSet.LOOTAPALOOZA:
                    strSource = $"{ResourceBasePath}kobolds_logo.png";
                    break;

                case CardSet.NAXX:
                    strSource = $"{ResourceBasePath}naxx_logo.png";
                    break;

                case CardSet.BRM:
                    strSource = $"{ResourceBasePath}mountain_logo.png";
                    break;

                case CardSet.LOE:
                    strSource = $"{ResourceBasePath}league_logo.png";
                    break;

                case CardSet.KARA:
                    strSource = $"{ResourceBasePath}karazhan_logo.png";
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
                case CardSet.HOF:
                    strSource = $"{ResourceBasePath}hearthstone_banner.jpg";
                    break;

                case CardSet.GVG:
                    strSource = $"{ResourceBasePath}goblins_banner.jpg";
                    break;

                case CardSet.TGT:
                    strSource = $"{ResourceBasePath}tournament_banner.jpg";
                    break;

                case CardSet.OG:
                    strSource = $"{ResourceBasePath}oldgods_banner.jpg";
                    break;

                case CardSet.GANGS:
                    strSource = $"{ResourceBasePath}gadgetzan_banner.jpg";
                    break;

                case CardSet.UNGORO:
                    strSource = $"{ResourceBasePath}ungoro_banner.jpg";
                    break;

                case CardSet.ICECROWN:
                    strSource = $"{ResourceBasePath}frozenthrone_banner.jpg";
                    break;

                case CardSet.LOOTAPALOOZA:
                    strSource = $"{ResourceBasePath}kobolds_banner.jpg";
                    break;

                case CardSet.NAXX:
                    strSource = $"{ResourceBasePath}naxx_banner.jpg";
                    break;

                case CardSet.BRM:
                    strSource = $"{ResourceBasePath}mountain_banner.jpg";
                    break;

                case CardSet.LOE:
                    strSource = $"{ResourceBasePath}league_banner.jpg";
                    break;

                case CardSet.KARA:
                    strSource = $"{ResourceBasePath}karazhan_banner.jpg";
                    break;
            }

            return new BitmapImage(new Uri(strSource, UriKind.Relative));
        }
        #endregion

        #region GetCountForRarity
        private int GetCountForRarity(CardSet cardSet, Rarity rarity)
        {
            int nRet = 0;

            //System.Diagnostics.Debug.WriteLine($"{cardSet.ToString()} - {rarity.ToString()}");

            List<HearthMirror.Objects.Card> lstChunk = m_lstCollection.FindAll(c =>
            {
                Card card = Cards.Collectible[c.Id];

                return card.Set == cardSet && card.Rarity == rarity && !c.Premium;
            });

            int nMaxCount = 2;

            if (rarity == Rarity.LEGENDARY)
            {
                nMaxCount = 1;
            }
            else { }

            for (int i = 0; i < lstChunk.Count; i++)
            {
                nRet += Math.Min(lstChunk[i].Count, nMaxCount);

                //System.Diagnostics.Debug.WriteLine($"{Cards.All[lstChunk[i].Id].Name}: {lstChunk[i].Count}");
            }

            //System.Diagnostics.Debug.WriteLine(string.Empty);

            return nRet;
        }
        #endregion

        #region GetDustValue
        private int GetDustValue(List<HearthMirror.Objects.Card> lstCards, CardSet cardSet = CardSet.INVALID, Rarity rarity = Rarity.INVALID)
        {
            int nRet = 0;

            List<HearthMirror.Objects.Card> lstChunk = lstCards;

            if (cardSet != CardSet.INVALID || rarity != Rarity.INVALID)
            {
                if (cardSet != CardSet.INVALID && rarity != Rarity.INVALID)
                {
                    lstChunk = lstCards.FindAll(c =>
                    {
                        Card card = Cards.Collectible[c.Id];

                        return card.Set == cardSet && card.Rarity == rarity && !c.Premium;
                    });
                }
                else if (cardSet != CardSet.INVALID)
                {
                    lstChunk = lstCards.FindAll(c =>
                    {
                        Card card = Cards.Collectible[c.Id];

                        return card.Set == cardSet && !c.Premium;
                    });
                }
                else if (rarity != Rarity.INVALID)
                {
                    lstChunk = lstCards.FindAll(c =>
                    {
                        Card card = Cards.Collectible[c.Id];

                        return card.Rarity == rarity && !c.Premium;
                    });
                }
                else { }
            }
            else { }

            for (int i = 0; i < lstChunk.Count; i++)
            {
                nRet += lstChunk[i].GetDustValue() * lstChunk[i].Count;
            }

            return nRet;
        }
        #endregion

        #region UpdateContainer
        private void UpdateContainer(CardSetInfoContainer container, ListViewCardSetItem item)
        {
            CardSet cardSet = (CardSet)item.Tag;
            CardSets.Info.InfoItem infoItem = CardSets.Info.Dictionary[cardSet];

            //TotalCount
            CardCountToStringConverter totalCountConverter = GetResource<CardCountToStringConverter>(container, TotalCountConverterKey);
            totalCountConverter.MaxAmount = infoItem.TotalCount;
            totalCountConverter.Prefix = "Total:";
            totalCountConverter.Suffix = $"({Convert.ToInt32(((float)item.TotalCount / infoItem.TotalCount) * 100)}%)";

            //CommonsCount
            CardCountToStringConverter commonsCountConverter = GetResource<CardCountToStringConverter>(container, CommonsCountConverterKey);
            commonsCountConverter.MaxAmount = infoItem.MaxCommonsCount;
            commonsCountConverter.Prefix = "Common:";
            commonsCountConverter.Suffix = $"({GetDustValue(m_lstCollection, cardSet, Rarity.COMMON)} Dust)";

            //RaresCount
            CardCountToStringConverter raresCountConverter = GetResource<CardCountToStringConverter>(container, RaresCountConverterKey);
            raresCountConverter.MaxAmount = infoItem.MaxRaresCount;
            raresCountConverter.Prefix = "Rare:";
            raresCountConverter.Suffix = $"({GetDustValue(m_lstCollection, cardSet, Rarity.RARE)} Dust)";

            //EpicsCount
            CardCountToStringConverter epicsCountConverter = GetResource<CardCountToStringConverter>(container, EpicsCountConverterKey);
            epicsCountConverter.MaxAmount = infoItem.MaxEpicsCount;
            epicsCountConverter.Prefix = "Epic:";
            epicsCountConverter.Suffix = $"({GetDustValue(m_lstCollection, cardSet, Rarity.EPIC)} Dust)";

            //LegendariesCount
            CardCountToStringConverter legendariesCountConverter = GetResource<CardCountToStringConverter>(container, LegendariesCountConverterKey);
            legendariesCountConverter.MaxAmount = infoItem.MaxLegendariesCount;
            legendariesCountConverter.Prefix = "Legendary:";
            legendariesCountConverter.Suffix = $"({GetDustValue(m_lstCollection, cardSet, Rarity.LEGENDARY)} Dust)";
        }
        #endregion

        #region GetResource
        private T GetResource<T>(CardSetInfoContainer container, string strKey)
        {
            return (T)container.FindResource(strKey);
        }
        #endregion

        #region FindVisualChildren
        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
        #endregion
    }
}