using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Collections.Generic;

namespace Spawn.HDT.DustUtility.CardManagement
{
    public class SearchParameters
    {
        #region Static Ctor
        static SearchParameters()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) => typeof(SearchParameters).Assembly;
        }
        #endregion

        #region Properties
        #region QueryString
        public string QueryString { get; set; }
        #endregion

        #region IncludeGoldenCards
        public bool IncludeGoldenCards { get; set; }
        #endregion

        #region GoldenCardsOnly
        public bool GoldenCardsOnly { get; set; }
        #endregion

        #region UnusedCardsOnly
        public bool UnusedCardsOnly { get; set; }
        #endregion

        #region Rarites
        public List<Rarity> Rarities { get; set; }
        #endregion

        #region Classes
        public List<CardClass> Classes { get; set; }
        #endregion

        #region Sets
        public List<CardSet> Sets { get; set; }
        #endregion
        #endregion

        #region Ctor
        public SearchParameters()
            : this(false)
        {
        }

        public SearchParameters(bool setDefaultValues)
        {
            if (!setDefaultValues)
            {
                Rarities = new List<Rarity>();
                Classes = new List<CardClass>();
                Sets = new List<CardSet>();
            }
            else
            {
                IncludeGoldenCards = false;
                GoldenCardsOnly = false;
                UnusedCardsOnly = true;

                Rarities = new List<Rarity>
                {
                    Rarity.COMMON,
                    Rarity.RARE,
                    Rarity.EPIC,
                    Rarity.LEGENDARY
                };

                Classes = new List<CardClass>
                {
                    CardClass.DRUID,
                    CardClass.HUNTER,
                    CardClass.MAGE,
                    CardClass.PALADIN,
                    CardClass.PRIEST,
                    CardClass.ROGUE,
                    CardClass.SHAMAN,
                    CardClass.WARLOCK,
                    CardClass.WARRIOR,
                    CardClass.NEUTRAL
                };

                Sets = new List<CardSet>
                {
                    CardSet.EXPERT1,
                    CardSet.GVG,
                    CardSet.TGT,
                    CardSet.OG,
                    CardSet.GANGS,
                    CardSet.UNGORO,
                    CardSet.ICECROWN,
                    CardSet.LOOTAPALOOZA,
                    CardSet.NAXX,
                    CardSet.BRM,
                    CardSet.LOE,
                    CardSet.KARA,
                    CardSet.PROMO,
                    CardSet.HOF
                };

                Log.WriteLine("Loaded default parameters", LogType.Debug);
            }
        }
        #endregion
    }
}