using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class TimestampConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = string.Empty;

            if (value is DateTime)
            {
                strRet = ((DateTime)value).ToString("d", culture);
            }
            else
            {
                Log.WriteLine($"Passed invalid value: \"{value}\"", LogType.Error);
            }

            return strRet;
        }
        #endregion

        #region ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}