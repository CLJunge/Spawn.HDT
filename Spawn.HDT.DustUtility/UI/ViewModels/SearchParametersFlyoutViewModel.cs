#region Using
using GalaSoft.MvvmLight.CommandWpf;
using HearthDb.Enums;
#if DEBUG
using Hearthstone_Deck_Tracker.Utility.Extensions;
#endif
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.Logging;
using System.Threading.Tasks;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class SearchParametersFlyoutViewModel : ViewModelBase
    {
        #region Member Variables
        private bool m_blnExpertSetEnabled;
        private string m_strExpertSetEnabledLabelText;
        private bool m_blnGoblinsSetEnabled;
        private string m_strGoblinsSetEnabledLabelText;
        private bool m_blnTournamentSetEnabled;
        private string m_strTournamentSetEnabledLabelText;
        private bool m_blnOldGodsSetEnabled;
        private string m_strOldGodsSetEnabledLabelText;
        private bool m_blnGadgetzanSetEnabled;
        private string m_strGadgetzanSetEnabledLabelText;
        private bool m_blnUngoroSetEnabled;
        private string m_strUngoroSetEnabledLabelText;
        private bool m_blnFrozenThroneSetEnabled;
        private string m_strFrozenThroneSetEnabledLabelText;
        private bool m_blnKoboldsSetEnabled;
        private string m_strKoboldsSetEnabledLabelText;
        private bool m_blnWitchwoodSetEnabled;
        private string m_strWitchwoodSetEnabledLabelText;
        private bool m_blnNaxxSetEnabled;
        private string m_strNaxxSetEnabledLabelText;
        private bool m_blnMountainSetEnabled;
        private string m_strMountainSetEnabledLabelText;
        private bool m_blnLeagueSetEnabled;
        private string m_strLeagueSetEnabledLabelText;
        private bool m_blnKarazhanSetEnabled;
        private string m_strKarazhanSetEnabledLabelText;
        private bool m_blnHallSetEnabled;
        private string m_strHallSetEnabledLabelText;
        private bool m_blnCommonRarityEnabled;
        private string m_strCommonRarityEnabledLabelText;
        private bool m_blnRareRarityEnabled;
        private string m_strRareRarityEnabledLabelText;
        private bool m_blnEpicRarityEnabled;
        private string m_strEpicRarityEnabledLabelText;
        private bool m_blnLegendaryRarityEnabled;
        private string m_strLegendaryRarityEnabledLabelText;
        private bool m_blnDruidClassEnabled;
        private string m_strDruidClassEnabledLabelText;
        private bool m_blnHunterClassEnabled;
        private string m_strHunterClassEnabledLabelText;
        private bool m_blnMageClassEnabled;
        private string m_strMageClassEnabledLabelText;
        private bool m_blnPaladinClassEnabled;
        private string m_strPaladinClassEnabledLabelText;
        private bool m_blnPriestClassEnabled;
        private string m_strPriestClassEnabledLabelText;
        private bool m_blnRogueClassEnabled;
        private string m_strRogueClassEnabledLabelText;
        private bool m_blnShamanClassEnabled;
        private string m_strShamanClassEnabledLabelText;
        private bool m_blnWarlockClassEnabled;
        private string m_strWarlockClassEnabledLabelText;
        private bool m_blnWarriorClassEnabled;
        private string m_strWarriorClassEnabledLabelText;
        private bool m_blnNeutralClassEnabled;
        private string m_strNeutralClassEnabledLabelText;
        private bool m_blnIncludeGoldenCards;
        private string m_strIncludeGoldenCardsLabelText;
        private bool m_blnIncludeGoldenCardsOnly;
        private string m_strIncludeGoldenCardsOnlyLabelText;
        private bool m_blnIncludeUnusedCardsOnly;
        private string m_strIncludeUnusedCardsOnlyLabelText;
        #endregion

        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => true;
        #endregion

        #region ExpertSetEnabled
        public bool ExpertSetEnabled
        {
            get => m_blnExpertSetEnabled;
            set => Set(ref m_blnExpertSetEnabled, value);
        }
        #endregion

        #region ExpertSetEnabledLabelText
        public string ExpertSetEnabledLabelText
        {
            get => m_strExpertSetEnabledLabelText;
            set => Set(ref m_strExpertSetEnabledLabelText, value);
        }
        #endregion

        #region GoblinsSetEnabled
        public bool GoblinsSetEnabled
        {
            get => m_blnGoblinsSetEnabled;
            set => Set(ref m_blnGoblinsSetEnabled, value);
        }
        #endregion

        #region GoblinsSetEnabledLabelText
        public string GoblinsSetEnabledLabelText
        {
            get => m_strGoblinsSetEnabledLabelText;
            set => Set(ref m_strGoblinsSetEnabledLabelText, value);
        }
        #endregion

        #region TournamentSetEnabled
        public bool TournamentSetEnabled
        {
            get => m_blnTournamentSetEnabled;
            set => Set(ref m_blnTournamentSetEnabled, value);
        }
        #endregion

        #region TournamentSetEnabledLabelText
        public string TournamentSetEnabledLabelText
        {
            get => m_strTournamentSetEnabledLabelText;
            set => Set(ref m_strTournamentSetEnabledLabelText, value);
        }
        #endregion

        #region OldGodsSetEnabled
        public bool OldGodsSetEnabled
        {
            get => m_blnOldGodsSetEnabled;
            set => Set(ref m_blnOldGodsSetEnabled, value);
        }
        #endregion

        #region OldGodsSetEnabledLabelText
        public string OldGodsSetEnabledLabelText
        {
            get => m_strOldGodsSetEnabledLabelText;
            set => Set(ref m_strOldGodsSetEnabledLabelText, value);
        }
        #endregion

        #region GadgetzanSetEnabled
        public bool GadgetzanSetEnabled
        {
            get => m_blnGadgetzanSetEnabled;
            set => Set(ref m_blnGadgetzanSetEnabled, value);
        }
        #endregion

        #region GadgetzanSetEnabledLabelText
        public string GadgetzanSetEnabledLabelText
        {
            get => m_strGadgetzanSetEnabledLabelText;
            set => Set(ref m_strGadgetzanSetEnabledLabelText, value);
        }
        #endregion

        #region UngoroSetEnabled
        public bool UngoroSetEnabled
        {
            get => m_blnUngoroSetEnabled;
            set => Set(ref m_blnUngoroSetEnabled, value);
        }
        #endregion

        #region UngoroSetEnabledLabelText
        public string UngoroSetEnabledLabelText
        {
            get => m_strUngoroSetEnabledLabelText;
            set => Set(ref m_strUngoroSetEnabledLabelText, value);
        }
        #endregion

        #region FrozenThroneSetEnabled
        public bool FrozenThroneSetEnabled
        {
            get => m_blnFrozenThroneSetEnabled;
            set => Set(ref m_blnFrozenThroneSetEnabled, value);
        }
        #endregion

        #region FrozenThroneSetEnabledLabelText
        public string FrozenThroneSetEnabledLabelText
        {
            get => m_strFrozenThroneSetEnabledLabelText;
            set => Set(ref m_strFrozenThroneSetEnabledLabelText, value);
        }
        #endregion

        #region KoboldsSetEnabled
        public bool KoboldsSetEnabled
        {
            get => m_blnKoboldsSetEnabled;
            set => Set(ref m_blnKoboldsSetEnabled, value);
        }
        #endregion

        #region KoboldsSetEnabledLabelText
        public string KoboldsSetEnabledLabelText
        {
            get => m_strKoboldsSetEnabledLabelText;
            set => Set(ref m_strKoboldsSetEnabledLabelText, value);
        }
        #endregion

        #region WitchwoodSetEnabled
        public bool WitchwoodSetEnabled
        {
            get => m_blnWitchwoodSetEnabled;
            set => Set(ref m_blnWitchwoodSetEnabled, value);
        }
        #endregion

        #region WitchwoodSetEnabledLabelText
        public string WitchwoodSetEnabledLabelText
        {
            get => m_strWitchwoodSetEnabledLabelText;
            set => Set(ref m_strWitchwoodSetEnabledLabelText, value);
        }
        #endregion

        #region NaxxSetEnabled
        public bool NaxxSetEnabled
        {
            get => m_blnNaxxSetEnabled;
            set => Set(ref m_blnNaxxSetEnabled, value);
        }
        #endregion

        #region NaxxSetEnabledLabelText
        public string NaxxSetEnabledLabelText
        {
            get => m_strNaxxSetEnabledLabelText;
            set => Set(ref m_strNaxxSetEnabledLabelText, value);
        }
        #endregion

        #region MountainSetEnabled
        public bool MountainSetEnabled
        {
            get => m_blnMountainSetEnabled;
            set => Set(ref m_blnMountainSetEnabled, value);
        }
        #endregion

        #region MountainSetEnabledLabelText
        public string MountainSetEnabledLabelText
        {
            get => m_strMountainSetEnabledLabelText;
            set => Set(ref m_strMountainSetEnabledLabelText, value);
        }
        #endregion

        #region LeagueSetEnabled
        public bool LeagueSetEnabled
        {
            get => m_blnLeagueSetEnabled;
            set => Set(ref m_blnLeagueSetEnabled, value);
        }
        #endregion

        #region LeagueSetEnabledLabelText
        public string LeagueSetEnabledLabelText
        {
            get => m_strLeagueSetEnabledLabelText;
            set => Set(ref m_strLeagueSetEnabledLabelText, value);
        }
        #endregion

        #region KarazhanSetEnabled
        public bool KarazhanSetEnabled
        {
            get => m_blnKarazhanSetEnabled;
            set => Set(ref m_blnKarazhanSetEnabled, value);
        }
        #endregion

        #region KarazhanSetEnabledLabelText
        public string KarazhanSetEnabledLabelText
        {
            get => m_strKarazhanSetEnabledLabelText;
            set => Set(ref m_strKarazhanSetEnabledLabelText, value);
        }
        #endregion

        #region HallSetEnabled
        public bool HallSetEnabled
        {
            get => m_blnHallSetEnabled;
            set => Set(ref m_blnHallSetEnabled, value);
        }
        #endregion

        #region HallSetEnabledLabelText
        public string HallSetEnabledLabelText
        {
            get => m_strHallSetEnabledLabelText;
            set => Set(ref m_strHallSetEnabledLabelText, value);
        }
        #endregion

        #region CommonRarityEnabled
        public bool CommonRarityEnabled
        {
            get => m_blnCommonRarityEnabled;
            set => Set(ref m_blnCommonRarityEnabled, value);
        }
        #endregion

        #region CommonRarityEnabledLabelText
        public string CommonRarityEnabledLabelText
        {
            get => m_strCommonRarityEnabledLabelText;
            set => Set(ref m_strCommonRarityEnabledLabelText, value);
        }
        #endregion

        #region RareRarityEnabled
        public bool RareRarityEnabled
        {
            get => m_blnRareRarityEnabled;
            set => Set(ref m_blnRareRarityEnabled, value);
        }
        #endregion

        #region RareRarityEnabledLabelText
        public string RareRarityEnabledLabelText
        {
            get => m_strRareRarityEnabledLabelText;
            set => Set(ref m_strRareRarityEnabledLabelText, value);
        }
        #endregion

        #region EpicRarityEnabled
        public bool EpicRarityEnabled
        {
            get => m_blnEpicRarityEnabled;
            set => Set(ref m_blnEpicRarityEnabled, value);
        }
        #endregion

        #region EpicRarityEnabledLabelText
        public string EpicRarityEnabledLabelText
        {
            get => m_strEpicRarityEnabledLabelText;
            set => Set(ref m_strEpicRarityEnabledLabelText, value);
        }
        #endregion

        #region LegendaryRarityEnabled
        public bool LegendaryRarityEnabled
        {
            get => m_blnLegendaryRarityEnabled;
            set => Set(ref m_blnLegendaryRarityEnabled, value);
        }
        #endregion

        #region LegendaryRarityEnabledLabelText
        public string LegendaryRarityEnabledLabelText
        {
            get => m_strLegendaryRarityEnabledLabelText;
            set => Set(ref m_strLegendaryRarityEnabledLabelText, value);
        }
        #endregion

        #region DruidClassEnabled
        public bool DruidClassEnabled
        {
            get => m_blnDruidClassEnabled;
            set => Set(ref m_blnDruidClassEnabled, value);
        }
        #endregion

        #region DruidClassEnabledLabelText
        public string DruidClassEnabledLabelText
        {
            get => m_strDruidClassEnabledLabelText;
            set => Set(ref m_strDruidClassEnabledLabelText, value);
        }
        #endregion

        #region HunterClassEnabled
        public bool HunterClassEnabled
        {
            get => m_blnHunterClassEnabled;
            set => Set(ref m_blnHunterClassEnabled, value);
        }
        #endregion

        #region HunterClassEnabledLabelText
        public string HunterClassEnabledLabelText
        {
            get => m_strHunterClassEnabledLabelText;
            set => Set(ref m_strHunterClassEnabledLabelText, value);
        }
        #endregion

        #region MageClassEnabled
        public bool MageClassEnabled
        {
            get => m_blnMageClassEnabled;
            set => Set(ref m_blnMageClassEnabled, value);
        }
        #endregion

        #region MageClassEnabledLabelText
        public string MageClassEnabledLabelText
        {
            get => m_strMageClassEnabledLabelText;
            set => Set(ref m_strMageClassEnabledLabelText, value);
        }
        #endregion

        #region PaladinClassEnabled
        public bool PaladinClassEnabled
        {
            get => m_blnPaladinClassEnabled;
            set => Set(ref m_blnPaladinClassEnabled, value);
        }
        #endregion

        #region PaladinClassEnabledLabelText
        public string PaladinClassEnabledLabelText
        {
            get => m_strPaladinClassEnabledLabelText;
            set => Set(ref m_strPaladinClassEnabledLabelText, value);
        }
        #endregion

        #region PriestClassEnabled
        public bool PriestClassEnabled
        {
            get => m_blnPriestClassEnabled;
            set => Set(ref m_blnPriestClassEnabled, value);
        }
        #endregion

        #region PriestClassEnabledLabelText
        public string PriestClassEnabledLabelText
        {
            get => m_strPriestClassEnabledLabelText;
            set => Set(ref m_strPriestClassEnabledLabelText, value);
        }
        #endregion

        #region RogueClassEnabled
        public bool RogueClassEnabled
        {
            get => m_blnRogueClassEnabled;
            set => Set(ref m_blnRogueClassEnabled, value);
        }
        #endregion

        #region RogueClassEnabledLabelText
        public string RogueClassEnabledLabelText
        {
            get => m_strRogueClassEnabledLabelText;
            set => Set(ref m_strRogueClassEnabledLabelText, value);
        }
        #endregion

        #region ShamanClassEnabled
        public bool ShamanClassEnabled
        {
            get => m_blnShamanClassEnabled;
            set => Set(ref m_blnShamanClassEnabled, value);
        }
        #endregion

        #region ShamanClassEnabledLabelText
        public string ShamanClassEnabledLabelText
        {
            get => m_strShamanClassEnabledLabelText;
            set => Set(ref m_strShamanClassEnabledLabelText, value);
        }
        #endregion

        #region WarlockClassEnabled
        public bool WarlockClassEnabled
        {
            get => m_blnWarlockClassEnabled;
            set => Set(ref m_blnWarlockClassEnabled, value);
        }
        #endregion

        #region WarlockClassEnabledLabelText
        public string WarlockClassEnabledLabelText
        {
            get => m_strWarlockClassEnabledLabelText;
            set => Set(ref m_strWarlockClassEnabledLabelText, value);
        }
        #endregion

        #region WarriorClassEnabled
        public bool WarriorClassEnabled
        {
            get => m_blnWarriorClassEnabled;
            set => Set(ref m_blnWarriorClassEnabled, value);
        }
        #endregion

        #region WarriorClassEnabledLabelText
        public string WarriorClassEnabledLabelText
        {
            get => m_strWarriorClassEnabledLabelText;
            set => Set(ref m_strWarriorClassEnabledLabelText, value);
        }
        #endregion

        #region NeutralClassEnabled
        public bool NeutralClassEnabled
        {
            get => m_blnNeutralClassEnabled;
            set => Set(ref m_blnNeutralClassEnabled, value);
        }
        #endregion

        #region NeutralClassEnabledLabelText
        public string NeutralClassEnabledLabelText
        {
            get => m_strNeutralClassEnabledLabelText;
            set => Set(ref m_strNeutralClassEnabledLabelText, value);
        }
        #endregion

        #region IncludeGoldenCards
        public bool IncludeGoldenCards
        {
            get => m_blnIncludeGoldenCards;
            set => Set(ref m_blnIncludeGoldenCards, value);
        }
        #endregion

        #region IncludeGoldenCardsLabelText
        public string IncludeGoldenCardsLabelText
        {
            get => m_strIncludeGoldenCardsLabelText;
            set => Set(ref m_strIncludeGoldenCardsLabelText, value);
        }
        #endregion

        #region IncludeGoldenCardsOnly
        public bool IncludeGoldenCardsOnly
        {
            get => m_blnIncludeGoldenCardsOnly;
            set => Set(ref m_blnIncludeGoldenCardsOnly, value);
        }
        #endregion

        #region IncludeGoldenCardsOnlyLabelText
        public string IncludeGoldenCardsOnlyLabelText
        {
            get => m_strIncludeGoldenCardsOnlyLabelText;
            set => Set(ref m_strIncludeGoldenCardsOnlyLabelText, value);
        }
        #endregion

        #region IncludeUnusedCardsOnly
        public bool IncludeUnusedCardsOnly
        {
            get => m_blnIncludeUnusedCardsOnly;
            set => Set(ref m_blnIncludeUnusedCardsOnly, value);
        }
        #endregion

        #region IncludeUnusedCardsOnlyLabelText
        public string IncludeUnusedCardsOnlyLabelText
        {
            get => m_strIncludeUnusedCardsOnlyLabelText;
            set => Set(ref m_strIncludeUnusedCardsOnlyLabelText, value);
        }
        #endregion

        #region SaveCommand
        public ICommand SaveCommand => new RelayCommand(SaveParameters, () => IsDirty);
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
            };

            NotifyDirtyStatus += OnNotifyDirtyStatus;

#if DEBUG
            if (IsInDesignMode)
            {
                InitializeAsync().Forget();
            }
#endif

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'SearchParametersFlyoutViewModel' instance");
        }
        #endregion

        #region Events
        #region OnNotifyDirtyStatus
        private void OnNotifyDirtyStatus(object sender, NotifyDirtyStatusEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ExpertSetEnabled):
                    if (e.IsDirty)
                    {
                        ExpertSetEnabledLabelText = $"{ExpertSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        ExpertSetEnabledLabelText = ExpertSetEnabledLabelText.Substring(0, ExpertSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(GoblinsSetEnabled):
                    if (e.IsDirty)
                    {
                        GoblinsSetEnabledLabelText = $"{GoblinsSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        GoblinsSetEnabledLabelText = GoblinsSetEnabledLabelText.Substring(0, GoblinsSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(TournamentSetEnabled):
                    if (e.IsDirty)
                    {
                        TournamentSetEnabledLabelText = $"{TournamentSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        TournamentSetEnabledLabelText = TournamentSetEnabledLabelText.Substring(0, TournamentSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(OldGodsSetEnabled):
                    if (e.IsDirty)
                    {
                        OldGodsSetEnabledLabelText = $"{OldGodsSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        OldGodsSetEnabledLabelText = OldGodsSetEnabledLabelText.Substring(0, OldGodsSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(GadgetzanSetEnabled):
                    if (e.IsDirty)
                    {
                        GadgetzanSetEnabledLabelText = $"{GadgetzanSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        GadgetzanSetEnabledLabelText = GadgetzanSetEnabledLabelText.Substring(0, GadgetzanSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(UngoroSetEnabled):
                    if (e.IsDirty)
                    {
                        UngoroSetEnabledLabelText = $"{UngoroSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        UngoroSetEnabledLabelText = UngoroSetEnabledLabelText.Substring(0, UngoroSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(FrozenThroneSetEnabled):
                    if (e.IsDirty)
                    {
                        FrozenThroneSetEnabledLabelText = $"{FrozenThroneSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        FrozenThroneSetEnabledLabelText = FrozenThroneSetEnabledLabelText.Substring(0, FrozenThroneSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(KoboldsSetEnabled):
                    if (e.IsDirty)
                    {
                        KoboldsSetEnabledLabelText = $"{KoboldsSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        KoboldsSetEnabledLabelText = KoboldsSetEnabledLabelText.Substring(0, KoboldsSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(WitchwoodSetEnabled):
                    if (e.IsDirty)
                    {
                        WitchwoodSetEnabledLabelText = $"{WitchwoodSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        WitchwoodSetEnabledLabelText = WitchwoodSetEnabledLabelText.Substring(0, WitchwoodSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(NaxxSetEnabled):
                    if (e.IsDirty)
                    {
                        NaxxSetEnabledLabelText = $"{NaxxSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        NaxxSetEnabledLabelText = NaxxSetEnabledLabelText.Substring(0, NaxxSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(MountainSetEnabled):
                    if (e.IsDirty)
                    {
                        MountainSetEnabledLabelText = $"{MountainSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        MountainSetEnabledLabelText = MountainSetEnabledLabelText.Substring(0, MountainSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(LeagueSetEnabled):
                    if (e.IsDirty)
                    {
                        LeagueSetEnabledLabelText = $"{LeagueSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        LeagueSetEnabledLabelText = LeagueSetEnabledLabelText.Substring(0, LeagueSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(KarazhanSetEnabled):
                    if (e.IsDirty)
                    {
                        KarazhanSetEnabledLabelText = $"{KarazhanSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        KarazhanSetEnabledLabelText = KarazhanSetEnabledLabelText.Substring(0, KarazhanSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(HallSetEnabled):
                    if (e.IsDirty)
                    {
                        HallSetEnabledLabelText = $"{HallSetEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        HallSetEnabledLabelText = HallSetEnabledLabelText.Substring(0, HallSetEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(CommonRarityEnabled):
                    if (e.IsDirty)
                    {
                        CommonRarityEnabledLabelText = $"{CommonRarityEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        CommonRarityEnabledLabelText = CommonRarityEnabledLabelText.Substring(0, CommonRarityEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(RareRarityEnabled):
                    if (e.IsDirty)
                    {
                        RareRarityEnabledLabelText = $"{RareRarityEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        RareRarityEnabledLabelText = RareRarityEnabledLabelText.Substring(0, RareRarityEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(EpicRarityEnabled):
                    if (e.IsDirty)
                    {
                        EpicRarityEnabledLabelText = $"{EpicRarityEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        EpicRarityEnabledLabelText = EpicRarityEnabledLabelText.Substring(0, EpicRarityEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(LegendaryRarityEnabled):
                    if (e.IsDirty)
                    {
                        LegendaryRarityEnabledLabelText = $"{LegendaryRarityEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        LegendaryRarityEnabledLabelText = LegendaryRarityEnabledLabelText.Substring(0, LegendaryRarityEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(DruidClassEnabled):
                    if (e.IsDirty)
                    {
                        DruidClassEnabledLabelText = $"{DruidClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        DruidClassEnabledLabelText = DruidClassEnabledLabelText.Substring(0, DruidClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(HunterClassEnabled):
                    if (e.IsDirty)
                    {
                        HunterClassEnabledLabelText = $"{HunterClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        HunterClassEnabledLabelText = HunterClassEnabledLabelText.Substring(0, HunterClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(MageClassEnabled):
                    if (e.IsDirty)
                    {
                        MageClassEnabledLabelText = $"{MageClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        MageClassEnabledLabelText = MageClassEnabledLabelText.Substring(0, MageClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(PaladinClassEnabled):
                    if (e.IsDirty)
                    {
                        PaladinClassEnabledLabelText = $"{PaladinClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        PaladinClassEnabledLabelText = PaladinClassEnabledLabelText.Substring(0, PaladinClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(PriestClassEnabled):
                    if (e.IsDirty)
                    {
                        PriestClassEnabledLabelText = $"{PriestClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        PriestClassEnabledLabelText = PriestClassEnabledLabelText.Substring(0, PriestClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(RogueClassEnabled):
                    if (e.IsDirty)
                    {
                        RogueClassEnabledLabelText = $"{RogueClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        RogueClassEnabledLabelText = RogueClassEnabledLabelText.Substring(0, RogueClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(ShamanClassEnabled):
                    if (e.IsDirty)
                    {
                        ShamanClassEnabledLabelText = $"{ShamanClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        ShamanClassEnabledLabelText = ShamanClassEnabledLabelText.Substring(0, ShamanClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(WarlockClassEnabled):
                    if (e.IsDirty)
                    {
                        WarlockClassEnabledLabelText = $"{WarlockClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        WarlockClassEnabledLabelText = WarlockClassEnabledLabelText.Substring(0, WarlockClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(WarriorClassEnabled):
                    if (e.IsDirty)
                    {
                        WarriorClassEnabledLabelText = $"{WarriorClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        WarriorClassEnabledLabelText = WarriorClassEnabledLabelText.Substring(0, WarriorClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(NeutralClassEnabled):
                    if (e.IsDirty)
                    {
                        NeutralClassEnabledLabelText = $"{NeutralClassEnabledLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        NeutralClassEnabledLabelText = NeutralClassEnabledLabelText.Substring(0, NeutralClassEnabledLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(IncludeGoldenCards):
                    if (e.IsDirty)
                    {
                        IncludeGoldenCardsLabelText = $"{IncludeGoldenCardsLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        IncludeGoldenCardsLabelText = IncludeGoldenCardsLabelText.Substring(0, IncludeGoldenCardsLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(IncludeGoldenCardsOnly):
                    if (e.IsDirty)
                    {
                        IncludeGoldenCardsOnlyLabelText = $"{IncludeGoldenCardsOnlyLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        IncludeGoldenCardsOnlyLabelText = IncludeGoldenCardsOnlyLabelText.Substring(0, IncludeGoldenCardsOnlyLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(IncludeUnusedCardsOnly):
                    if (e.IsDirty)
                    {
                        IncludeUnusedCardsOnlyLabelText = $"{IncludeUnusedCardsOnlyLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        IncludeUnusedCardsOnlyLabelText = IncludeUnusedCardsOnlyLabelText.Substring(0, IncludeUnusedCardsOnlyLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;
            }
        }
        #endregion
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LoadLabelTexts();

            SearchParameters parameters = DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters;

            ExpertSetEnabled = parameters.Sets.Contains(CardSet.EXPERT1);
            GoblinsSetEnabled = parameters.Sets.Contains(CardSet.GVG);
            TournamentSetEnabled = parameters.Sets.Contains(CardSet.TGT);
            OldGodsSetEnabled = parameters.Sets.Contains(CardSet.OG);
            GadgetzanSetEnabled = parameters.Sets.Contains(CardSet.GANGS);
            UngoroSetEnabled = parameters.Sets.Contains(CardSet.UNGORO);
            FrozenThroneSetEnabled = parameters.Sets.Contains(CardSet.ICECROWN);
            KoboldsSetEnabled = parameters.Sets.Contains(CardSet.LOOTAPALOOZA);
            WitchwoodSetEnabled = parameters.Sets.Contains(CardSet.GILNEAS);
            NaxxSetEnabled = parameters.Sets.Contains(CardSet.NAXX);
            MountainSetEnabled = parameters.Sets.Contains(CardSet.BRM);
            LeagueSetEnabled = parameters.Sets.Contains(CardSet.LOE);
            KarazhanSetEnabled = parameters.Sets.Contains(CardSet.KARA);
            HallSetEnabled = parameters.Sets.Contains(CardSet.HOF);

            CommonRarityEnabled = parameters.Rarities.Contains(Rarity.COMMON);
            RareRarityEnabled = parameters.Rarities.Contains(Rarity.RARE);
            EpicRarityEnabled = parameters.Rarities.Contains(Rarity.EPIC);
            LegendaryRarityEnabled = parameters.Rarities.Contains(Rarity.LEGENDARY);

            DruidClassEnabled = parameters.Classes.Contains(CardClass.DRUID);
            HunterClassEnabled = parameters.Classes.Contains(CardClass.HUNTER);
            MageClassEnabled = parameters.Classes.Contains(CardClass.MAGE);
            PaladinClassEnabled = parameters.Classes.Contains(CardClass.PALADIN);
            PriestClassEnabled = parameters.Classes.Contains(CardClass.PRIEST);
            RogueClassEnabled = parameters.Classes.Contains(CardClass.ROGUE);
            ShamanClassEnabled = parameters.Classes.Contains(CardClass.SHAMAN);
            WarlockClassEnabled = parameters.Classes.Contains(CardClass.WARLOCK);
            WarriorClassEnabled = parameters.Classes.Contains(CardClass.WARRIOR);
            NeutralClassEnabled = parameters.Classes.Contains(CardClass.NEUTRAL);

            IncludeGoldenCards = parameters.IncludeGoldenCards;
            IncludeGoldenCardsOnly = parameters.GoldenCardsOnly;
            IncludeUnusedCardsOnly = parameters.UnusedCardsOnly;

            SetInitialPropertyValue(nameof(ExpertSetEnabled), ExpertSetEnabled);
            SetInitialPropertyValue(nameof(GoblinsSetEnabled), GoblinsSetEnabled);
            SetInitialPropertyValue(nameof(TournamentSetEnabled), TournamentSetEnabled);
            SetInitialPropertyValue(nameof(OldGodsSetEnabled), OldGodsSetEnabled);
            SetInitialPropertyValue(nameof(GadgetzanSetEnabled), GadgetzanSetEnabled);
            SetInitialPropertyValue(nameof(UngoroSetEnabled), UngoroSetEnabled);
            SetInitialPropertyValue(nameof(FrozenThroneSetEnabled), FrozenThroneSetEnabled);
            SetInitialPropertyValue(nameof(KoboldsSetEnabled), KoboldsSetEnabled);
            SetInitialPropertyValue(nameof(WitchwoodSetEnabled), WitchwoodSetEnabled);
            SetInitialPropertyValue(nameof(NaxxSetEnabled), NaxxSetEnabled);
            SetInitialPropertyValue(nameof(MountainSetEnabled), MountainSetEnabled);
            SetInitialPropertyValue(nameof(LeagueSetEnabled), LeagueSetEnabled);
            SetInitialPropertyValue(nameof(KarazhanSetEnabled), KarazhanSetEnabled);
            SetInitialPropertyValue(nameof(HallSetEnabled), HallSetEnabled);
            SetInitialPropertyValue(nameof(CommonRarityEnabled), CommonRarityEnabled);
            SetInitialPropertyValue(nameof(RareRarityEnabled), RareRarityEnabled);
            SetInitialPropertyValue(nameof(EpicRarityEnabled), EpicRarityEnabled);
            SetInitialPropertyValue(nameof(LegendaryRarityEnabled), LegendaryRarityEnabled);
            SetInitialPropertyValue(nameof(DruidClassEnabled), DruidClassEnabled);
            SetInitialPropertyValue(nameof(HunterClassEnabled), HunterClassEnabled);
            SetInitialPropertyValue(nameof(MageClassEnabled), MageClassEnabled);
            SetInitialPropertyValue(nameof(PaladinClassEnabled), PaladinClassEnabled);
            SetInitialPropertyValue(nameof(PriestClassEnabled), PriestClassEnabled);
            SetInitialPropertyValue(nameof(RogueClassEnabled), RogueClassEnabled);
            SetInitialPropertyValue(nameof(ShamanClassEnabled), ShamanClassEnabled);
            SetInitialPropertyValue(nameof(WarlockClassEnabled), WarlockClassEnabled);
            SetInitialPropertyValue(nameof(WarriorClassEnabled), WarriorClassEnabled);
            SetInitialPropertyValue(nameof(NeutralClassEnabled), NeutralClassEnabled);
            SetInitialPropertyValue(nameof(IncludeGoldenCards), IncludeGoldenCards);
            SetInitialPropertyValue(nameof(IncludeGoldenCardsOnly), IncludeGoldenCardsOnly);
            SetInitialPropertyValue(nameof(IncludeUnusedCardsOnly), IncludeUnusedCardsOnly);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Finished initializing");
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
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.EXPERT1);
            }

            if (GoblinsSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.GVG);
            }

            if (TournamentSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.TGT);
            }

            if (OldGodsSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.OG);
            }

            if (GadgetzanSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.GANGS);
            }

            if (UngoroSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.UNGORO);
            }

            if (FrozenThroneSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.ICECROWN);
            }

            if (KoboldsSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.LOOTAPALOOZA);
            }

            if (WitchwoodSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.GILNEAS);
            }

            if (NaxxSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.NAXX);
            }

            if (MountainSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.BRM);
            }

            if (LeagueSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.LOE);
            }

            if (KarazhanSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.KARA);
            }

            if (HallSetEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Sets.Add(CardSet.HOF);
            }

            if (CommonRarityEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Rarities.Add(Rarity.COMMON);
            }

            if (RareRarityEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Rarities.Add(Rarity.RARE);
            }

            if (EpicRarityEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Rarities.Add(Rarity.EPIC);
            }

            if (LegendaryRarityEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Rarities.Add(Rarity.LEGENDARY);
            }

            if (DruidClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.DRUID);
            }

            if (HunterClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.HUNTER);
            }

            if (MageClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.MAGE);
            }

            if (PaladinClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.PALADIN);
            }

            if (PriestClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.PRIEST);
            }

            if (RogueClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.ROGUE);
            }

            if (ShamanClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.SHAMAN);
            }

            if (WarlockClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.WARLOCK);
            }

            if (WarriorClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.WARRIOR);
            }

            if (NeutralClassEnabled)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.Classes.Add(CardClass.NEUTRAL);
            }

            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.IncludeGoldenCards = IncludeGoldenCards;
            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.GoldenCardsOnly = IncludeGoldenCardsOnly;
            DustUtilityPlugin.CurrentAccount.Preferences.SearchParameters.UnusedCardsOnly = IncludeUnusedCardsOnly;

            DustUtilityPlugin.MainWindow.SearchParametersFlyout.IsOpen = false;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Saved search parameters");
        }
        #endregion

        #region LoadLabelTexts
        private void LoadLabelTexts()
        {
            ExpertSetEnabledLabelText = CardSet.EXPERT1.GetDisplayString();
            GoblinsSetEnabledLabelText = CardSet.GVG.GetDisplayString();
            TournamentSetEnabledLabelText = CardSet.TGT.GetDisplayString();
            OldGodsSetEnabledLabelText = CardSet.OG.GetDisplayString();
            GadgetzanSetEnabledLabelText = CardSet.GANGS.GetDisplayString();
            UngoroSetEnabledLabelText = CardSet.UNGORO.GetDisplayString();
            FrozenThroneSetEnabledLabelText = CardSet.ICECROWN.GetDisplayString();
            KoboldsSetEnabledLabelText = CardSet.LOOTAPALOOZA.GetDisplayString();
            WitchwoodSetEnabledLabelText = CardSet.GILNEAS.GetDisplayString();
            NaxxSetEnabledLabelText = CardSet.NAXX.GetDisplayString();
            MountainSetEnabledLabelText = CardSet.BRM.GetDisplayString();
            LeagueSetEnabledLabelText = CardSet.LOE.GetDisplayString();
            KarazhanSetEnabledLabelText = CardSet.KARA.GetDisplayString();
            HallSetEnabledLabelText = CardSet.HOF.GetDisplayString();
            CommonRarityEnabledLabelText = Rarity.COMMON.GetString();
            RareRarityEnabledLabelText = Rarity.RARE.GetString();
            EpicRarityEnabledLabelText = Rarity.EPIC.GetString();
            LegendaryRarityEnabledLabelText = Rarity.LEGENDARY.GetString();
            DruidClassEnabledLabelText = CardClass.DRUID.GetString();
            HunterClassEnabledLabelText = CardClass.HUNTER.GetString();
            MageClassEnabledLabelText = CardClass.MAGE.GetString();
            PaladinClassEnabledLabelText = CardClass.PALADIN.GetString();
            PriestClassEnabledLabelText = CardClass.PRIEST.GetString();
            RogueClassEnabledLabelText = CardClass.ROGUE.GetString();
            ShamanClassEnabledLabelText = CardClass.SHAMAN.GetString();
            WarlockClassEnabledLabelText = CardClass.WARLOCK.GetString();
            WarriorClassEnabledLabelText = CardClass.WARRIOR.GetString();
            NeutralClassEnabledLabelText = CardClass.NEUTRAL.GetString();
            IncludeGoldenCardsLabelText = "Incl. Golden Cards";
            IncludeGoldenCardsOnlyLabelText = "Golden Cards Only";
            IncludeUnusedCardsOnlyLabelText = "Unused Cards Only";

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Loaded label texts");
        }
        #endregion
    }
}