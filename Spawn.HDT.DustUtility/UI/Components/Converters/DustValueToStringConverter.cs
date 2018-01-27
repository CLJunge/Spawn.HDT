using System;
using System.Globalization;
using System.Windows.Data;

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class DustValueToStringConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = string.Empty;

            if (value is int)
            {
                strRet = $"{value} Dust";
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