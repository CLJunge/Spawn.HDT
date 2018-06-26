#region Using
using CommonServiceLocator;
using HearthDb.Enums;
using HearthMirror;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.UI.Controls;
using Spawn.HDT.DustUtility.UI.Models;
using Spawn.HDT.DustUtility.UI.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.UI
{
    public class CardSelectionManager : ViewModelBase
    {
        #region Member Variables
        private Visibility m_disenchantButtonVisibility;

        private bool m_blnDisenchantingConfirmation;

        private CardItemModel m_currentItem;
        private MetroDialogSettings m_dialogSettings;
        private readonly CustomDialog m_cardCountDialog;
        #endregion

        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => false;
        #endregion

        #region CardItems
        public ObservableCollection<CardItemModel> CardItems { get; set; }
        #endregion

        #region CardsInfo
        public CardsInfoModel CardsInfo { get; set; }
        #endregion

        #region DisenchantButtonVisibility
        public Visibility DisenchantButtonVisibility
        {
            get => m_disenchantButtonVisibility;
            set => Set(ref m_disenchantButtonVisibility, value);
        }
        #endregion
        #endregion

        #region Ctor
        public CardSelectionManager()
        {
            CardItems = new ObservableCollection<CardItemModel>();

            CardsInfo = new CardsInfoModel();

            m_cardCountDialog = new CustomDialog(DustUtilityPlugin.MainWindow)
            {
                Content = CreateCardCountDialogContent()
            };
        }
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await Task.Delay(1);

            Clear();

            DisenchantButtonVisibility = (DustUtilityPlugin.Config.AutoDisenchanting ? Visibility.Visible : Visibility.Hidden);

            if (DustUtilityPlugin.CurrentAccount.Preferences.CardSelection.Count > 0)
            {
                List<CardWrapper> lstWrappers = DustUtilityPlugin.CurrentAccount.Preferences.CardSelection
                    .Select(c => new CardWrapper(c)).ToList();

                SearchResult result = SearchResult.Create(lstWrappers);

                result.CopyToCardsInfoModel(CardsInfo);

                for (int i = 0; i < result.CardItems.Count; i++)
                {
                    CardItems.Add(result.CardItems[i]);
                }
            }

            m_blnDisenchantingConfirmation = false;
        }
        #endregion

        #region Clear
        public void Clear()
        {
            CardItems.Clear();
            CardsInfo.Clear();
        }
        #endregion

        #region ImportLatestPack
        public void ImportLatestPack()
        {
            List<HearthMirror.Objects.Card> lstCards = Reflection.GetPackCards();

            if (lstCards?.Count > 0)
            {
                for (int i = 0; i < lstCards.Count; i++)
                {
                    AddOrUpdateCardItem(new CardItemModel(new CardWrapper(lstCards[i])));
                }
            }
        }
        #endregion

        #region DisenchantSelection
#pragma warning disable S3168 // "async" methods should not return "void"
        public async void DisenchantSelection()
#pragma warning restore S3168 // "async" methods should not return "void"
        {
            if (!m_blnDisenchantingConfirmation)
            {
                MessageDialogResult result = await GetCurrentWindow().ShowMessageAsync(string.Empty, "Open your collection and leave the collection screen open. Click 'Disenchant' and do not move your mouse or type until done.", MessageDialogStyle.AffirmativeAndNegative);

                m_blnDisenchantingConfirmation = result == MessageDialogResult.Affirmative;
            }
            else
            {
                List<CardWrapper> lstCards = CardItems.Select(m => m.Wrapper).ToList();

                if (await CardsManager.AutoDisenchant(DustUtilityPlugin.CurrentAccount, lstCards))
                {
                    Clear();

                    if (DustUtilityPlugin.Config.OfflineMode)
                    {
                        Cache.SaveCollection(DustUtilityPlugin.CurrentAccount);
                    }

                    ServiceLocator.Current.GetInstance<MainViewModel>().SearchCommand.Execute(null);
                }
            }
        }
        #endregion

        #region AddOrUpdateCardItem
        private void AddOrUpdateCardItem(CardItemModel cardItem)
        {
            switch (cardItem.Rarity)
            {
                case Rarity.COMMON:
                    CardsInfo.CommonsCount += cardItem.Count;
                    break;

                case Rarity.RARE:
                    CardsInfo.RaresCount += cardItem.Count;
                    break;

                case Rarity.EPIC:
                    CardsInfo.EpicsCount += cardItem.Count;
                    break;

                case Rarity.LEGENDARY:
                    CardsInfo.LegendariesCount += cardItem.Count;
                    break;
            }

            CardsInfo.DustAmount += cardItem.Dust;

            CardItemModel item = CardItems.FirstOrDefault(i => i.Id.Equals(cardItem.Id) && i.Golden == cardItem.Golden);

            if (item != null)
            {
                item.Count += cardItem.Count;
                item.Dust = item.Wrapper.GetDustValue(item.Count);
            }
            else
            {
                CardItems.Add(cardItem);
            }
        }
        #endregion

        #region CreateCardCountDialogContent
        private CardCountDialog CreateCardCountDialogContent()
        {
            CardCountDialog retVal = new CardCountDialog();

            m_dialogSettings = new MetroDialogSettings()
            {
                AnimateShow = true,
                AnimateHide = true,
                ColorScheme = MetroDialogColorScheme.Accented
            };

            retVal.CancelButton.Click += async (s, e) =>
            {
                await GetCurrentWindow().HideMetroDialogAsync(m_cardCountDialog, m_dialogSettings);
            };

            retVal.AcceptButton.Click += async (s, e) =>
            {
                if (m_currentItem != null && m_cardCountDialog.Content is CardCountDialog countControl
                && (countControl.NumericUpDownCtrl.Value.HasValue
                && countControl.NumericUpDownCtrl.Value > 0))
                {
                    int nNewCount = (int)countControl.NumericUpDownCtrl.Value;

                    m_currentItem.Count = nNewCount;
                    m_currentItem.Dust = m_currentItem.Wrapper.GetDustValue(nNewCount);

                    AddOrUpdateCardItem(m_currentItem);
                }

                await GetCurrentWindow().HideMetroDialogAsync(m_cardCountDialog, m_dialogSettings);

                m_currentItem = null;
            };

            return retVal;
        }
        #endregion

        #region OnRemoveCardItem
        public void OnRemoveCardItem(CardItemEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                CardItems.RemoveAt(e.RowIndex);

                switch (e.Item.Rarity)
                {
                    case Rarity.COMMON:
                        CardsInfo.CommonsCount -= e.Item.Count;
                        break;

                    case Rarity.RARE:
                        CardsInfo.RaresCount -= e.Item.Count;
                        break;

                    case Rarity.EPIC:
                        CardsInfo.EpicsCount -= e.Item.Count;
                        break;

                    case Rarity.LEGENDARY:
                        CardsInfo.LegendariesCount -= e.Item.Count;
                        break;
                }

                CardsInfo.DustAmount -= e.Item.Dust;
            }
        }
        #endregion

        #region OnItemDropped
        public async Task OnItemDropped(CardItemEventArgs e)
        {
            m_currentItem = e.Item;

            if (IsCardSelectable(m_currentItem))
            {
                int nCount = m_currentItem.Count;

                CardItemModel cardItem = CardItems.FirstOrDefault(i => i.Id.Equals(m_currentItem.Id) && i.Golden == m_currentItem.Golden);

                if (cardItem != null)
                {
                    nCount = nCount - cardItem.Count;
                }

                if (nCount > 1 && m_cardCountDialog != null)
                {
                    (m_cardCountDialog.Content as CardCountDialog).Initialize(e.Item.Name, nCount);

                    await GetCurrentWindow().ShowMetroDialogAsync(m_cardCountDialog, m_dialogSettings);
                }
                else if (nCount == 1)
                {
                    m_currentItem.Count = nCount;
                    m_currentItem.Dust = m_currentItem.Wrapper.GetDustValue(nCount);

                    AddOrUpdateCardItem(m_currentItem);

                    m_currentItem = null;
                }
            }
        }
        #endregion

        #region OnClosing
        public void OnClosing(object sender, CancelEventArgs e)
        {
            List<CachedCard> lstCards = CardItems.Select(i => new CachedCard() { Id = i.Id, Count = i.Count, IsGolden = i.Golden }).ToList();

            DustUtilityPlugin.CurrentAccount.Preferences.CardSelection.Clear();

            for (int i = 0; i < lstCards.Count; i++)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.CardSelection.Add(lstCards[i]);
            }
        }
        #endregion

        #region IsCardSelectable
        public bool IsCardSelectable(CardItemModel cardItem)
        {
            bool blnRet = false;

            if (cardItem != null)
            {
                CardItemModel temp = CardItems.FirstOrDefault(i => i.Id.Equals(cardItem.Id) && i.Golden == cardItem.Golden);

                if (temp != null)
                {
                    blnRet = temp.Count < cardItem.Count;
                }
                else
                {
                    blnRet = true;
                }
            }

            return blnRet;
        }
        #endregion

        #region GetCurrentWindow
        private MetroWindow GetCurrentWindow()
        {
            MetroWindow retVal = null;

            switch (DustUtilityPlugin.Config.ViewMode)
            {
                case ViewMode.Default:
                    retVal = ServiceLocator.Current.GetInstance<MainViewModel>().SelectionWindow;
                    break;

                case ViewMode.Split:
                    retVal = DustUtilityPlugin.MainWindow;
                    break;
            }

            return retVal;
        }
        #endregion
    }
}