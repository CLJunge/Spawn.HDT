﻿#region Using
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
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

                IsDirty = !ComparePropertyValues(objInitialValue, objNewValue);

                if (IsDirty && !DirtyProperties.Contains(e.PropertyName))
                {
                    DirtyProperties.Add(e.PropertyName);
                }
                else if (!IsDirty && DirtyProperties.Contains(e.PropertyName))
                {
                    DirtyProperties.Remove(e.PropertyName);
                }
                else { }

                NotifyDirtyStatus?.Invoke(sender, new NotifyDirtyStatusEventArgs(e.PropertyName, IsDirty));
            }
            else { }
        }
        #endregion
        #endregion

        #region InitializeAsync
        public abstract Task InitializeAsync();
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