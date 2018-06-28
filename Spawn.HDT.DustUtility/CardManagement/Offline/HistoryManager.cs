#region Using
using HearthMirror.Objects;
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement.Offline
{
    public static class HistoryManager
    {
        #region Static Fields
        private static readonly CardComparer s_cardComparer = new CardComparer();
        private static bool s_blnCheckInProgress;
        #endregion

        #region CheckCollectionForChanges
        public static Status CheckCollectionForChanges(IAccount account)
        {
            Status retVal = Status.Failed;

            List<Card> lstCurrentCollection = DustUtilityPlugin.GetCollectionWrapper();

            if (!s_blnCheckInProgress && account != null && lstCurrentCollection?.Count > 0)
            {
                s_blnCheckInProgress = true;

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Checking collection for any changes... ({account.DisplayString})");

                List<Card> lstLocalCollection = Cache.LoadCollection(account);

                List<CachedHistoryCard> lstHistory = LoadHistory(account);

                if (lstLocalCollection?.Count > 0)
                {
                    List<Card> lstCurrentExceptLocal = lstCurrentCollection.Except(lstLocalCollection, s_cardComparer).ToList();
                    List<Card> lstLocalExceptCurrent = lstLocalCollection.Except(lstCurrentCollection, s_cardComparer).ToList();

                    int nChanges = 0;

                    //new cards
                    nChanges += CheckForNewCards(lstLocalCollection, lstHistory, lstCurrentExceptLocal, lstLocalExceptCurrent);

                    //disenchanted cards
                    nChanges += CheckForDisenchantedCards(lstCurrentCollection, lstHistory, lstLocalExceptCurrent);

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Found {nChanges} changes");

                    SaveHistory(account, lstHistory);

                    retVal = Status.Success;
                }
                else
                {
                    retVal = Status.LocalCollectionMissing;
                }

                s_blnCheckInProgress = false;
            }

            return retVal;
        }
        #endregion

        #region CheckForNewCards
        private static int CheckForNewCards(List<Card> lstLocalCollection, List<CachedHistoryCard> lstHistory, List<Card> lstCurrentExceptLocal, List<Card> lstLocalExceptCurrent)
        {
            int nRet = 0;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Checking for new cards...");

            for (int i = 0; i < lstCurrentExceptLocal.Count; i++)
            {
                Card a = lstCurrentExceptLocal[i];

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Current: Name={HearthDb.Cards.All[a.Id].Name}, Count={a.Count}, Premium={a.Premium}");

                if ((HearthDb.Cards.Collectible.ContainsKey(a.Id)
                    && CardSets.All.ContainsKey(HearthDb.Cards.Collectible[a.Id].Set))
                    && lstLocalExceptCurrent.Find(c => c.Id.Equals(a.Id) && c.Premium == a.Premium) == null)
                {
                    int nCount = a.Count;

                    Card b = lstLocalCollection.Find(c => c.Id.Equals(a.Id) && c.Premium == a.Premium);

                    if (b != null)
                    {
                        nCount = a.Count - b.Count;

                        DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Found equivalent in the local collection (Count={b.Count})");
                    }

                    lstHistory.Add(new CachedHistoryCard
                    {
                        Id = a.Id,
                        Count = nCount,
                        IsGolden = a.Premium,
                        Date = DateTime.Now
                    });

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Added {nCount}x '{HearthDb.Cards.Collectible[a.Id].Name}' (Premium={a.Premium}) to history");

                    nRet += 1;
                }
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Added {nRet} new entry(-ies)");

            return nRet;
        }
        #endregion

        #region CheckForDisenchantedCards
        private static int CheckForDisenchantedCards(List<Card> lstCurrentCollection, List<CachedHistoryCard> lstHistory, List<Card> lstLocalExceptCurrent)
        {
            int nRet = 0;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Checking for disenchanted cards...");

            for (int i = 0; i < lstLocalExceptCurrent.Count; i++)
            {
                Card a = lstLocalExceptCurrent[i];

                if (HearthDb.Cards.Collectible.ContainsKey(a.Id)
                    && CardSets.All.ContainsKey(HearthDb.Cards.Collectible[a.Id].Set))
                {
                    int nCount = a.Count;

                    Card b = lstCurrentCollection.Find(c => c.Id.Equals(a.Id) && c.Premium == a.Premium);

                    if (b != null)
                    {
                        nCount = a.Count - b.Count;

                        DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Found equivalent in the current collection (Count={b.Count})");
                    }

                    nCount *= -1;

                    lstHistory.Add(new CachedHistoryCard
                    {
                        Id = a.Id,
                        Count = nCount,
                        IsGolden = a.Premium,
                        Date = DateTime.Now
                    });

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Added {nCount}x '{HearthDb.Cards.Collectible[a.Id].Name}' (Premium={a.Premium}) to history");

                    nRet += 1;
                }
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

            return FileHelper.Load<List<CachedHistoryCard>>(strPath);
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

                FileHelper.Write(strPath, lstHistory);

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Saved history for {account.DisplayString}");
#if DEBUG
            }
#endif
        }
        #endregion

        #region GetHistory
        public static List<CachedHistoryCard> GetHistory(IAccount account)
        {
            List<CachedHistoryCard> lstRet = LoadHistory(account);

            lstRet.Reverse();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Retrieved history for {account.DisplayString}");

            return lstRet;
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

        public enum Status
        {
            Failed,
            Success,
            LocalCollectionMissing
        }

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
                return (obj != null ? (obj.Id?.GetHashCode() ?? 0) + obj.Premium.GetHashCode() + obj.Count.GetHashCode() : 0);
            }
            #endregion
        }
    }
}