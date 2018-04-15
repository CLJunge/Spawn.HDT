#region Using
using GalaSoft.MvvmLight.CommandWpf;
using Spawn.HDT.DustUtility.CardManagement;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class SearchParametersFlyoutViewModel : ViewModelBase
    {
        #region Member Variables
        private bool m_blnExpertSetEnabled;
        private bool m_blnGoblinsSetEnabled;
        private bool m_blnTournamentSetEnabled;
        private bool m_blnOldGodsSetEnabled;
        private bool m_blnGadgetzanSetEnabled;
        private bool m_blnUngoroSetEnabled;
        private bool m_blnFrozenThroneSetEnabled;
        private bool m_blnKoboldsSetEnabled;
        private bool m_blnWitchwoodSetEnabled;
        private bool m_blnNaxxSetEnabled;
        private bool m_blnMountainSetEnabled;
        private bool m_blnLeagueSetEnabled;
        private bool m_blnKarazhanSetEnabled;
        private bool m_blnHallSetEnabled;
        private bool m_blnCommonRarityEnabled;
        private bool m_blnRareRarityEnabled;
        private bool m_blnEpicRarityEnabled;
        private bool m_blnLegendaryRarityEnabled;
        private bool m_blnDruidClassEnabled;
        private bool m_blnHunterClassEnabled;
        private bool m_blnMageClassEnabled;
        private bool m_blnPaladinClassEnabled;
        private bool m_blnPriestClassEnabled;
        private bool m_blnRogueClassEnabled;
        private bool m_blnShamanClassEnabled;
        private bool m_blnWarlockClassEnabled;
        private bool m_blnWarriorClassEnabled;
        private bool m_blnNeutralClassEnabled;
        private bool m_blnIncludeGoldenCards;
        private bool m_blnIncludeGoldenCardsOnly;
        private bool m_blnIncludeUnusedCardsOnly;
        #endregion

        #region Properties
        #region ExpertSetEnabled
        public bool ExpertSetEnabled
        {
            get => m_blnExpertSetEnabled;
            set => Set(ref m_blnExpertSetEnabled, value);
        }
        #endregion

        #region GoblinsSetEnabled
        public bool GoblinsSetEnabled
        {
            get => m_blnGoblinsSetEnabled;
            set => Set(ref m_blnGoblinsSetEnabled, value);
        }
        #endregion

        #region TournamentSetEnabled
        public bool TournamentSetEnabled
        {
            get => m_blnTournamentSetEnabled;
            set => Set(ref m_blnTournamentSetEnabled, value);
        }
        #endregion

        #region OldGodsSetEnabled
        public bool OldGodsSetEnabled
        {
            get => m_blnOldGodsSetEnabled;
            set => Set(ref m_blnOldGodsSetEnabled, value);
        }
        #endregion

        #region GadgetzanSetEnabled
        public bool GadgetzanSetEnabled
        {
            get => m_blnGadgetzanSetEnabled;
            set => Set(ref m_blnGadgetzanSetEnabled, value);
        }
        #endregion

        #region UngoroSetEnabled
        public bool UngoroSetEnabled
        {
            get => m_blnUngoroSetEnabled;
            set => Set(ref m_blnUngoroSetEnabled, value);
        }
        #endregion

        #region FrozenThroneSetEnabled
        public bool FrozenThroneSetEnabled
        {
            get => m_blnFrozenThroneSetEnabled;
            set => Set(ref m_blnFrozenThroneSetEnabled, value);
        }
        #endregion

        #region KoboldsSetEnabled
        public bool KoboldsSetEnabled
        {
            get => m_blnKoboldsSetEnabled;
            set => Set(ref m_blnKoboldsSetEnabled, value);
        }
        #endregion

        #region WitchwoodSetEnabled
        public bool WitchwoodSetEnabled
        {
            get => m_blnWitchwoodSetEnabled;
            set => Set(ref m_blnWitchwoodSetEnabled, value);
        }
        #endregion

        #region NaxxSetEnabled
        public bool NaxxSetEnabled
        {
            get => m_blnNaxxSetEnabled;
            set => Set(ref m_blnNaxxSetEnabled, value);
        }
        #endregion

        #region MountainSetEnabled
        public bool MountainSetEnabled
        {
            get => m_blnMountainSetEnabled;
            set => Set(ref m_blnMountainSetEnabled, value);
        }
        #endregion

        #region LeagueSetEnabled
        public bool LeagueSetEnabled
        {
            get => m_blnLeagueSetEnabled;
            set => Set(ref m_blnLeagueSetEnabled, value);
        }
        #endregion

        #region KarazhanSetEnabled
        public bool KarazhanSetEnabled
        {
            get => m_blnKarazhanSetEnabled;
            set => Set(ref m_blnKarazhanSetEnabled, value);
        }
        #endregion

        #region HallSetEnabled
        public bool HallSetEnabled
        {
            get => m_blnHallSetEnabled;
            set => Set(ref m_blnHallSetEnabled, value);
        }
        #endregion

        #region CommonRarityEnabled
        public bool CommonRarityEnabled
        {
            get => m_blnCommonRarityEnabled;
            set => Set(ref m_blnCommonRarityEnabled, value);
        }
        #endregion

        #region RareRarityEnabled
        public bool RareRarityEnabled
        {
            get => m_blnRareRarityEnabled;
            set => Set(ref m_blnRareRarityEnabled, value);
        }
        #endregion

        #region EpicRarityEnabled
        public bool EpicRarityEnabled
        {
            get => m_blnEpicRarityEnabled;
            set => Set(ref m_blnEpicRarityEnabled, value);
        }
        #endregion

        #region LegendaryRarityEnabled
        public bool LegendaryRarityEnabled
        {
            get => m_blnLegendaryRarityEnabled;
            set => Set(ref m_blnLegendaryRarityEnabled, value);
        }
        #endregion

        #region DruidClassEnabled
        public bool DruidClassEnabled
        {
            get => m_blnDruidClassEnabled;
            set => Set(ref m_blnDruidClassEnabled, value);
        }
        #endregion

        #region HunterClassEnabled
        public bool HunterClassEnabled
        {
            get => m_blnHunterClassEnabled;
            set => Set(ref m_blnHunterClassEnabled, value);
        }
        #endregion

        #region MageClassEnabled
        public bool MageClassEnabled
        {
            get => m_blnMageClassEnabled;
            set => Set(ref m_blnMageClassEnabled, value);
        }
        #endregion

        #region PaladinClassEnabled
        public bool PaladinClassEnabled
        {
            get => m_blnPaladinClassEnabled;
            set => Set(ref m_blnPaladinClassEnabled, value);
        }
        #endregion

        #region PriestClassEnabled
        public bool PriestClassEnabled
        {
            get => m_blnPriestClassEnabled;
            set => Set(ref m_blnPriestClassEnabled, value);
        }
        #endregion

        #region RogueClassEnabled
        public bool RogueClassEnabled
        {
            get => m_blnRogueClassEnabled;
            set => Set(ref m_blnRogueClassEnabled, value);
        }
        #endregion

        #region ShamanClassEnabled
        public bool ShamanClassEnabled
        {
            get => m_blnShamanClassEnabled;
            set => Set(ref m_blnShamanClassEnabled, value);
        }
        #endregion

        #region WarlockClassEnabled
        public bool WarlockClassEnabled
        {
            get => m_blnWarlockClassEnabled;
            set => Set(ref m_blnWarlockClassEnabled, value);
        }
        #endregion

        #region WarriorClassEnabled
        public bool WarriorClassEnabled
        {
            get => m_blnWarriorClassEnabled;
            set => Set(ref m_blnWarriorClassEnabled, value);
        }
        #endregion

        #region NeutralClassEnabled
        public bool NeutralClassEnabled
        {
            get => m_blnNeutralClassEnabled;
            set => Set(ref m_blnNeutralClassEnabled, value);
        }
        #endregion

        #region IncludeGoldenCards
        public bool IncludeGoldenCards
        {
            get => m_blnIncludeGoldenCards;
            set => Set(ref m_blnIncludeGoldenCards, value);
        }
        #endregion

        #region IncludeGoldenCardsOnly
        public bool IncludeGoldenCardsOnly
        {
            get => m_blnIncludeGoldenCardsOnly;
            set => Set(ref m_blnIncludeGoldenCardsOnly, value);
        }
        #endregion

        #region IncludeUnusedCardsOnly
        public bool IncludeUnusedCardsOnly
        {
            get => m_blnIncludeUnusedCardsOnly;
            set => Set(ref m_blnIncludeUnusedCardsOnly, value);
        }
        #endregion

        #region SaveCommand
        public ICommand SaveCommand => new RelayCommand(SaveParameters);
        #endregion
        #endregion

        #region Ctor
        public SearchParametersFlyoutViewModel()
        {
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(IncludeGoldenCards)) && !IncludeGoldenCards)
                {
                    IncludeGoldenCardsOnly = false;
                }
                else { }
            };

            if (IsInDesignMode)
            {
                Initialize();
            }
            else { }
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            SearchParameters parameters = DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters;

            ExpertSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.EXPERT1);
            GoblinsSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.GVG);
            TournamentSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.TGT);
            OldGodsSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.OG);
            GadgetzanSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.GANGS);
            UngoroSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.UNGORO);
            FrozenThroneSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.ICECROWN);
            KoboldsSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.LOOTAPALOOZA);
            WitchwoodSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.GILNEAS);
            NaxxSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.NAXX);
            MountainSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.BRM);
            LeagueSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.LOE);
            KarazhanSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.KARA);
            HallSetEnabled = parameters.Sets.Contains(HearthDb.Enums.CardSet.HOF);

            CommonRarityEnabled = parameters.Rarities.Contains(HearthDb.Enums.Rarity.COMMON);
            RareRarityEnabled = parameters.Rarities.Contains(HearthDb.Enums.Rarity.RARE);
            EpicRarityEnabled = parameters.Rarities.Contains(HearthDb.Enums.Rarity.EPIC);
            LegendaryRarityEnabled = parameters.Rarities.Contains(HearthDb.Enums.Rarity.LEGENDARY);

            DruidClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.DRUID);
            HunterClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.HUNTER);
            MageClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.MAGE);
            PaladinClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.PALADIN);
            PriestClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.PRIEST);
            RogueClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.ROGUE);
            ShamanClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.SHAMAN);
            WarlockClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.WARLOCK);
            WarriorClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.WARRIOR);
            NeutralClassEnabled = parameters.Classes.Contains(HearthDb.Enums.CardClass.NEUTRAL);

            IncludeGoldenCards = parameters.IncludeGoldenCards;
            IncludeGoldenCardsOnly = parameters.GoldenCardsOnly;
            IncludeUnusedCardsOnly = parameters.UnusedCardsOnly;
        }
        #endregion

        #region SaveParameters
        private void SaveParameters()
        {
            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Clear();
            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Clear();
            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Rarities.Clear();

            if (ExpertSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.EXPERT1);
            }
            else { }

            if (GoblinsSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.GVG);
            }
            else { }

            if (TournamentSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.TGT);
            }
            else { }

            if (OldGodsSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.OG);
            }
            else { }

            if (GadgetzanSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.GANGS);
            }
            else { }

            if (UngoroSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.UNGORO);
            }
            else { }

            if (FrozenThroneSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.ICECROWN);
            }
            else { }

            if (KoboldsSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.LOOTAPALOOZA);
            }
            else { }

            if (WitchwoodSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.GILNEAS);
            }
            else { }

            if (NaxxSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.NAXX);
            }
            else { }

            if (MountainSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.BRM);
            }
            else { }

            if (LeagueSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.LOE);
            }
            else { }

            if (KarazhanSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.KARA);
            }
            else { }

            if (HallSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(HearthDb.Enums.CardSet.HOF);
            }
            else { }

            if (CommonRarityEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Rarities.Add(HearthDb.Enums.Rarity.COMMON);
            }
            else { }

            if (RareRarityEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Rarities.Add(HearthDb.Enums.Rarity.RARE);
            }
            else { }

            if (EpicRarityEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Rarities.Add(HearthDb.Enums.Rarity.EPIC);
            }
            else { }

            if (LegendaryRarityEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Rarities.Add(HearthDb.Enums.Rarity.LEGENDARY);
            }
            else { }

            if (DruidClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.DRUID);
            }
            else { }

            if (HunterClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.HUNTER);
            }
            else { }

            if (MageClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.MAGE);
            }
            else { }

            if (PaladinClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.PALADIN);
            }
            else { }

            if (PriestClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.PRIEST);
            }
            else { }

            if (RogueClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.ROGUE);
            }
            else { }

            if (ShamanClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.SHAMAN);
            }
            else { }

            if (WarlockClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.WARLOCK);
            }
            else { }

            if (WarriorClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.WARRIOR);
            }
            else { }

            if (NeutralClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(HearthDb.Enums.CardClass.NEUTRAL);
            }
            else { }

            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.IncludeGoldenCards = IncludeGoldenCards;
            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.GoldenCardsOnly = IncludeGoldenCardsOnly;
            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.UnusedCardsOnly = IncludeUnusedCardsOnly;

            DustUtilityPlugin.MainWindow.SearchParametersFlyout.IsOpen = false;
        }
        #endregion
    }
}