using HearthMirror;
using HearthMirror.Objects;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Spawn.HDT.DustUtility.Offline
{
    public static class Cache
    {
        #region Constants
        public const string CollectionString = "collection";
        public const string DecksString = "decks";
        #endregion

        #region Static Variables
        private static Timer s_timer;

        private static bool s_blnSaveCollectionInProgress;
        private static bool s_blnSaveDecksInProgress;
        #endregion

        #region Static Properties
        public static bool TimerEnabled => s_timer != null;

        //public static bool SaveProcessSuccessful => s_blnSavedCollection && s_blnSavedDecks;
        #endregion

        #region SaveCollection
        private static bool SaveCollection(Account account)
        {
            bool blnRet = false;

            List<Card> lstCollection = Reflection.GetCollection();

            if (lstCollection != null && lstCollection.Count > 0 && !s_blnSaveCollectionInProgress)
            {
                s_blnSaveCollectionInProgress = true;

                string strPath = DustUtilityPlugin.GetFullFileName(account, CollectionString);

                FileManager.Write(strPath, lstCollection.ToCachedCards());

                blnRet = true;

                s_blnSaveCollectionInProgress = false;
            }
            else { }

            return blnRet;
        }
        #endregion

        #region SaveDecks
        private static bool SaveDecks(Account account)
        {
            bool blnRet = false;

            List<Deck> lstAllDecks = Reflection.GetDecks();

            if (lstAllDecks != null && (lstAllDecks.Count > 0 && lstAllDecks[0].Cards.Count > 0) && !s_blnSaveDecksInProgress)
            {
                s_blnSaveDecksInProgress = true;

                string strPath = DustUtilityPlugin.GetFullFileName(account, DecksString);

                List<Deck> lstDecks = new List<Deck>(lstAllDecks.Count);

                for (int i = 0; i < lstAllDecks.Count; i++)
                {
                    if (lstAllDecks[i].Type == 1)
                    {
                        lstDecks.Add(lstAllDecks[i]);
                    }
                    else { }
                }

                FileManager.Write(strPath, lstDecks.ToCachedDecks());

                blnRet = true;

                s_blnSaveDecksInProgress = false;
            }
            else { }

            return blnRet;
        }
        #endregion

        #region LoadCollection
        public static List<Card> LoadCollection(Account account)
        {
            List<Card> lstRet = new List<Card>();

            try
            {
                string strPath = DustUtilityPlugin.GetFullFileName(account, CollectionString);

                if (File.Exists(strPath))
                {
                    List<CachedCard> lstCachedCards = FileManager.Load<List<CachedCard>>(strPath);

                    for (int i = 0; i < lstCachedCards.Count; i++)
                    {
                        CachedCard cachedCard = lstCachedCards[i];

                        lstRet.Add(new Card(cachedCard.Id, cachedCard.Count, cachedCard.IsGolden));
                    }
                }
                else { }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine($"Exception occured while loading collection: {ex}", LogType.Error);
            }

            return lstRet;
        }
        #endregion

        #region LoadDecks
        public static List<Deck> LoadDecks(Account account)
        {
            List<Deck> lstRet = new List<Deck>();

            try
            {
                string strPath = DustUtilityPlugin.GetFullFileName(account, DecksString);

                if (File.Exists(strPath))
                {
                    List<CachedDeck> lstCachedDecks = FileManager.Load<List<CachedDeck>>(strPath);

                    for (int i = 0; i < lstCachedDecks.Count; i++)
                    {
                        CachedDeck cachedDeck = lstCachedDecks[i];

                        Deck deck = new Deck()
                        {
                            Id = cachedDeck.Id,
                            Name = cachedDeck.Name,
                            Hero = cachedDeck.Hero,
                            IsWild = cachedDeck.IsWild,
                            Type = cachedDeck.Type,
                            SeasonId = cachedDeck.SeasonId,
                            CardBackId = cachedDeck.CardBackId,
                            HeroPremium = cachedDeck.HeroPremium,
                            Cards = cachedDeck.Cards.ToCards()
                        };

                        lstRet.Add(deck);
                    }
                }
                else { }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine($"Exception occured while loading decks: {ex}", LogType.Error);
            }

            return lstRet;
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

            s_timer = new Timer(OnTick, null, 0, 1000 * Settings.SaveInterval);

            Log.WriteLine($"Started cache timer (Interval={Settings.SaveInterval}s)", LogType.Debug);
        }
        #endregion

        #region StopTimer
        public static void StopTimer()
        {
            s_timer.Dispose();
            s_timer = null;

            Log.WriteLine("Stopped cache timer", LogType.Debug);
        }
        #endregion

        #region OnTick
        private static void OnTick(object state)
        {
            Log.WriteLine("Cache OnTick", LogType.Debug);

            Account account = Account.Current;

            if (!account.IsEmpty && account.IsValid)
            {
                HistoryManager.CheckCollection(account);

                Log.WriteLine("Saving collection", LogType.Debug);

                if (SaveCollection(account))
                {
                    Log.WriteLine("Saved collection successfuly", LogType.Info);
                }
                else { }

                Log.WriteLine("Saving decks", LogType.Debug);

                if (SaveDecks(account))
                {
                    Log.WriteLine("Saved decks successfuly", LogType.Info);
                }
                else { }
            }
            else
            {
                Log.WriteLine("Couldn't retrieve account", LogType.Debug);
            }
        }
        #endregion
    }
}