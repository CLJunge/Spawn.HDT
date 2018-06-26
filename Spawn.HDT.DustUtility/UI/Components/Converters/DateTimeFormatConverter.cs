#region Using
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class DateTimeFormatConverter : IValueConverter
    {
        #region Properties
        #region FormatString
        public string FormatString { get; set; }
        #endregion
        #endregion

        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = null;

            if (value is DateTime date)
            {
                strRet = date.ToString(FormatString, CultureInfo.InvariantCulture);

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Converted '{value}' to '{strRet}' (FormatString={FormatString})");
            }
            else
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Passed invalid value: \"{value}\"! (FormatString={FormatString})");
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