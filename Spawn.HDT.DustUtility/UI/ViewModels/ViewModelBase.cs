#region Using
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        #region Constants
        public const string IsDirtySuffix = "*";
        #endregion

        #region Member Variables
        protected Dictionary<string, object> m_dInitialPropertyValues;
        #endregion

        #region Properties
        #region ReloadRequired
        public bool ReloadRequired { get; set; } = true;
        #endregion

        #region NotifyDirtyStatus
        public abstract bool CanNotifyDirtyStatus { get; }
        #endregion

        #region IsDirty
        public bool IsDirty { get; set; } = false;
        #endregion

        #region DirtyProperties
        public List<string> DirtyProperties { get; }
        #endregion
        #endregion

        #region Custom Events
        public event EventHandler<NotifyDirtyStatusEventArgs> NotifyDirtyStatus;
        #endregion

        #region Ctor
        public ViewModelBase()
        {
            if (CanNotifyDirtyStatus)
            {
                m_dInitialPropertyValues = new Dictionary<string, object>();
                DirtyProperties = new List<string>();

                PropertyChanged += OnPropertyChanged;
            }
            else { }
        }
        #endregion

        #region Events
        #region OnPropertyChanged
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (CanNotifyDirtyStatus && (m_dInitialPropertyValues?.ContainsKey(e.PropertyName) ?? false))
            {
                object objInitialValue = m_dInitialPropertyValues[e.PropertyName];

                object objNewValue = GetType().GetProperty(e.PropertyName).GetValue(this, null);

                bool blnIsDirty = !ComparePropertyValues(objInitialValue, objNewValue);

                if (blnIsDirty && (!DirtyProperties?.Contains(e.PropertyName) ?? false))
                {
                    DirtyProperties.Add(e.PropertyName);
                }
                else if (!blnIsDirty && (DirtyProperties?.Contains(e.PropertyName) ?? false))
                {
                    DirtyProperties.Remove(e.PropertyName);
                }
                else { }

                IsDirty = DirtyProperties?.Count > 0;

                NotifyDirtyStatus?.Invoke(sender, new NotifyDirtyStatusEventArgs(e.PropertyName, blnIsDirty));
            }
            else { }
        }
        #endregion
        #endregion

        #region InitializeAsync
        public virtual async Task InitializeAsync()
        {
            await Task.Delay(5);

            if (CanNotifyDirtyStatus)
            {
                IsDirty = false;

                m_dInitialPropertyValues?.Clear();

                DirtyProperties?.Clear();
            }
            else { }
        }
        #endregion

        #region AddInitialPropertyValue
        protected void SetInitialPropertyValue(string strPropertyName, object objValue)
        {
            if (CanNotifyDirtyStatus && m_dInitialPropertyValues != null)
            {
                m_dInitialPropertyValues[strPropertyName] = objValue;
            }
            else { }
        }
        #endregion

        #region ComparePropertyValues
        protected virtual bool ComparePropertyValues<T>(T a, T b)
        {
            return EqualityComparer<T>.Default.Equals(a, b);
        }
        #endregion
    }
}