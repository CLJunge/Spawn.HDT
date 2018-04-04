#region Using
using HearthDb.Enums;
using HearthMirror.Objects;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class CollectionInfoFlyoutViewModel : ViewModelBase
    {
        #region Constants
        private const string ResourceBasePath = "/Spawn.HDT.DustUtility;component/Resources/Images/";
        #endregion

        #region Properties
        public ObservableCollection<CardSetItemModel> CardSetItems { get; set; }
        #endregion

        #region Ctor
        public CollectionInfoFlyoutViewModel()
        {
            CardSetItems = new ObservableCollection<CardSetItemModel>();

            if (IsInDesignMode)
            {
                ReloadRequired = true;

                Initialize();
            }
            else { }
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            if (ReloadRequired)
            {
                CardSetItems.Clear();

                List<Card> lstCollection = DustUtilityPlugin.CurrentAccount.GetCollection();

                List<CardSet> lstCardSets = new List<CardSet>(CardSets.All.Keys);

                for (int i = 0; i < lstCardSets.Count; i++)
                {
                    CardSet cardSet = lstCardSets[i];

                    if (HasCardSet(lstCollection, cardSet))
                    {
                        CardSetItems.Add(CreateCardSetItem(cardSet));
                    }
                    else { }
                }

                ReloadRequired = false;
            }
            else { }
        }
        #endregion

        #region HasCardSet
        private bool HasCardSet(List<Card> lstCollection, CardSet cardSet)
        {
            bool blnRet = false;

            if (lstCollection.Count > 0)
            {
                blnRet = lstCollection.Find(c =>
                {
                    return HearthDb.Cards.Collectible[c.Id].Set == cardSet;
                }) != null;
            }
            else { }

            return blnRet;
        }
        #endregion

        #region CreateCardSetItem
        private CardSetItemModel CreateCardSetItem(CardSet cardSet)
        {
            List<Card> lstCollection = DustUtilityPlugin.CurrentAccount.GetCollection();

            CardSets.Info.InfoItem cardSetInfo = CardSets.Info.Dictionary[cardSet];

            return new CardSetItemModel
            {
                Logo = GetLogo(cardSet),
                BackgroundImage = GetBanner(cardSet),
                Name = CardSets.AllFullName[cardSet],
                CommonsCount = GetCountForRarity(lstCollection, cardSet, Rarity.COMMON, false),
                RaresCount = GetCountForRarity(lstCollection, cardSet, Rarity.RARE, false),
                EpicsCount = GetCountForRarity(lstCollection, cardSet, Rarity.EPIC, false),
                LegendariesCount = GetCountForRarity(lstCollection, cardSet, Rarity.LEGENDARY, false),
                DustAmount = GetDustValue(lstCollection, cardSet, Rarity.INVALID, false),
                GoldenCommonsCount = GetCountForRarity(lstCollection, cardSet, Rarity.COMMON, true),
                GoldenRaresCount = GetCountForRarity(lstCollection, cardSet, Rarity.RARE, true),
                GoldenEpicsCount = GetCountForRarity(lstCollection, cardSet, Rarity.EPIC, true),
                GoldenLegendariesCount = GetCountForRarity(lstCollection, cardSet, Rarity.LEGENDARY, true),
                GoldenDustAmount = GetDustValue(lstCollection, cardSet, Rarity.INVALID, true),
                MaxCommonsCount = cardSetInfo.MaxCommonsCount,
                MaxRaresCount = cardSetInfo.MaxRaresCount,
                MaxEpicsCount = cardSetInfo.MaxEpicsCount,
                MaxLegendariesCount = cardSetInfo.MaxLegendariesCount
            };
        }
        #endregion

        #region GetLogo
        private BitmapImage GetLogo(CardSet cardSet)
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
        private BitmapImage GetBanner(CardSet cardSet)
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
        private int GetCountForRarity(List<Card> lstCollection, CardSet cardSet, Rarity rarity, bool blnIsGolden)
        {
            int nRet = 0;

            //System.Diagnostics.Debug.WriteLine($"{cardSet.ToString()} - {rarity.ToString()}");

            List<Card> lstChunk = lstCollection.FindAll(c =>
            {
                HearthDb.Card card = HearthDb.Cards.Collectible[c.Id];

                return card.Set == cardSet && card.Rarity == rarity && c.Premium == blnIsGolden;
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
        private int GetDustValue(List<Card> lstCards, CardSet cardSet, Rarity rarity, bool blnIsGolden)
        {
            int nRet = 0;

            List<Card> lstChunk = lstCards;

            if (cardSet != CardSet.INVALID || rarity != Rarity.INVALID)
            {
                if (cardSet != CardSet.INVALID && rarity != Rarity.INVALID)
                {
                    lstChunk = lstCards.FindAll(c =>
                    {
                        HearthDb.Card card = HearthDb.Cards.Collectible[c.Id];

                        return card.Set == cardSet && card.Rarity == rarity && c.Premium == blnIsGolden;
                    });
                }
                else if (cardSet != CardSet.INVALID)
                {
                    lstChunk = lstCards.FindAll(c =>
                    {
                        HearthDb.Card card = HearthDb.Cards.Collectible[c.Id];

                        return card.Set == cardSet && c.Premium == blnIsGolden;
                    });
                }
                else if (rarity != Rarity.INVALID)
                {
                    lstChunk = lstCards.FindAll(c =>
                    {
                        HearthDb.Card card = HearthDb.Cards.Collectible[c.Id];

                        return card.Rarity == rarity && c.Premium == blnIsGolden;
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
    }
}