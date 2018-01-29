#region Using
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class DisplayTextConverter : IValueConverter
    {
        #region Properties
        #region Prefix
        public string Prefix { get; set; }
        #endregion

        #region Suffix
        public string Suffix { get; set; }
        #endregion
        #endregion

        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = value?.ToString();

            if (!string.IsNullOrEmpty(strRet) && !string.IsNullOrEmpty(Prefix))
            {
                strRet = $"{Prefix} {strRet}";
            }
            else { }

            if (!string.IsNullOrEmpty(strRet) && !string.IsNullOrEmpty(Suffix))
            {
                strRet = $"{strRet} {Suffix}";
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