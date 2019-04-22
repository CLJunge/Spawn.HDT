#region Using
using GalaSoft.MvvmLight;
using HearthMirror.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Imaging;
#endregion

namespace Spawn.HDT.DustUtility.UI.Models
{
    [DebuggerDisplay("{Name}: {CardCount} Card(s) ({CraftingCost} Dust)")]
    public class DeckItemModel : ObservableObject
    {
        #region Static Fields
        private static Dictionary<string, BitmapImage> s_dImageCache = new Dictionary<string, BitmapImage>();
        #endregion

        #region Member Variables
        private Deck m_deck;
        private int m_nMaxCardCount;
        private double m_dblOpacity;
        private bool m_blnShowToolTip;

        private bool m_blnIsWhizbangDeck;
        #endregion

        #region Properties
        #region Deck
        public Deck Deck
        {
            get => m_deck;
            set => Set(ref m_deck, value);
        }
        #endregion

        #region DeckId
        public long DeckId => (Deck?.Id ?? 0);
        #endregion

        #region Name
        public string Name => Deck?.Name;
        #endregion

        #region CardCount
        public int CardCount => (Deck?.Cards.Sum(c => c.Count) ?? 0);
        #endregion

        #region CraftingCost
        public int CraftingCost => (Deck?.GetCraftingCost() ?? 0);
        #endregion

        #region HeroImage
        public BitmapImage HeroImage => GetHeroImage();
        #endregion

        #region MaxCardCount
        public int MaxCardCount
        {
            get => m_nMaxCardCount;
            set => Set(ref m_nMaxCardCount, value);
        }
        #endregion

        #region Opacity
        public double Opacity
        {
            get => m_dblOpacity;
            set => Set(ref m_dblOpacity, value);
        }
        #endregion

        #region ShowToolTip
        public bool ShowToolTip
        {
            get => m_blnShowToolTip;
            set => Set(ref m_blnShowToolTip, value);
        }
        #endregion
        #endregion

        #region Ctor
        public DeckItemModel()
        {
            MaxCardCount = 30;
            Opacity = 1;

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(Deck)))
                {
                    if (Deck.Cards.Count == 1)
                    {
                        Card card = Deck.Cards[0];

                        m_blnIsWhizbangDeck = card.Id == HearthDb.CardIds.Collectible.Neutral.WhizbangTheWonderful;

                        if (m_blnIsWhizbangDeck)
                            MaxCardCount = 1;
                    }

                    ShowToolTip = Deck.GetTotalCardCount() < MaxCardCount;

                    RaisePropertyChanged(nameof(DeckId));
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(CardCount));
                    RaisePropertyChanged(nameof(CraftingCost));
                    RaisePropertyChanged(nameof(HeroImage));
                    RaisePropertyChanged(nameof(MaxCardCount));
                    RaisePropertyChanged(nameof(ShowToolTip));
                }
            };
#if DEBUG
            if (ViewModelBase.IsInDesignModeStatic)
                Deck = new Deck() { Id = 4342323, Hero = "HERO_01", Name = "Test Deck", Cards = new List<Card>() { new Card("", 0, false) } };
#endif

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'DeckItemModel' instance");
        }

        public DeckItemModel(Deck deck) : this() => Deck = deck;
        #endregion

        #region GetHeroImage
        private BitmapImage GetHeroImage()
        {
            string strHero = Deck?.Hero.Substring(0, 7);

            if (m_blnIsWhizbangDeck)
                strHero = "HERO_WHIZBANG";

            if (!s_dImageCache.TryGetValue(strHero, out BitmapImage retVal))
            {
                string strSource = string.Empty;

                if (!m_blnIsWhizbangDeck)
                {
                    switch (strHero)
                    {
                        case "HERO_01": //Warrior
                            strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_garrosh.png";
                            break;

                        case "HERO_02": //Shaman
                            strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_thrall.png";
                            break;

                        case "HERO_03": //Rogue
                            strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_valeera.png";
                            break;

                        case "HERO_04": //Paladin
                            strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_uther.png";
                            break;

                        case "HERO_05": //Hunter
                            strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_rexxar.png";
                            break;

                        case "HERO_06": //Druid
                            strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_malfurion.png";
                            break;

                        case "HERO_07": //Warlock
                            strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_guldan.png";
                            break;

                        case "HERO_08": //Mage
                            strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_jaina.png";
                            break;

                        case "HERO_09": //Priest
                            strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_anduin.png";
                            break;
                    }
                }
                else
                {
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Images/hero_whizbang.png";
                }

                if (!string.IsNullOrEmpty(strSource))
                {
                    retVal = new BitmapImage(new Uri(strSource, UriKind.Relative));

                    s_dImageCache.Add(strHero, retVal);
                }
            }

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, strHero);

            return retVal;
        }
        #endregion
    }
}