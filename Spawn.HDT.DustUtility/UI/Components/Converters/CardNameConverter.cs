using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class CardNameConverter : IValueConverter
    {
        #region Properties
        #region DefaultColorBrush
        public SolidColorBrush DefaultColorBrush { get; set; }
        #endregion
        #endregion

        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush retVal = DefaultColorBrush;

            if (value is bool && (bool)value)
            {
                retVal = Brushes.Goldenrod;
            }
            else
            {
                Log.WriteLine($"Passed invalid value: \"{value}\"", LogType.Error);
            }

            return retVal;
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