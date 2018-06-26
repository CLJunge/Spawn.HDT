#region Using
using CommonServiceLocator;
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class ViewModelLocator
    {
        #region Static Properties
        #region ViewModelName AP
        public static string GetViewModelName(DependencyObject obj)
        {
            return (string)obj.GetValue(ViewModelNameProperty);
        }

        public static void SetViewModelName(DependencyObject obj, string value)
        {
            obj.SetValue(ViewModelNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for ViewModelName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelNameProperty =
            DependencyProperty.RegisterAttached("ViewModelName", typeof(string), typeof(ViewModelLocator), new PropertyMetadata(string.Empty, OnViewModelNameChanged));

        private static void OnViewModelNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string strViewModelName = e.NewValue?.ToString();

            if (!string.IsNullOrEmpty(strViewModelName))
            {
                Type viewModelType = Type.GetType($"{typeof(MainViewModel).Namespace}.{strViewModelName}");

                if (viewModelType != null)
                {
                    ((FrameworkElement)d).DataContext = ServiceLocator.Current.GetInstance(viewModelType);

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Setting data context for '{strViewModelName}'");
                }
            }
        }
        #endregion
        #endregion

        #region Properties
        #region Main
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        #endregion
        #endregion

        #region Static Ctor
#if DEBUG
        static ViewModelLocator()
        {
            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
            {
                DustUtilityPlugin.CreateContainer();
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Initialized 'ViewModelLocator'");
        }
#endif
        #endregion
    }
}