#region Using
using GalaSoft.MvvmLight.CommandWpf;
using Spawn.HDT.DustUtility.Services;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class SortOrderItemSelectorDialogViewModel : ViewModelBase, IResultProvider<SortOrder.OrderItem>
    {
        #region Member Variables
        private string m_strWindowTitle;
        private bool? m_blnAccepted;
        private SortOrderItemModel m_selectedSortOrderItem;
        private int m_nSelectedSortOrderItemIndex;
        #endregion

        #region Properties
        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion

        #region Accepted
        public bool? Accepted
        {
            get => m_blnAccepted;
            set => Set(ref m_blnAccepted);
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

        #region AcceptCommand
        public ICommand AcceptCommand => new RelayCommand(Accept, () => SelectedSortOrderItem != null);
        #endregion
        #endregion

        #region Ctor
        public SortOrderItemSelectorDialogViewModel()
        {
            WindowTitle = "Dust Utility - Add new item...";

            SortOrderItems = new ObservableCollection<SortOrderItemModel>();
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            SelectedSortOrderItemIndex = 0;
        }
        #endregion

        #region Accept
        public void Accept()
        {
            Accepted = true;
        }
        #endregion

        #region GetResult
        public SortOrder.OrderItem GetResult() => SelectedSortOrderItem.Value;
        #endregion
    }
}