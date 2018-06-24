#region Using
using HearthMirror.Objects;
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement.Offline
{
    public static class HistoryManager
    {
        #region Static Fields
        private static CardComparer s_cardComparer = new CardComparer();
        private static bool s_blnCheckInProgress;
        #endregion

        #region CheckCollection
        public static void CheckCollection(IAccount account)
        {
            List<Card> lstCurrentCollection = DustUtilityPlugin.GetCollectionWrapper();

            if (!s_blnCheckInProgress && account != null && (lstCurrentCollection != null && lstCurrentCollection.Count > 0))
            {
                s_blnCheckInProgress = true;

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Checking for any changes... ({account.DisplayString})");

                List<Card> lstOldCollection = account.GetCollection();

                List<CachedHistoryCard> lstHistory = LoadHistory(account);

                if (lstOldCollection != null && lstOldCollection.Count > 0)
                {
                    List<Card> lstCurrent = lstCurrentCollection.Except(lstOldCollection, s_cardComparer).ToList();
                    List<Card> lstOld = lstOldCollection.Except(lstCurrentCollection, s_cardComparer).ToList();

                    int nChanges = 0;

                    //new cards
                    nChanges += CheckForNewCards(lstOldCollection, lstHistory, lstCurrent, lstOld);

                    //disenchanted cards
                    nChanges += CheckForDisenchantedCards(lstCurrentCollection, lstHistory, lstOld);

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Found {nChanges} changes");

                    SaveHistory(account, lstHistory);
                }
                else { }

                s_blnCheckInProgress = false;
            }
            else { }
        }
        #endregion

        #region CheckForNewCards
        private static int CheckForNewCards(List<Card> lstOldCollection, List<CachedHistoryCard> lstHistory, List<Card> lstCurrent, List<Card> lstOld)
        {
            int nRet = 0;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Checking for new cards...");

            for (int i = 0; i < lstCurrent.Count; i++)
            {
                Card cardA = lstCurrent[i];

                HearthDb.Card dbCardA = null;

                if (HearthDb.Cards.Collectible.ContainsKey(cardA.Id))
                {
                    dbCardA = HearthDb.Cards.Collectible[cardA.Id];
                }
                else { }

                if (dbCardA != null && (CardSets.All.ContainsKey(dbCardA.Set) && lstOld.Find(c => c.Id.Equals(cardA.Id) && c.Premium == cardA.Premium) == null))
                {
                    Card cardB = lstOldCollection.Find(c => c.Id.Equals(cardA.Id) && c.Premium == cardA.Premium);

                    int nCount = cardA.Count;

                    if (cardB != null)
                    {
                        nCount = cardA.Count - cardB.Count;
                    }
                    else { }

                    lstHistory.Add(new CachedHistoryCard
                    {
                        Id = cardA.Id,
                        Count = nCount,
                        IsGolden = cardA.Premium,
                        Date = DateTime.Now
                    });

                    nRet += 1;
                }
                else { }
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Found {nRet} new card(s)");

            return nRet;
        }
        #endregion

        #region CheckForDisenchantedCards
        private static int CheckForDisenchantedCards(List<Card> lstCurrentCollection, List<CachedHistoryCard> lstHistory, List<Card> lstOld)
        {
            int nRet = 0;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Checking for disenchanted cards...");

            for (int i = 0; i < lstOld.Count; i++)
            {
                Card cardB = lstOld[i];

                HearthDb.Card dbCardB = HearthDb.Cards.Collectible[cardB.Id];

                if (CardSets.All.ContainsKey(dbCardB.Set))
                {
                    Card cardA = lstCurrentCollection.Find(c => c.Id.Equals(cardB.Id) && c.Premium == cardB.Premium);

                    int nCount = cardB.Count;

                    if (cardA != null)
                    {
                        nCount = cardB.Count - cardA.Count;
                    }
                    else { }

                    nCount *= -1;

                    lstHistory.Add(new CachedHistoryCard
                    {
                        Id = cardB.Id,
                        Count = nCount,
                        IsGolden = cardB.Premium,
                        Date = DateTime.Now
                    });

                    nRet += 1;
                }
                else { }
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Found {nRet} disenchanted cards...");

            return nRet;
        }
        #endregion

        #region LoadHistory
        private static List<CachedHistoryCard> LoadHistory(IAccount account)
        {
            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Loading history for {account.DisplayString}...");

            string strPath = DustUtilityPlugin.GetFullFileName(account, Account.HistoryString);

            return FileManager.Load<List<CachedHistoryCard>>(strPath);
        }
        #endregion

        #region SaveHistory
        private static void SaveHistory(IAccount account, List<CachedHistoryCard> lstHistory)
        {
#if DEBUG
            if (!(account is MockAccount))
            {
#endif

                string strPath = DustUtilityPlugin.GetFullFileName(account, Account.HistoryString);

                FileManager.Write(strPath, lstHistory);

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Saved history for {account.DisplayString}");
#if DEBUG
            }
            else { }
#endif
        }
        #endregion

        #region GetHistory
        public static List<CachedHistoryCard> GetHistory(IAccount account)
        {
            List<CachedHistoryCard> lstHistory = LoadHistory(account);

            //List<IGrouping<string, Card>> lstGroupedById = lstHistory.GroupBy(c => c.Id).ToList();

            //lstHistory.Clear();

            //for (int i = 0; i < lstGroupedById.Count; i++)
            //{
            //    List<IGrouping<bool, Card>> lstGroupedByPremium = lstGroupedById[i].GroupBy(c => c.Premium).ToList();

            //    for (int j = 0; j < lstGroupedByPremium.Count; j++)
            //    {
            //        IGrouping<bool, Card> grouping = lstGroupedByPremium[j];

            //        lstHistory.Add(grouping.Aggregate((a, b) => new Card(a.Id, a.Count + b.Count, a.Premium)));
            //    }
            //}

            lstHistory.Reverse();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Retrieved history for {account.DisplayString}");

            return lstHistory;
        }
        #endregion

        #region ClearHistory
        public static void ClearHistory(IAccount account)
        {
            SaveHistory(account, new List<CachedHistoryCard>());

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Cleared history ({account.DisplayString})");
        }
        #endregion

        #region RemoveItemAt
        public static void RemoveItemAt(IAccount account, int nIndex)
        {
            List<CachedHistoryCard> lstHistory = account.GetHistory();

            lstHistory.RemoveAt(nIndex);

            lstHistory.Reverse();

            SaveHistory(account, lstHistory);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Removed item at index '{nIndex}' ({account.DisplayString})");
        }
        #endregion

        private class CardComparer : IEqualityComparer<Card>
        {
            #region Equals
            public bool Equals(Card x, Card y)
            {
                return x.Id.Equals(y.Id) && x.Premium == y.Premium && x.Count == y.Count;
            }
            #endregion

            #region GetHashCode
            public int GetHashCode(Card obj)
            {
                return obj.Id.GetHashCode() + obj.Premium.GetHashCode() + obj.Count.GetHashCode();
            }
            #endregion
        }
    }
}