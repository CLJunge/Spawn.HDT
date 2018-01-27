using System;
using System.Globalization;
using System.Windows.Data;

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class CountLabelConverter : IMultiValueConverter
    {
        #region Convert
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = $"{values[0]}x";

            if (values.Length == 2 && (System.Convert.ToBoolean(values[1]) && System.Convert.ToInt32(values[0]) > 0))
            {
                strRet = $"+{strRet}";
            }
            else { }

            return strRet;
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