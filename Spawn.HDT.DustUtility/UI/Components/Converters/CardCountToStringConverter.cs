#region Using
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class CardCountToStringConverter : IValueConverter
    {
        #region Properties
        #region MaxAmount
        public int MaxAmount { get; set; }
        #endregion

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
            string strRet = string.Empty;

            if (value is int)
            {
                strRet = $"{value}/{MaxAmount}";

                if (!string.IsNullOrEmpty(Prefix))
                {
                    strRet = $"{Prefix} {strRet}";
                }
                else { }

                if (!string.IsNullOrEmpty(Suffix))
                {
                    strRet = $"{strRet} {Suffix}";
                }
                else { }
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