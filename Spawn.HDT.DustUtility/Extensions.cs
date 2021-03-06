﻿#region Using
using HearthDb;
using HearthDb.Enums;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Hearthstone;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Media.Imaging;
#endregion

namespace Spawn.HDT.DustUtility
{
    public static class Extensions
    {
        #region ContainsCard
        public static bool ContainsCard(this HearthMirror.Objects.Deck deck, string strId, bool blnPremium) => GetCard(deck, strId, blnPremium) != null;
        #endregion

        #region GetCard
        public static HearthMirror.Objects.Card GetCard(this HearthMirror.Objects.Deck deck, string strId, bool blnPremium) => deck.Cards.Find(delegate (HearthMirror.Objects.Card c) { return string.CompareOrdinal(c.Id, strId) == 0 && c.Premium == blnPremium; });
        #endregion

        #region GetDustValue
        public static int GetDustValue(this HearthMirror.Objects.Card card)
        {
            int nRet = 0;

            if (card != null)
            {
                Card c = Cards.All[card.Id];

                if (c.Set != CardSet.CORE)
                {
                    bool blnIsCollectible = card.Premium ? !CardSets.NonCraftableGoldenCardIds.Contains(c.Id) : !CardSets.NonCraftableRegularCardIds.Contains(c.Id);

                    if (blnIsCollectible)
                    {
                        switch (c.Rarity)
                        {
                            case Rarity.COMMON:
                                nRet = (card.Premium ? 50 : 5);
                                break;

                            case Rarity.RARE:
                                nRet = (card.Premium ? 100 : 20);
                                break;

                            case Rarity.EPIC:
                                nRet = (card.Premium ? 400 : 100);
                                break;

                            case Rarity.LEGENDARY:
                                nRet = (card.Premium ? 1600 : 400);
                                break;
                        }
                    }
                }
            }

            return nRet;
        }
        #endregion

        #region ToCachedCards
        public static List<CachedCard> ToCachedCards(this List<HearthMirror.Objects.Card> lstCards)
        {
            List<CachedCard> lstRet = new List<CachedCard>();

            for (int i = 0; i < lstCards?.Count; i++)
            {
                HearthMirror.Objects.Card card = lstCards[i];

                CachedCard cachedCard = new CachedCard()
                {
                    Id = card.Id,
                    Count = card.Count,
                    IsGolden = card.Premium
                };

                lstRet.Add(cachedCard);
            }

            return lstRet;
        }
        #endregion

        #region ToCachedDecks
        public static List<CachedDeck> ToCachedDecks(this List<HearthMirror.Objects.Deck> lstDecks)
        {
            List<CachedDeck> lstRet = new List<CachedDeck>();

            for (int i = 0; i < lstDecks?.Count; i++)
            {
                HearthMirror.Objects.Deck deck = lstDecks[i];

                CachedDeck cachedDeck = new CachedDeck()
                {
                    Id = deck.Id,
                    Name = deck.Name,
                    Hero = deck.Hero,
                    IsWild = deck.IsWild,
                    Type = deck.Type,
                    SeasonId = deck.SeasonId,
                    CardBackId = deck.CardBackId,
                    HeroPremium = deck.HeroPremium,
                    Cards = deck.Cards.ToCachedCards()
                };

                lstRet.Add(cachedDeck);
            }

            return lstRet;
        }
        #endregion

        #region ToCards
        public static List<HearthMirror.Objects.Card> ToCards(this List<CachedCard> lstCachedCards)
        {
            List<HearthMirror.Objects.Card> lstRet = new List<HearthMirror.Objects.Card>();

            for (int i = 0; i < lstCachedCards?.Count; i++)
            {
                CachedCard cachedCard = lstCachedCards[i];

                lstRet.Add(new HearthMirror.Objects.Card(cachedCard.Id, cachedCard.Count, cachedCard.IsGolden));
            }

            return lstRet;
        }
        #endregion

        #region GetString
        public static string GetString(this CardSet cardSet) => CardSets.All[cardSet];

        public static string GetDisplayString(this CardSet cardSet) => CardSets.AllDisplayName[cardSet];

        public static string GetShortString(this CardSet cardSet) => CardSets.AllShortName[cardSet];

        public static string GetString(this Rarity rarity) => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(rarity.ToString().ToLowerInvariant());

        public static string GetString(this CardClass cardClass)
        {
            string strRet = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(cardClass.ToString().ToLowerInvariant());

            if (cardClass == CardClass.DEMONHUNTER)
                strRet = "DH";

            return strRet;
        }
        #endregion

        #region OrderBy
        //All credits to Aaron Powell https://stackoverflow.com/a/307600
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string strProperty, int nIteration)
        {
            System.Type type = typeof(T);

            System.Reflection.PropertyInfo property = type.GetProperty(strProperty);

            ParameterExpression parameter = Expression.Parameter(type, "p");

            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);

            LambdaExpression expr = Expression.Lambda(propertyAccess, parameter);

            string strMethod = "OrderBy";

            if (nIteration > 0)
                strMethod = "ThenBy";

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), strMethod, new System.Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(expr));

            return source.Provider.CreateQuery<T>(resultExp);
        }
        #endregion

        #region ToBitmapImage
        public static BitmapImage ToBitmapImage(this System.Drawing.Bitmap bitmap, ImageFormat imageFormat)
        {
            BitmapImage retVal = null;

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, imageFormat);

                ms.Position = 0;

                retVal = new BitmapImage();

                retVal.BeginInit();
                retVal.CacheOption = BitmapCacheOption.OnLoad;
                retVal.StreamSource = ms;
                retVal.EndInit();
            }

            return retVal;
        }
        #endregion

        #region GetCraftingCost
        public static int GetCraftingCost(this HearthMirror.Objects.Deck deck)
        {
            int nRet = 0;

            for (int i = 0; i < deck?.Cards.Count; i++)
                nRet += deck.Cards[i].GetCraftingCost() * deck.Cards[i].Count;

            return nRet;
        }

        public static int GetCraftingCost(this HearthMirror.Objects.Card card)
        {
            int nRet = 0;

            if (card != null)
            {
                Card c = Cards.All[card.Id];

                if (c.Set != CardSet.CORE)
                {
                    bool blnIsCollectible = card.Premium ? !CardSets.NonCraftableGoldenCardIds.Contains(c.Id) : !CardSets.NonCraftableRegularCardIds.Contains(c.Id);

                    if (blnIsCollectible)
                    {
                        switch (c.Rarity)
                        {
                            case Rarity.COMMON:
                                nRet = (card.Premium ? 400 : 40);
                                break;

                            case Rarity.RARE:
                                nRet = (card.Premium ? 800 : 100);
                                break;

                            case Rarity.EPIC:
                                nRet = (card.Premium ? 1600 : 400);
                                break;

                            case Rarity.LEGENDARY:
                                nRet = (card.Premium ? 3200 : 1600);
                                break;
                        }
                    }
                }
            }

            return nRet;
        }
        #endregion

        #region GetTotalCardCount
        public static int GetTotalCardCount(this HearthMirror.Objects.Deck deck)
        {
            int nRet = 0;

            for (int i = 0; i < deck?.Cards.Count; i++)
                nRet += deck.Cards[i].Count;

            return nRet;
        }
        #endregion

        #region Clone
        #region HearthMirror.Objects.Card
        public static HearthMirror.Objects.Card Clone(this HearthMirror.Objects.Card card) => new HearthMirror.Objects.Card(card.Id, card.Count, card.Premium);
        #endregion

        #region HearthMirror.Objects.Deck
        public static HearthMirror.Objects.Deck Clone(this HearthMirror.Objects.Deck deck) => new HearthMirror.Objects.Deck()
        {
            Id = deck.Id,
            Name = deck.Name,
            Hero = deck.Hero,
            IsWild = deck.IsWild,
            Type = deck.Type,
            SeasonId = deck.SeasonId,
            CardBackId = deck.CardBackId,
            HeroPremium = deck.HeroPremium,
            SourceType = deck.SourceType,
            CreateDate = deck.CreateDate,
            Cards = deck.Cards.Select(c => c.Clone()).ToList()
        };

        #endregion
        #endregion

        #region FindParent
        public static T FindParent<T>(this System.Windows.FrameworkElement element) where T : System.Windows.DependencyObject
        {
            T retVal = null;

            if (element?.Parent != null)
                retVal = element.Parent is T ? (T)element.Parent : FindParent<T>(element.Parent as System.Windows.FrameworkElement);

            return retVal;
        }
        #endregion
    }
}