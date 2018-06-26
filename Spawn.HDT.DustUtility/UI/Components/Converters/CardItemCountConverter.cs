#region Using
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class CardItemCountConverter : IMultiValueConverter
    {
        #region Convert
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = string.Empty;

            try
            {
                strRet = $"{values[0]}x";

                if (values.Length == 2 && (System.Convert.ToBoolean(values[1]) && System.Convert.ToInt32(values[0]) > 0))
                {
                    strRet = $"+{strRet}";
                }

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Converted [{string.Join(",", values)}] to '{strRet}'");
            }
            catch
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Passed invalid values: \"{string.Join(", ", values)}\"!");
            }

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