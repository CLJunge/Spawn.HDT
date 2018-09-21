#region Using
using GalaSoft.MvvmLight.Messaging;
using Spawn.HDT.DustUtility.Messaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class SearchParametersFlyoutView
    {
        #region Contants
        private const string SetsGroupBoxName = "SetsGroupBox";
        private const string RaritiesGroupBoxName = "RaritiesGroupBox";
        private const string ClassesGroupBoxName = "ClassesGroupBox";
        private const string MiscGroupBoxName = "MiscGroupBox";
        #endregion

        #region Member Variables
        private bool m_blnFlyoutInititalized;
        private bool m_blnSkipAllActions;

        private int m_mMaxSetCheckBoxes;
        private int m_mMaxRarityCheckBoxes;
        private int m_mMaxClassCheckBoxes;
        private int m_mMaxMiscCheckBoxes;
        #endregion

        #region Ctor
        public SearchParametersFlyoutView()
        {
            InitializeComponent();

            m_blnSkipAllActions = true;

            m_mMaxSetCheckBoxes = (SetsGroupBox.Content as Grid).Children.OfType<CheckBox>().Count();
            m_mMaxRarityCheckBoxes = (RaritiesGroupBox.Content as Grid).Children.OfType<CheckBox>().Count();
            m_mMaxClassCheckBoxes = (ClassesGroupBox.Content as Grid).Children.OfType<CheckBox>().Count();
            m_mMaxMiscCheckBoxes = (MiscGroupBox.Content as Grid).Children.OfType<CheckBox>().Count();

            Messenger.Default.Register<FlyoutInitializedMessage>(this, msg => m_blnFlyoutInititalized = msg.FlyoutName.Equals(DustUtilityPlugin.SearchParametersFlyoutName));

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'SearchParametersFlyoutView' instance");
        }
        #endregion

        #region Events
        #region OnCheckBoxAllChecked
        private void OnCheckBoxAllChecked(object sender, RoutedEventArgs e)
        {
            if (m_blnFlyoutInititalized && !m_blnSkipAllActions)
            {
                GroupBox groupBox = (sender as FrameworkElement).FindParent<GroupBox>();

                if (groupBox?.Content is Grid contentGrid)
                {
                    foreach (CheckBox cb in contentGrid.Children.OfType<CheckBox>())
                    {
                        cb.IsChecked = true;
                    }
                }
            }
        }
        #endregion

        #region OnCheckBoxAllUnchecked
        private void OnCheckBoxAllUnchecked(object sender, RoutedEventArgs e)
        {
            if (m_blnFlyoutInititalized && !m_blnSkipAllActions)
            {
                GroupBox groupBox = (sender as FrameworkElement).FindParent<GroupBox>();

                if (groupBox?.Content is Grid contentGrid)
                {
                    foreach (CheckBox cb in contentGrid.Children.OfType<CheckBox>())
                    {
                        cb.IsChecked = false;
                    }
                }
            }
        }
        #endregion

        #region OnCheckBoxChecked
        private void OnCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            if (m_blnFlyoutInititalized)
            {
                GroupBox groupBox = (sender as FrameworkElement).FindParent<GroupBox>();

                if (groupBox?.Content is Grid contentGrid)
                {
                    m_blnSkipAllActions = true;

                    int nCheckedCount = contentGrid.Children.OfType<CheckBox>().Sum(cb => (bool)cb.IsChecked ? 1 : 0);

                    switch (groupBox.Name)
                    {
                        case SetsGroupBoxName:
                            SetAllCheckBox.IsChecked = m_mMaxSetCheckBoxes == nCheckedCount;
                            break;

                        case RaritiesGroupBoxName:
                            RarityAllCheckBox.IsChecked = m_mMaxRarityCheckBoxes == nCheckedCount;
                            break;

                        case ClassesGroupBoxName:
                            ClassAllCheckBox.IsChecked = m_mMaxClassCheckBoxes == nCheckedCount;
                            break;

                        case MiscGroupBoxName:
                            MiscAllCheckBox.IsChecked = m_mMaxMiscCheckBoxes == nCheckedCount;
                            break;
                    }

                    m_blnSkipAllActions = false;
                }
            }
        }
        #endregion

        #region OnCheckBoxUnchecked
        private void OnCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            if (m_blnFlyoutInititalized)
            {
                GroupBox groupBox = (sender as FrameworkElement).FindParent<GroupBox>();

                if (groupBox?.Content is Grid contentGrid)
                {
                    m_blnSkipAllActions = true;

                    switch (groupBox.Name)
                    {
                        case SetsGroupBoxName:
                            SetAllCheckBox.IsChecked = false;
                            break;

                        case RaritiesGroupBoxName:
                            RarityAllCheckBox.IsChecked = false;
                            break;

                        case ClassesGroupBoxName:
                            ClassAllCheckBox.IsChecked = false;
                            break;

                        case MiscGroupBoxName:
                            MiscAllCheckBox.IsChecked = false;
                            break;
                    }

                    m_blnSkipAllActions = false;
                }
            }
        }
        #endregion
        #endregion
    }
}