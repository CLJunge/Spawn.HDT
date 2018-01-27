using HearthMirror;
using HearthMirror.Objects;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.Hearthstone;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spawn.HDT.DustUtility.Offline
{
    public static class HistoryManager
    {
        #region Static Fields
        private static CardComparer s_cardComparer = new CardComparer();
        private static bool s_blnCheckInProgress;
        #endregion

        #region CheckCollection
        public static void CheckCollection(Account account)
        {
            List<Card> lstCurrentCollection = Reflection.GetCollection();

            if (!s_blnCheckInProgress && account != null && (lstCurrentCollection != null && lstCurrentCollection.Count > 0))
            {
                s_blnCheckInProgress = true;

                Log.WriteLine($"Checking for changes for \"{account.AccountString}\"...", LogType.Debug);

                List<Card> lstOldCollection = account.GetCollection();

                List<CachedCardEx> lstCardsHistory = LoadHistory(account);

                if (lstOldCollection != null && lstOldCollection.Count > 0)
                {
                    List<Card> lstCurrent = lstCurrentCollection.Except(lstOldCollection, s_cardComparer).ToList();
                    List<Card> lstOld = lstOldCollection.Except(lstCurrentCollection, s_cardComparer).ToList();

                    int nChanges = 0;

                    //new cards
                    nChanges += CheckForNewCards(lstOldCollection, lstCardsHistory, lstCurrent, lstOld);

                    //disenchanted cards
                    nChanges += CheckForDisenchantedCards(lstCurrentCollection, lstCardsHistory, lstOld);

                    Log.WriteLine($"Found {nChanges} changes", LogType.Debug);

                    SaveHistory(account, lstCardsHistory);
                }
                else { }

                s_blnCheckInProgress = false;
            }
            else { }
        }
        #endregion

        #region CheckForNewCards
        private static int CheckForNewCards(List<Card> lstOldCollection, List<CachedCardEx> lstCardsHistory, List<Card> lstCurrent, List<Card> lstOld)
        {
            int nRet = 0;

            for (int i = 0; i < lstCurrent.Count; i++)
            {
                Card cardA = lstCurrent[i];

                HearthDb.Card dbCardA = HearthDb.Cards.All[cardA.Id];

                if (CardSets.All.ContainsKey(dbCardA.Set) && lstOld.Find(c => c.Id.Equals(cardA.Id) && c.Premium == cardA.Premium) == null)
                {
                    Card cardB = lstOldCollection.Find(c => c.Id.Equals(cardA.Id) && c.Premium == cardA.Premium);

                    int nCount = cardA.Count;

                    if (cardB != null)
                    {
                        nCount = cardA.Count - cardB.Count;
                    }
                    else { }

                    lstCardsHistory.Add(new CachedCardEx
                    {
                        Id = cardA.Id,
                        Count = nCount,
                        IsGolden = cardA.Premium,
                        Timestamp = DateTime.Now
                    });

                    nRet += 1;
                }
                else { }
            }

            return nRet;
        }
        #endregion

        #region CheckForDisenchantedCards
        private static int CheckForDisenchantedCards(List<Card> lstCurrentCollection, List<CachedCardEx> lstCardsHistory, List<Card> lstOld)
        {
            int nRet = 0;

            for (int i = 0; i < lstOld.Count; i++)
            {
                Card cardB = lstOld[i];

                HearthDb.Card dbCardB = HearthDb.Cards.All[cardB.Id];

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

                    lstCardsHistory.Add(new CachedCardEx
                    {
                        Id = cardB.Id,
                        Count = nCount,
                        IsGolden = cardB.Premium,
                        Timestamp = DateTime.Now
                    });

                    nRet += 1;
                }
                else { }
            }

            return nRet;
        }
        #endregion

        #region LoadHistory
        private static List<CachedCardEx> LoadHistory(Account account)
        {
            Log.WriteLine($"Loading history for \"{account.AccountString}\"", LogType.Debug);

            string strPath = DustUtilityPlugin.GetFullFileName(account, Account.HistoryString);

            return FileManager.Load<List<CachedCardEx>>(strPath);
        }
        #endregion

        #region SaveHistory
        private static void SaveHistory(Account account, List<CachedCardEx> lstCardsHistory)
        {
            Log.WriteLine($"Saving history for \"{account.AccountString}\"", LogType.Debug);

            string strPath = DustUtilityPlugin.GetFullFileName(account, Account.HistoryString);

            FileManager.Write(strPath, lstCardsHistory);
        }
        #endregion

        #region GetHistory
        public static List<CachedCardEx> GetHistory(Account account)
        {
            List<CachedCardEx> lstHistory = LoadHistory(account);

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

            return lstHistory;
        }
        #endregion

        #region ClearHistory
        public static void ClearHistory(Account account)
        {
            SaveHistory(account, new List<CachedCardEx>());

            Log.WriteLine($"Cleared history for \"{account.AccountString}\"", LogType.Debug);
        }
        #endregion

        #region RemoveItemAt
        public static void RemoveItemAt(Account account, int nIndex)
        {
            Log.WriteLine($"Removing item at index \"{nIndex}\" for \"{account.AccountString}\"", LogType.Debug);

            List<CachedCardEx> lstHistory = GetHistory(account);

            lstHistory.RemoveAt(nIndex);

            lstHistory.Reverse();

            SaveHistory(account, lstHistory);
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