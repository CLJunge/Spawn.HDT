#region Using
using Spawn.HDT.DustUtility.Logging;
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

            if (values.Length == 2
                && (values[0] is Visibility v1 && values[1] is Visibility v2)
                && (v1 == Visibility.Visible && v2 == Visibility.Visible))
            {
                retVal = Visibility.Visible;

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Converted [{string.Join(",", values)}] to '{retVal}'");
            }

            return retVal;
        }
        #endregion

        #region ConvertBack
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
        #endregion
    }
}