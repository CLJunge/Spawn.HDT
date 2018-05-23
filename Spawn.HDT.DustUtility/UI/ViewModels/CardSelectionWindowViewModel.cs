#region Using
using CommonServiceLocator;
using GalaSoft.MvvmLight.CommandWpf;
using HearthDb.Enums;
using HearthMirror;
using MahApps.Metro.Controls.Dialogs;
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.UI.Controls;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class CardSelectionWindowViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
        private Visibility m_disenchantButtonVisibility;

        private bool m_blnDisenchantingConfirmation;

        private CardItemModel m_currentItem;
        private MetroDialogSettings m_dialogSettings;
        private CustomDialog m_cardCountDialog;
        #endregion

        #region Properties
        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
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

        #region ClearCommand
        public ICommand ClearCommand => new RelayCommand(Clear, () => CardItems.Count > 0);
        #endregion

        #region ImportLatestPackCommand
        public ICommand ImportLatestPackCommand => new RelayCommand(ImportLatestPack, () => Reflection.GetPackCards()?.Count > 0);
        #endregion

        #region DisenchantSelectionCommand
        public ICommand DisenchantSelectionCommand => new RelayCommand(DisenchantSelection, () =>
        {
            return CardItems.Count > 0 && !DustUtilityPlugin.IsOffline;
        });
        #endregion
        #endregion

        #region Ctor
        public CardSelectionWindowViewModel()
        {
            WindowTitle = "Dust Utility - Selection";

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
            await Task.Delay(0);

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
            else { }

            m_blnDisenchantingConfirmation = false;
        }
        #endregion

        #region Clear
        private void Clear()
        {
            CardItems.Clear();
            CardsInfo.Clear();
        }
        #endregion

        #region ImportLatestPack
        private void ImportLatestPack()
        {
            List<HearthMirror.Objects.Card> lstCards = Reflection.GetPackCards();

            if (lstCards?.Count > 0)
            {
                for (int i = 0; i < lstCards.Count; i++)
                {
                    AddOrUpdateCardItem(new CardItemModel(new CardWrapper(lstCards[i])));
                }
            }
            else { }
        }
        #endregion

        #region DisenchantSelection
        private async void DisenchantSelection()
        {
            if (!m_blnDisenchantingConfirmation)
            {
                MessageDialogResult result = await ServiceLocator.Current.GetInstance<MainViewModel>()
                        .SelectionWindow.ShowMessageAsync(string.Empty, "Go to your collection and leave the collection screen open. Click 'Disenchant' and do not move your mouse or type until done.", MessageDialogStyle.AffirmativeAndNegative);

                m_blnDisenchantingConfirmation = result == MessageDialogResult.Affirmative;
            }
            else
            {
                List<CardWrapper> lstCards = CardItems.Select(m => m.Wrapper).ToList();

                if (await CardsManager.Disenchant(DustUtilityPlugin.CurrentAccount, lstCards))
                {
                    Clear();

                    if (DustUtilityPlugin.Config.OfflineMode)
                    {
                        Cache.ForceSave(DustUtilityPlugin.CurrentAccount);
                    }
                    else { }

                    ServiceLocator.Current.GetInstance<MainViewModel>().SearchCommand.Execute(null);
                }
                else { }
            }
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
            else { }
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
                else { }

                if (nCount > 1 && m_cardCountDialog != null)
                {
                    (m_cardCountDialog.Content as CardCountDialog).Initialize(e.Item.Name, nCount);

                    await ServiceLocator.Current.GetInstance<MainViewModel>()
                        .SelectionWindow.ShowMetroDialogAsync(m_cardCountDialog, m_dialogSettings);
                }
                else if (nCount == 1)
                {
                    m_currentItem.Count = nCount;
                    m_currentItem.Dust = m_currentItem.Wrapper.GetDustValue(nCount);

                    AddOrUpdateCardItem(m_currentItem);

                    m_currentItem = null;
                }
                else { }
            }
            else { }
        }
        #endregion

        #region OnClosing
        public void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<CachedCard> lstCards = CardItems.Select(i => new CachedCard() { Id = i.Id, Count = i.Count, IsGolden = i.Golden }).ToList();

            DustUtilityPlugin.CurrentAccount.Preferences.CardSelection.Clear();

            for (int i = 0; i < lstCards.Count; i++)
            {
                DustUtilityPlugin.CurrentAccount.Preferences.CardSelection.Add(lstCards[i]);
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
                await ServiceLocator.Current.GetInstance<MainViewModel>()
                .SelectionWindow.HideMetroDialogAsync(m_cardCountDialog, m_dialogSettings);
            };

            retVal.AcceptButton.Click += async (s, e) =>
            {
                if (m_currentItem != null && m_cardCountDialog.Content is CardCountDialog countControl)
                {
                    if (countControl.NumericUpDownCtrl.Value.HasValue
                    && countControl.NumericUpDownCtrl.Value > 0)
                    {
                        int nNewCount = (int)countControl.NumericUpDownCtrl.Value;

                        m_currentItem.Count = nNewCount;
                        m_currentItem.Dust = m_currentItem.Wrapper.GetDustValue(nNewCount);

                        AddOrUpdateCardItem(m_currentItem);
                    }
                    else { }
                }
                else { }

                await ServiceLocator.Current.GetInstance<MainViewModel>()
                .SelectionWindow.HideMetroDialogAsync(m_cardCountDialog, m_dialogSettings);

                m_currentItem = null;
            };

            return retVal;
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
            else { }

            return blnRet;
        }
        #endregion
    }
}