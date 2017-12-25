﻿using HearthDb.Enums;
using HearthMirror.Objects;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spawn.HDT.DustUtility.Search
{
    public class CardManager
    {
        #region Static Variables
        public static Regex NumericRegex = new Regex("[0-9]+");
        #endregion

        #region Member Variables
        private MetroWindow m_mainWindow;

        private Account m_account;

        private List<CardWrapper> m_lstUnusedCards;
        #endregion

        #region Ctor
        public CardManager(MetroWindow mainWindow, Account account)
        {
            m_mainWindow = mainWindow;

            m_account = account;

            m_lstUnusedCards = new List<CardWrapper>();
        }
        #endregion

        #region GetCardsAsync
        public async Task<CardWrapper[]> GetCardsAsync(Parameters parameters)
        {
            bool blnDustMode = NumericRegex.IsMatch(parameters.QueryString);

            StringBuilder sb = new StringBuilder();

            sb.Append("Collecting cards... ").Append($"(Parameters: QueryString={parameters.QueryString} ")
                .Append($"IncludeGoldenCards={parameters.IncludeGoldenCards} ")
                .Append($"UnusedCardsOnly={parameters.UnusedCardsOnly} ")
                .Append($"Rarities={string.Join(",", parameters.Rarities)} ")
                .Append($"Classes={string.Join(",", parameters.Classes)} ")
                .Append($"Sets={string.Join(",", parameters.Sets)}");

            Log.WriteLine(sb.ToString(), LogType.Debug);

            List<Card> lstCollection = m_account.LoadCollection();

            if (parameters.UnusedCardsOnly)
            {
                await CheckForUnusedCardsAsync(lstCollection);
            }
            else
            {
                m_lstUnusedCards = new List<CardWrapper>();

                for (int i = 0; i < lstCollection.Count; i++)
                {
                    m_lstUnusedCards.Add(new CardWrapper(lstCollection[i]));
                }
            }

            List<CardWrapper> lstRet = new List<CardWrapper>();

            if (lstCollection.Count > 0)
            {
                if (blnDustMode)
                {
                    GetCardsForDustAmount(parameters, lstRet);
                }
                else
                {
                    GetCardByQueryString(parameters, lstRet);
                }
            }
            else { }

            Log.WriteLine($"Found {lstRet.Count} cards", LogType.Debug);

            return lstRet.ToArray();
        }
        #endregion

        #region GetCardForDustAmount
        private void GetCardsForDustAmount(Parameters parameters, List<CardWrapper> lstCards)
        {
            int nDustAmount = 0;

            try
            {
                nDustAmount = Convert.ToInt32(parameters.QueryString);
            }
            catch
            {
                //Invalid value
                nDustAmount = Int32.MaxValue;
            }

            int nTotalAmount = 0;

            bool blnDone = false;

            for (int i = 0; i < parameters.Rarities.Count && !blnDone; i++)
            {
                List<CardWrapper> lstChunk = GetCardsForRarity(parameters.Rarities[i], parameters);

                lstChunk = FilterForClasses(lstChunk, parameters.Classes);

                lstChunk = FilterForSets(lstChunk, parameters.Sets);

                lstChunk = new List<CardWrapper>(lstChunk.OrderBy(c => c.GetDustValue()));

                for (int j = 0; j < lstChunk.Count && !blnDone; j++)
                {
                    CardWrapper cardWrapper = lstChunk[j];

                    nTotalAmount += cardWrapper.GetDustValue();

                    lstCards.Add(cardWrapper);

                    blnDone = nTotalAmount >= nDustAmount;
                }
            }

            //Post processing
            //Remove low rarity cards if the total amount is over the targeted amount
            if (nTotalAmount > nDustAmount)
            {
                RemoveRedundantCards(lstCards, parameters, nDustAmount, nTotalAmount);
            }
            else { }
        }
        #endregion

        #region RemoveRedundantCards
        private void RemoveRedundantCards(List<CardWrapper> lstCards, Parameters parameters, int nDustAmount, int nTotalAmount)
        {
            if (lstCards.Count > 0 && parameters.Rarities.Count > 0)
            {
                int nDifference = nTotalAmount - nDustAmount;

                if (nDifference > 0)
                {
                    bool blnDone = false;

                    int nCurrentAmount = 0;

                    for (int i = 0; i < parameters.Rarities.Count && !blnDone; i++)
                    {
                        List<CardWrapper> lstChunk = lstCards.FindAll(c => c.DbCard.Rarity == parameters.Rarities[i]);

                        for (int j = 0; j < lstChunk.Count && !blnDone; j++)
                        {
                            CardWrapper cardWrapper = lstChunk[j];

                            nCurrentAmount += cardWrapper.GetDustValue();

                            int nCurrentDifference = nDifference - nCurrentAmount;

                            blnDone = nCurrentDifference == 0;

                            if (!blnDone)
                            {
                                if (nCurrentDifference < 0)
                                {
                                    blnDone = true;
                                }
                                else
                                {
                                    lstCards.Remove(cardWrapper);
                                }
                            }
                            else
                            {
                                lstCards.Remove(cardWrapper);
                            }
                        }
                    }
                }
                else { }
            }
            else { }
        }
        #endregion

        #region GetCardsByQueryString
        private void GetCardByQueryString(Parameters parameters, List<CardWrapper> lstCards)
        {
            bool blnDone = false;

            string strQueryString = parameters.QueryString.ToLowerInvariant();

            for (int i = 0; i < parameters.Rarities.Count && !blnDone; i++)
            {
                List<CardWrapper> lstChunk = GetCardsForRarity(parameters.Rarities[i], parameters);

                lstChunk = FilterForClasses(lstChunk, parameters.Classes);

                lstChunk = FilterForSets(lstChunk, parameters.Sets);

                for (int j = 0; j < lstChunk.Count; j++)
                {
                    CardWrapper cardWrapper = lstChunk[j];

                    if (IsCardMatch(cardWrapper, strQueryString))
                    {
                        lstCards.Add(cardWrapper);
                    }
                    else { }
                }
            }
        }
        #endregion

        #region IsCardMatch
        private bool IsCardMatch(CardWrapper cardWrapper, string strKeyString)
        {
            bool blnRet = false;

            blnRet |= cardWrapper.DbCard.Name.ToLowerInvariant().Contains(strKeyString);

            blnRet |= cardWrapper.DbCard.Race.ToString().Equals(strKeyString.ToUpperInvariant());

            blnRet |= cardWrapper.DbCard.Mechanics.Any(s => s.ToLowerInvariant().Equals(strKeyString));

            blnRet |= CardSets.AllFullName[cardWrapper.DbCard.Set].ToLowerInvariant().Contains(strKeyString);

            blnRet |= cardWrapper.DbCard.Type.ToString().ToLowerInvariant().Equals(strKeyString) || (strKeyString.Equals("spell") && cardWrapper.DbCard.Type.ToString().ToLowerInvariant().Equals("ability"));

            return blnRet;
        }
        #endregion

        #region GetTotalDustValueForAllCards
        public int GetTotalDustValueForAllCards()
        {
            int nRet = 0;

            List<Card> lstCards = m_account.LoadCollection();

            for (int i = 0; i < lstCards.Count; i++)
            {
                Card card = lstCards[i];

                nRet += card.GetDustValue() * card.Count;
            }

            return nRet;
        }
        #endregion

        #region CheckForUnusedCardsAsync
        private async Task CheckForUnusedCardsAsync(List<Card> lstCollection)
        {
            if (lstCollection == null)
            {
                throw new ArgumentNullException("lstCollection");
            }
            else { }

            m_lstUnusedCards.Clear();

            List<Deck> lstDecks = m_account.LoadDecks();

            if (lstDecks.Count > 0 && lstDecks[0].Cards.Count > 0)
            {
                for (int i = 0; i < lstCollection.Count; i++)
                {
                    Card card = lstCollection[i];
                    CardWrapper cardWrapper = new CardWrapper(card);

                    for (int j = 0; j < lstDecks.Count; j++)
                    {
                        if (!m_account.IsDeckExcluded(lstDecks[j].Id) && lstDecks[j].ContainsCard(card.Id))
                        {
                            Card c = lstDecks[j].GetCard(card.Id);

                            if (c.Count > cardWrapper.MaxCountInDecks)
                            {
                                cardWrapper.MaxCountInDecks = c.Count;
                            }
                            else { }
                        }
                        else { }
                    }

                    if (cardWrapper.MaxCountInDecks < 2 && cardWrapper.Card.Count > cardWrapper.MaxCountInDecks && !(cardWrapper.DbCard.Rarity == Rarity.LEGENDARY && cardWrapper.MaxCountInDecks == 1))
                    {
                        m_lstUnusedCards.Add(cardWrapper);
                    }
                    else { }
                }
            }
            else if (!DustUtilityPlugin.IsOffline)
            {
                await m_mainWindow.ShowMessageAsync("No decks available", "Navigate to the \"Play\" page first!");
                //MessageBox.Show("Navigate to the \"Play\" page first!", "Dust Utility", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else { }
        }
        #endregion

        #region FilterForClasses
        private List<CardWrapper> FilterForClasses(List<CardWrapper> lstCards, List<CardClass> lstClasses)
        {
            List<CardWrapper> lstRet = new List<CardWrapper>();

            for (int i = 0; i < lstClasses.Count; i++)
            {
                List<CardWrapper> lstChunk = lstCards.FindAll(c => c.DbCard.Class == lstClasses[i]);

                lstRet.AddRange(lstChunk);
            }

            return lstRet;
        }
        #endregion

        #region FilterForSets
        private List<CardWrapper> FilterForSets(List<CardWrapper> lstCards, List<CardSet> lstSets)
        {
            List<CardWrapper> lstRet = new List<CardWrapper>();

            for (int i = 0; i < lstSets.Count; i++)
            {
                List<CardWrapper> lstChunk = lstCards.FindAll(c => c.DbCard.Set == lstSets[i]);

                lstRet.AddRange(lstChunk);
            }

            return lstRet;
        }
        #endregion

        #region GetCardsForRarity
        private List<CardWrapper> GetCardsForRarity(Rarity rarity, Parameters parameters)
        {
            List<CardWrapper> lstRet = new List<CardWrapper>();

            if (parameters.IncludeGoldenCards)
            {
                if (parameters.GoldenCardsOnly)
                {
                    lstRet = m_lstUnusedCards.FindAll(c => c.DbCard.Rarity == rarity && c.Card.Premium);
                }
                else
                {
                    lstRet = m_lstUnusedCards.FindAll(c => c.DbCard.Rarity == rarity);
                }
            }
            else
            {
                lstRet = m_lstUnusedCards.FindAll(c => c.DbCard.Rarity == rarity && !c.Card.Premium);
            }

            return lstRet;
        }
        #endregion
    }
}