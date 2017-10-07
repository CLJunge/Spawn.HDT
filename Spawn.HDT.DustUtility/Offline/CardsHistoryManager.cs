using HearthMirror;
using HearthMirror.Objects;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        private static Timer s_timer;
        #endregion

        #region CheckCollection
        private static void CheckCollection(Account account)
        {
            List<Card> lstCurrentCollection = Reflection.GetCollection();

            if (!s_blnCheckInProgress && account != null && (lstCurrentCollection != null && lstCurrentCollection.Count > 0))
            {
                s_blnCheckInProgress = true;

                List<Card> lstOldCollection = Cache.LoadCollection(account);

                List<Card> lstCardsHistory = LoadHistoryFile(account);

                if (lstOldCollection != null && lstOldCollection.Count > 0)
                {
                    List<Card> a = lstCurrentCollection.Except(lstOldCollection, s_cardComparer).ToList();

                    List<Card> b = lstOldCollection.Except(lstCurrentCollection, s_cardComparer).ToList();

                    for (int i = 0; i < b.Count; i++)
                    {
                        Card cardB = b[i];

                        Card cardA = a.Find(c => c.Id.Equals(cardB.Id) && c.Premium == cardB.Premium);

                        //int nCount = cardB.Count;

                        //if (cardA != null)
                        //{
                        //    nCount = cardB.Count - cardA.Count;
                        //}
                        //else { }

                        //lstCardsHistory.Add(new Card(cardB.Id, nCount, cardB.Premium));
                    }

                    //SaveHistoryFile(account, lstCardsHistory);
                }
                else { }

                s_blnCheckInProgress = false;
            }
            else { }
        }
        #endregion

        #region LoadHistoryFile
        private static List<Card> LoadHistoryFile(Account account)
        {
            string strPath = DustUtilityPlugin.GetFullFileName(account, HistoryString);

            List<CachedCard> lstCachedCards = FileManager.Load<List<CachedCard>>(strPath);

            return lstCachedCards.ToCards();
        }
        #endregion

        #region SaveHistoryFile
        private static void SaveHistoryFile(Account account, List<Card> lstCardsHistory)
        {
            string strPath = DustUtilityPlugin.GetFullFileName(account, HistoryString);

            FileManager.Write(strPath, lstCardsHistory.ToCachedCards());
        }
        #endregion

        #region GetHistory
        public static List<Card> GetHistory(Account account)
        {
            List<Card> lstHistory = LoadHistoryFile(account);

            List<IGrouping<string, Card>> lstGroupedById = lstHistory.GroupBy(c => c.Id).ToList();

            lstHistory.Clear();

            for (int i = 0; i < lstGroupedById.Count; i++)
            {
                List<IGrouping<bool, Card>> lstGroupedByPremium = lstGroupedById[i].GroupBy(c => c.Premium).ToList();

                for (int j = 0; j < lstGroupedByPremium.Count; j++)
                {
                    IGrouping<bool, Card> grouping = lstGroupedByPremium[j];

                    lstHistory.Add(grouping.Aggregate((a, b) => new Card(a.Id, a.Count + b.Count, a.Premium)));
                }
            }

            return lstHistory;
        }
        #endregion

        #region StartTimer
        public static void StartTimer()
        {
            if (s_timer != null)
            {
                StopTimer();
            }
            else { }

            s_timer = new Timer(OnTick, null, 0, 1000 * 60);

            Log.WriteLine("Started history timer", LogType.Debug);
        }
        #endregion

        #region StopTimer
        public static void StopTimer()
        {
            s_timer.Dispose();
            s_timer = null;

            Log.WriteLine("Stopped history timer", LogType.Debug);
        }
        #endregion

        #region OnTick
        private static void OnTick(object state)
        {
            Log.WriteLine("History OnTick", LogType.Debug);

            Account account = new Account(Reflection.GetBattleTag(), Helper.GetCurrentRegion().Result);

            CheckCollection(account);
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