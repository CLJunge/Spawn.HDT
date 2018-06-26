#region Using
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class CardSetCardCountConverter : IMultiValueConverter
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
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = string.Empty;

            if (values.Length == 2)
            {
                strRet = $"{values[0]}/{values[1]}";

                if (!string.IsNullOrEmpty(Prefix))
                {
                    strRet = $"{Prefix} {strRet}";
                }

                if (!string.IsNullOrEmpty(Suffix))
                {
                    strRet = $"{strRet} ({(int)((System.Convert.ToSingle(values[0]) / (int)values[1]) * 100)}%)";
                }

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Converted [{string.Join(",", values)}] to '{strRet}' (Prefix={Prefix}, Suffix={Suffix})");
            }
            else
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Passed invalid values: '{string.Join(", ", values)}'! (Prefix={Prefix}, Suffix={Suffix})");
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