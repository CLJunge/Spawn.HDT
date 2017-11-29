﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Spawn.HDT.DustUtility.UI
{
    public class SearchResultContainer : INotifyPropertyChanged
    {
        #region Member Variables
        private int m_nDust;
        private int m_nCommonsCount;
        private int m_nRaresCount;
        private int m_nEpicsCount;
        private int m_nLegendariesCount;
        private int m_nTotalCount;
        private ObservableCollection<DataGridCardItem> m_lstGridItems;
        #endregion

        #region Properties
        #region Dust
        public int Dust
        {
            get => m_nDust;
            set
            {
                m_nDust = value;
                OnPropertyChanged("Dust");
            }
        }
        #endregion

        #region CommonsCount
        public int CommonsCount
        {
            get => m_nCommonsCount;
            set
            {
                m_nCommonsCount = value;
                OnPropertyChanged("CommonsCount");
            }
        }
        #endregion

        #region RaresCount
        public int RaresCount
        {
            get => m_nRaresCount;
            set
            {
                m_nRaresCount = value;
                OnPropertyChanged("RaresCount");
            }
        }
        #endregion

        #region EpicsCount
        public int EpicsCount
        {
            get => m_nEpicsCount;
            set
            {
                m_nEpicsCount = value;
                OnPropertyChanged("EpicsCount");
            }
        }
        #endregion

        #region LegendariesCount
        public int LegendariesCount
        {
            get => m_nLegendariesCount;
            set
            {
                m_nLegendariesCount = value;
                OnPropertyChanged("LegendariesCount");
            }
        }
        #endregion

        #region TotalCount
        public int TotalCount
        {
            get => m_nTotalCount;
            set
            {
                m_nTotalCount = value;
                OnPropertyChanged("TotalCount");
            }
        }
        #endregion

        #region GridItems
        public ObservableCollection<DataGridCardItem> GridItems
        {
            get => m_lstGridItems;
            set => m_lstGridItems = value;
        }
        #endregion
        #endregion

        #region Custom Events
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }
        #endregion
        #endregion

        #region Ctor
        public SearchResultContainer()
        {
            m_lstGridItems = new ObservableCollection<DataGridCardItem>();
        }
        #endregion

        #region CopyTo
        public void CopyTo(SearchResultContainer searchResult)
        {
            if (searchResult != null)
            {
                searchResult.Clear();

                searchResult.Dust = m_nDust;
                searchResult.CommonsCount = m_nCommonsCount;
                searchResult.RaresCount = m_nRaresCount;
                searchResult.EpicsCount = m_nEpicsCount;
                searchResult.LegendariesCount = m_nLegendariesCount;
                searchResult.TotalCount = m_nTotalCount;

                for (int i = 0; i < m_lstGridItems.Count; i++)
                {
                    searchResult.GridItems.Add(m_lstGridItems[i]);
                }
            }
            else { }
        }
        #endregion

        #region SetGridItems
        public void SetGridItems(IEnumerable<DataGridCardItem> items)
        {
            m_lstGridItems.Clear();

            List<DataGridCardItem> lstItems = new List<DataGridCardItem>(items);

            for (int i = 0; i < lstItems.Count; i++)
            {
                m_lstGridItems.Add(lstItems[i]);
            }
        }
        #endregion

        #region Clear
        private void Clear()
        {
            Dust = 0;
            CommonsCount = 0;
            RaresCount = 0;
            EpicsCount = 0;
            LegendariesCount = 0;
            TotalCount = 0;

            GridItems.Clear();
        }
        #endregion
    }
}