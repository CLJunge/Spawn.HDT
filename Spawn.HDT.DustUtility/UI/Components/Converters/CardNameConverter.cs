using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class CardNameConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush retVal = Brushes.Black;
            //#c7a602
            if (value is bool && (bool)value)
            {
                retVal = Brushes.Goldenrod;
            }
            else { }

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