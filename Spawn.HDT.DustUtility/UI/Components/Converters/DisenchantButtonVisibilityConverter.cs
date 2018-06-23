#region Using
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
#endregion

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class DisenchantButtonVisibilityConverter : IMultiValueConverter
    {
        #region Convert
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility retVal = Visibility.Hidden;

            if (values.Length == 2 && (Visibility)values[0] == Visibility.Visible && (Visibility)values[1] == Visibility.Visible)
            {
                retVal = Visibility.Visible;
            }
            else { }

            return retVal;
        }
        #endregion

        #region ConvertBack
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}