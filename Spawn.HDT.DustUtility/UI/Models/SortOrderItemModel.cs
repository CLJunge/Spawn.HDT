#region Using
using GalaSoft.MvvmLight;
using System.Diagnostics;
#endregion

namespace Spawn.HDT.DustUtility.UI.Models
{
    [DebuggerDisplay("{Name} ({Value})")]
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
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(Value)))
                    RaisePropertyChanged(nameof(Name));
            };

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'SortOrderItemModel' instance");
        }

        public SortOrderItemModel(SortOrder.OrderItem item) : this() => Value = item;
        #endregion
    }
}