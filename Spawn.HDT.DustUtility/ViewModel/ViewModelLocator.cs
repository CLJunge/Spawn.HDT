#region Using
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.ViewModel
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
                Type viewModelType = Type.GetType($"Spawn.HDT.DustUtility.ViewModel.{strViewModelName}");

                if (viewModelType != null)
                {
                    var viewModel = ServiceLocator.Current.GetInstance(viewModelType);

                    ((FrameworkElement)d).DataContext = viewModel;
                }
                else { }
            }
            else { }
        }
        #endregion
        #endregion

        #region Properties
        #region Main
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        #endregion
        #endregion

        #region Static Ctor
        static ViewModelLocator()
        {
            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            }
            else { }

            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<HistoryFlyoutViewModel>();

            SimpleIoc.Default.Register<AccountSelectorDialogViewModel>();
            SimpleIoc.Default.Register<SettingsDialogViewModel>();
        }
        #endregion
    }
}