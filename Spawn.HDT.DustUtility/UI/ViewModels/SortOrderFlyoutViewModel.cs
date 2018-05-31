#region Using
using CommonServiceLocator;
using GalaSoft.MvvmLight.CommandWpf;
using Spawn.HDT.DustUtility.UI.Dialogs;
using Spawn.HDT.DustUtility.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class SortOrderFlyoutViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strSortOrderString;
        private SortOrderItemModel m_selectedSortOrderItem;
        private int m_nSelectedSortOrderItemIndex;
        private int m_nMaxCount;
        #endregion

        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => true;
        #endregion

        #region SortOrderString
        public string SortOrderString
        {
            get => m_strSortOrderString;
            set => Set(ref m_strSortOrderString, value);
        }
        #endregion

        #region SortOrderItems
        public ObservableCollection<SortOrderItemModel> SortOrderItems { get; set; }
        #endregion

        #region SelectedSortOrderItem
        public SortOrderItemModel SelectedSortOrderItem
        {
            get => m_selectedSortOrderItem;
            set => Set(ref m_selectedSortOrderItem, value);
        }
        #endregion

        #region SelectedSortOrderItemIndex
        public int SelectedSortOrderItemIndex
        {
            get => m_nSelectedSortOrderItemIndex;
            set => Set(ref m_nSelectedSortOrderItemIndex, value);
        }
        #endregion

        #region AddItemCommand
        public ICommand AddItemCommand => new RelayCommand(AddItem, () => SortOrderItems.Count < m_nMaxCount);
        #endregion

        #region RemoveItemCommand
        public ICommand RemoveItemCommand => new RelayCommand(RemoveItem, () => SelectedSortOrderItem != null);
        #endregion

        #region MoveUpItemCommand
        public ICommand MoveUpItemCommand => new RelayCommand(MoveUpItem, () =>
        {
            return SelectedSortOrderItem != null && SelectedSortOrderItemIndex > 0;
        });
        #endregion

        #region MoveDownItemCommand
        public ICommand MoveDownItemCommand => new RelayCommand(MoveDownItem, () =>
        {
            return SelectedSortOrderItem != null && SelectedSortOrderItemIndex < (SortOrderItems.Count - 1);
        });
        #endregion

        #region SaveCommand
        public ICommand SaveCommand => new RelayCommand(SaveSortOrder, () => IsDirty);
        #endregion
        #endregion

        #region Ctor
        public SortOrderFlyoutViewModel()
        {
            SortOrderItems = new ObservableCollection<SortOrderItemModel>();

#if DEBUG
            if (IsInDesignMode)
            {
                InitializeAsync().Forget();
            }
            else { }
#endif
        }
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            SortOrderItems.Clear();

            SortOrder sortOrder = SortOrder.Parse(DustUtilityPlugin.Config.SortOrder);

            for (int i = 0; i < sortOrder?.Count; i++)
            {
                SortOrderItems.Add(sortOrder[i]);
            }

            m_nMaxCount = Enum.GetValues(typeof(SortOrder.OrderItem)).Length;

            SetInitialPropertyValue(nameof(SortOrderString), DustUtilityPlugin.Config.SortOrder);
        }
        #endregion

        #region AddItem
        private async void AddItem()
        {
            List<SortOrderItemModel> lstUnusedItems = GetUnusedSortOrderItems();

            SortOrderItemSelectorDialogViewModel viewModel = ServiceLocator.Current.GetInstance<SortOrderItemSelectorDialogViewModel>();

            viewModel.SortOrderItems.Clear();

            for (int i = 0; i < lstUnusedItems.Count; i++)
            {
                viewModel.SortOrderItems.Add(lstUnusedItems[i]);
            }

            await viewModel.InitializeAsync();

            SortOrderItemSelectorDialogView dialog = new SortOrderItemSelectorDialogView()
            {
                Owner = DustUtilityPlugin.MainWindow
            };

            if (dialog.ShowDialog().Value)
            {
                SortOrder.OrderItem orderItem = ServiceLocator.Current.GetInstance<SortOrderItemSelectorDialogViewModel>().SelectedSortOrderItem.Value;

                SortOrderItems.Add(new SortOrderItemModel(orderItem));

                UpdateSortOrderString();
            }
            else { }
        }
        #endregion

        #region RemoveItem
        private void RemoveItem()
        {
            int nIndex = SelectedSortOrderItemIndex;

            SortOrderItems.Remove(SelectedSortOrderItem);

            SelectedSortOrderItemIndex = (nIndex > 0 ? nIndex - 1 : 0);

            UpdateSortOrderString();
        }
        #endregion

        #region MoveUpItem
        private void MoveUpItem()
        {
            int nIndex = SortOrderItems.IndexOf(SelectedSortOrderItem);

            SortOrderItems.Move(nIndex, nIndex - 1);

            UpdateSortOrderString();
        }
        #endregion

        #region MoveDownItem
        private void MoveDownItem()
        {
            int nIndex = SortOrderItems.IndexOf(SelectedSortOrderItem);

            SortOrderItems.Move(nIndex, nIndex + 1);

            UpdateSortOrderString();
        }
        #endregion

        #region SaveSortOrder
        private async void SaveSortOrder()
        {
            DustUtilityPlugin.Config.SortOrder = CreateSortOrderString();

            DustUtilityPlugin.MainWindow.SortOrderFlyout.IsOpen = false;

            await ServiceLocator.Current.GetInstance<MainViewModel>().UpdateCardItemsSortOrderAsync();
        }
        #endregion

        #region GetUnusedSortOrderItems
        private List<SortOrderItemModel> GetUnusedSortOrderItems()
        {
            List<SortOrderItemModel> lstRet = new List<SortOrderItemModel>();

            SortOrder.OrderItem[] vItems = (SortOrder.OrderItem[])Enum.GetValues(typeof(SortOrder.OrderItem));

            List<SortOrderItemModel> lstModels = new List<SortOrderItemModel>(SortOrderItems);

            for (int i = 0; i < vItems.Length; i++)
            {
                SortOrderItemModel model = new SortOrderItemModel(vItems[i]);

                if (lstModels.Find(m => m.Value == model.Value) == null)
                {
                    lstRet.Add(model);
                }
                else { }
            }

            return lstRet;
        }
        #endregion

        #region CreateSortOrderString
        private string CreateSortOrderString()
        {
            string strRet = string.Empty;

            if (SortOrderItems.Count > 0)
            {
                strRet = SortOrderItems[0].Value.ToString();

                for (int i = 1; i < SortOrderItems.Count; i++)
                {
                    strRet = $"{strRet};{SortOrderItems[i].Value}";
                }
            }
            else { }

            return strRet;
        }
        #endregion

        #region UpdateSortOrderString
        private void UpdateSortOrderString()
        {
            SortOrderString = CreateSortOrderString();
        }
        #endregion
    }
}