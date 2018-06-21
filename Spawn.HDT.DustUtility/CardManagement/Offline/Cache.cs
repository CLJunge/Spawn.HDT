#region Using
using CommonServiceLocator;
using HearthMirror;
using HearthMirror.Objects;
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement.Offline
{
    public static class Cache
    {
        #region Static Fields
        private static Timer s_timer;

        private static bool s_blnSaveCollectionInProgress;
        private static bool s_blnSaveDecksInProgress;

        private static List<Card> m_lstCachedCollection;
        private static List<Deck> m_lstCachedDecks;
        #endregion

        #region Static Properties
        public static bool TimerEnabled => s_timer != null;
        #endregion

        #region ForceSave
        public static void ForceSave(IAccount account)
        {
            OnTick(account);
        }
        #endregion

        #region SaveCollection
        private static bool SaveCollection(IAccount account)
        {
            bool blnRet = false;

            List<Card> lstCollection = DustUtilityPlugin.GetCollectionWrapper();

            if (lstCollection?.Count > 0 && !s_blnSaveCollectionInProgress)
            {
                s_blnSaveCollectionInProgress = true;

                string strPath = DustUtilityPlugin.GetFullFileName(account, Account.CollectionString);

                FileManager.Write(strPath, lstCollection.ToCachedCards());

                blnRet = true;

                s_blnSaveCollectionInProgress = false;
            }
            else { }

            return blnRet;
        }
        #endregion

        #region SaveDecks
        private static bool SaveDecks(IAccount account)
        {
            bool blnRet = false;

            List<Deck> lstAllDecks = Reflection.GetDecks();

            if ((lstAllDecks?.Count > 0 && lstAllDecks?[0]?.Cards.Count > 0) && !s_blnSaveDecksInProgress)
            {
                s_blnSaveDecksInProgress = true;

                string strPath = DustUtilityPlugin.GetFullFileName(account, Account.DecksString);

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
        public static List<Card> LoadCollection(IAccount account)
        {
            List<Card> lstRet = new List<Card>();

            if (DustUtilityPlugin.IsOffline && DustUtilityPlugin.Config.OfflineMode)
            {
                if (m_lstCachedCollection == null)
                {
                    m_lstCachedCollection = InternalLoadCollection(account);
                }
                else { }

                lstRet = m_lstCachedCollection.Select(c => c.Clone()).ToList();
            }
            else
            {
                lstRet = InternalLoadCollection(account);
            }

            return lstRet;
        }
        #endregion

        #region InternalLoadCollection
        private static List<Card> InternalLoadCollection(IAccount account)
        {
            List<Card> lstRet = new List<Card>();

            try
            {
                string strPath = DustUtilityPlugin.GetFullFileName(account, Account.CollectionString);

                List<CachedCard> lstCachedCards = FileManager.Load<List<CachedCard>>(strPath);

                for (int i = 0; i < lstCachedCards.Count; i++)
                {
                    CachedCard cachedCard = lstCachedCards[i];

                    lstRet.Add(new Card(cachedCard.Id, cachedCard.Count, cachedCard.IsGolden));
                }
            }
            catch (System.Exception ex)
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while loading collection: {ex}");
            }

            return lstRet;
        }
        #endregion

        #region LoadDecks
        public static List<Deck> LoadDecks(IAccount account)
        {
            List<Deck> lstRet = new List<Deck>();

            if (DustUtilityPlugin.IsOffline && DustUtilityPlugin.Config.OfflineMode)
            {
                if (m_lstCachedDecks == null)
                {
                    m_lstCachedDecks = InternalLoadDecks(account);
                }
                else { }

                lstRet = m_lstCachedDecks.Select(d => d.Clone()).ToList();
            }
            else
            {
                lstRet = InternalLoadDecks(account);
            }

            return lstRet;
        }
        #endregion

        #region InternalLoadDecks
        private static List<Deck> InternalLoadDecks(IAccount account)
        {
            List<Deck> lstRet = new List<Deck>();

            try
            {
                string strPath = DustUtilityPlugin.GetFullFileName(account, Account.DecksString);

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
            catch (System.Exception ex)
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while loading decks: {ex}");
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

            s_timer = new Timer(OnTick, null, 0, 1000 * DustUtilityPlugin.Config.SaveInterval);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Started cache timer (Interval={DustUtilityPlugin.Config.SaveInterval}s)");
        }
        #endregion

        #region StopTimer
        public static void StopTimer()
        {
            s_timer.Dispose();
            s_timer = null;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Stopped cache timer");
        }
        #endregion

        #region ClearCache
        public static void ClearCache()
        {
            m_lstCachedCollection?.Clear();
            m_lstCachedCollection = null;

            m_lstCachedDecks?.Clear();
            m_lstCachedDecks = null;
        }
        #endregion

        #region OnTick
        private static async void OnTick(object state)
        {
            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Saving collection and decks...");

            ServiceLocator.Current.GetInstance<MainViewModel>().IsSyncing = true;

            IAccount account = await Account.GetLoggedInAccountAsync();

            if (state != null)
            {
                account = state as IAccount;
            }
            else { }

            if ((!account?.IsEmpty ?? false) && (account?.IsValid ?? false))
            {
                HistoryManager.CheckCollection(account);

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Saving collection...");

                if (SaveCollection(account))
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Info, "Saved collection successfuly");

                    DustUtilityPlugin.ShowToastNotification("Saved Collection!");
                }
                else { }

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Saving decks...");

                if (SaveDecks(account))
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Info, "Saved decks successfuly");

                    DustUtilityPlugin.ShowToastNotification("Saved Decks!");
                }
                else { }
            }
            else
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Warning, "Couldn't retrieve account!");
            }

            ServiceLocator.Current.GetInstance<MainViewModel>().IsSyncing = false;
        }
        #endregion
    }
}