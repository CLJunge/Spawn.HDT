#region Using
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Spawn.HDT.DustUtility.AccountManagement;
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
#if DEBUG
            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
                SimpleIoc.Default.Register<IAccount>(() => new MockAccount());
            }
            else { }
#endif

            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<HistoryFlyoutViewModel>();
            SimpleIoc.Default.Register<UpdateFlyoutViewModel>();
            SimpleIoc.Default.Register<DecksFlyoutViewModel>();
            SimpleIoc.Default.Register<DeckListFlyoutViewModel>();
            SimpleIoc.Default.Register<SearchParametersFlyoutViewModel>();
            SimpleIoc.Default.Register<SortOrderFlyoutViewModel>();

            SimpleIoc.Default.Register<AccountSelectorDialogViewModel>();
            SimpleIoc.Default.Register<SettingsDialogViewModel>();
        }
        #endregion
    }
}