#region Using
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class TimestampConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = null;

            if (value is DateTime date)
            {
                strRet = date.ToString("d", CultureInfo.InvariantCulture);
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