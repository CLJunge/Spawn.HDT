using GalaSoft.MvvmLight;
using System.Diagnostics;

namespace Spawn.HDT.DustUtility.UI.Models
{
    [DebuggerDisplay("Name={Name} Value={Value}")]
    public class SortOrderItemModel : ObservableObject
    {
        #region Member Variables
        private SortOrder.OrderItem m_item;
        #endregion

        #region Properties
        #region Value
        public SortOrder.OrderItem Value
        {
            get => m_item;
            set => Set(ref m_item, value);
        }
        #endregion

        #region Name
        public string Name => SortOrder.ItemToString(Value);
        #endregion
        #endregion

        #region Ctor
        public SortOrderItemModel()
        {
            PropertyChanged += (s, e) => RaisePropertyChanged(nameof(Name));
        }

        public SortOrderItemModel(SortOrder.OrderItem item)
            : this()
        {
            Value = item;
        }
        #endregion
    }
}