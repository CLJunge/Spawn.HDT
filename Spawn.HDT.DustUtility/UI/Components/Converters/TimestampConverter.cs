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
            else { }

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