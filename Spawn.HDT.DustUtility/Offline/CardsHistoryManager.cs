using HearthMirror;
using HearthMirror.Objects;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HearthstonePackHistory.Hearthstone;
using System.Collections.Generic;
using System.Linq;

namespace Spawn.HDT.DustUtility.Offline
{
    public static class CardsHistoryManager
    {
        #region Constants
        private const string HistoryString = "history";
        #endregion

        #region Static Variables
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

                List<Card> lstOldCollection = Cache.LoadCollection(account);

                List<Card> lstCardsHistory = LoadHistory(account);

                if (lstOldCollection != null && lstOldCollection.Count > 0)
                {
                    List<Card> lstCurrent = lstCurrentCollection.Except(lstOldCollection, s_cardComparer).ToList();
                    List<Card> lstOld = lstOldCollection.Except(lstCurrentCollection, s_cardComparer).ToList();

                    int nChanges = 0;

                    //new cards
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

                            lstCardsHistory.Add(new Card(cardA.Id, nCount, cardA.Premium));

                            nChanges += 1;
                        }
                        else { }
                    }

                    //disenchanted cards
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

                            lstCardsHistory.Add(new Card(cardB.Id, nCount, cardB.Premium));

                            nChanges += 1;
                        }
                        else { }
                    }

                    Log.WriteLine($"Found {nChanges} changes", LogType.Debug);

                    SaveHistory(account, lstCardsHistory);
                }
                else { }

                s_blnCheckInProgress = false;
            }
            else { }
        }
        #endregion

        #region LoadHistory
        private static List<Card> LoadHistory(Account account)
        {
            string strPath = DustUtilityPlugin.GetFullFileName(account, HistoryString);

            List<CachedCard> lstCachedCards = FileManager.Load<List<CachedCard>>(strPath);

            return lstCachedCards.ToCards();
        }
        #endregion

        #region SaveHistory
        private static void SaveHistory(Account account, List<Card> lstCardsHistory)
        {
            string strPath = DustUtilityPlugin.GetFullFileName(account, HistoryString);

            FileManager.Write(strPath, lstCardsHistory.ToCachedCards());
        }
        #endregion

        #region GetHistory
        public static List<Card> GetHistory(Account account)
        {
            List<Card> lstHistory = LoadHistory(account);

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

        private class CardComparer : IEqualityComparer<Card>
        {
            public bool Equals(Card x, Card y)
            {
                return x.Id.Equals(y.Id) && x.Premium == y.Premium && x.Count == y.Count;
            }

            public int GetHashCode(Card obj)
            {
                return obj.Id.GetHashCode() + obj.Premium.GetHashCode() + obj.Count.GetHashCode();
            }
        }
    }
}