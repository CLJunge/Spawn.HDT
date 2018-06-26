#region Using
using GalaSoft.MvvmLight;
using HearthDb.Enums;
using Spawn.HDT.DustUtility.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement
{
    public class SearchParameters : ObservableObject
    {
        #region Member Variables
        private string m_strQueryString;
        private bool m_blnIncludeGoldenCards;
        private bool m_blnGoldenCardsOnly;
        private bool m_blnUnusedCardsOnly;
        #endregion

        #region Properties
        #region QueryString
        [DefaultValue("")]
        public string QueryString
        {
            get => m_strQueryString;
            set => Set(ref m_strQueryString, value);
        }
        #endregion

        #region IncludeGoldenCards
        [DefaultValue(false)]
        public bool IncludeGoldenCards
        {
            get => m_blnIncludeGoldenCards;
            set => Set(ref m_blnIncludeGoldenCards, value);
        }
        #endregion

        #region GoldenCardsOnly
        [DefaultValue(false)]
        public bool GoldenCardsOnly
        {
            get => m_blnGoldenCardsOnly;
            set => Set(ref m_blnGoldenCardsOnly, value);
        }
        #endregion

        #region UnusedCardsOnly
        [DefaultValue(true)]
        public bool UnusedCardsOnly
        {
            get => m_blnUnusedCardsOnly;
            set => Set(ref m_blnUnusedCardsOnly, value);
        }
        #endregion

        #region Rarities
        [DefaultValue(typeof(ObservableCollection<Rarity>))]
        public ObservableCollection<Rarity> Rarities { get; set; }
        #endregion

        #region Classes
        [DefaultValue(typeof(ObservableCollection<CardClass>))]
        public ObservableCollection<CardClass> Classes { get; set; }
        #endregion

        #region Sets
        [DefaultValue(typeof(ObservableCollection<CardSet>))]
        public ObservableCollection<CardSet> Sets { get; set; }
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
                Rarities = new ObservableCollection<Rarity>();
                Classes = new ObservableCollection<CardClass>();
                Sets = new ObservableCollection<CardSet>();
            }
            else
            {
                QueryString = string.Empty;
                IncludeGoldenCards = true;
                GoldenCardsOnly = false;
                UnusedCardsOnly = false;

                Rarities = new ObservableCollection<Rarity>
                {
                    Rarity.COMMON,
                    Rarity.RARE,
                    Rarity.EPIC,
                    Rarity.LEGENDARY
                };

                Classes = new ObservableCollection<CardClass>
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

                Sets = new ObservableCollection<CardSet>
                {
                    CardSet.EXPERT1,
                    CardSet.GVG,
                    CardSet.TGT,
                    CardSet.OG,
                    CardSet.GANGS,
                    CardSet.UNGORO,
                    CardSet.ICECROWN,
                    CardSet.LOOTAPALOOZA,
                    CardSet.GILNEAS,
                    CardSet.NAXX,
                    CardSet.BRM,
                    CardSet.LOE,
                    CardSet.KARA,
                    CardSet.PROMO,
                    CardSet.HOF
                };

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Loaded default parameters");
            }

            Rarities.CollectionChanged += (s, e) => RaisePropertyChanged(nameof(Rarities));
            Classes.CollectionChanged += (s, e) => RaisePropertyChanged(nameof(Classes));
            Sets.CollectionChanged += (s, e) => RaisePropertyChanged(nameof(Sets));

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Initialized new 'SearchParameters' instance");
        }
        #endregion
    }
}