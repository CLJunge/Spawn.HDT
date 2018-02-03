﻿#region Using
using GalaSoft.MvvmLight.CommandWpf;
using HearthDb.Enums;
using HearthMirror;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.ServiceLocation;
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.Services;
using Spawn.HDT.DustUtility.UI.Controls;
using Spawn.HDT.DustUtility.UI.Models;
using Spawn.HDT.DustUtility.UI.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class CardSelectionWindowViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
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

        #region ClearCommand
        public ICommand ClearCommand => new RelayCommand(Clear, () => CardItems.Count > 0);
        #endregion

        #region ImportLatestPackCommand
        public ICommand ImportLatestPackCommand => new RelayCommand(ImportLatestPack, () => Reflection.GetPackCards()?.Count > 0);
        #endregion
        #endregion

        #region Ctor
        public CardSelectionWindowViewModel()
        {
            WindowTitle = "Dust Utility - Selection";

            CardItems = new ObservableCollection<CardItemModel>();

            CardsInfo = new CardsInfoModel();

            m_dialogSettings = new MetroDialogSettings()
            {
                AnimateShow = true,
                AnimateHide = true,
                ColorScheme = MetroDialogColorScheme.Accented
            };

            CardCountDialog dialogControl = new CardCountDialog();

            dialogControl.CancelButton.Click += (s, e) =>
            {
                ServiceLocator.Current.GetInstance<IWindowService>()
                .GetInstance<CardSelectionWindow>(DustUtilityPlugin.CardSelectionWindowKey)
                .HideMetroDialogAsync(m_cardCountDialog, m_dialogSettings);
            };

            dialogControl.AcceptButton.Click += (s, e) =>
            {
                if (m_currentItem != null && m_cardCountDialog.Content is CardCountDialog countControl)
                {
                    if (countControl.NumericUpDownCtrl.Value.HasValue
                    && countControl.NumericUpDownCtrl.Value > 0)
                    {
                        int nNewCount = (int)countControl.NumericUpDownCtrl.Value;

                        m_currentItem.Count = nNewCount;
                        m_currentItem.Dust = m_currentItem.Wrapper.GetDustValue(nNewCount);

                        AddCardItem(m_currentItem);
                    }
                    else { }
                }
                else { }

                ServiceLocator.Current.GetInstance<IWindowService>()
                .GetInstance<CardSelectionWindow>(DustUtilityPlugin.CardSelectionWindowKey)
                .HideMetroDialogAsync(m_cardCountDialog, m_dialogSettings);

                m_currentItem = null;
            };

            m_cardCountDialog = new CustomDialog(DustUtilityPlugin.MainWindow)
            {
                Content = dialogControl
            };
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            Clear();

            if (DustUtilityPlugin.CurrentAccount.Preferences.CardSelection.Count > 0)
            {
                List<CardWrapper> lstWrappers = DustUtilityPlugin.CurrentAccount.Preferences.CardSelection
                    .Select(c => new CardWrapper(c.Id, c.Count, c.IsGolden, null)).ToList();

                SearchResult result = SearchResult.Create(lstWrappers);

                result.CopyToCardsInfoModel(CardsInfo);

                for (int i = 0; i < result.CardItems.Count; i++)
                {
                    CardItems.Add(result.CardItems[i]);
                }
            }
            else { }
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
                    AddCardItem(new CardItemModel(new CardWrapper(lstCards[i])));
                }
            }
            else { }
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
            if (m_cardCountDialog != null)
            {
                m_currentItem = e.Item;

                if (m_currentItem.Count > 1)
                {
                    (m_cardCountDialog.Content as CardCountDialog).Initialize(e.Item.Name, e.Item.Count);

                    await ServiceLocator.Current.GetInstance<IWindowService>()
                        .GetInstance<CardSelectionWindow>(DustUtilityPlugin.CardSelectionWindowKey)
                        .ShowMetroDialogAsync(m_cardCountDialog, m_dialogSettings);
                }
                else if (m_currentItem.Count == 1)
                {
                    AddCardItem(m_currentItem);

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

            ServiceLocator.Current.GetInstance<IWindowService>().Dispose(DustUtilityPlugin.CardSelectionWindowKey);
        }
        #endregion

        #region AddCardItem
        private void AddCardItem(CardItemModel cardItem)
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

            CardItems.Add(cardItem);
        }
        #endregion
    }
}