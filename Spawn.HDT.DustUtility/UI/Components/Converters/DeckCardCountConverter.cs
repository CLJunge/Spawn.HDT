﻿#region Using
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class DeckCardCountConverter : IValueConverter
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

                if (!string.IsNullOrEmpty(Suffix))
                {
                    strRet = $"{strRet} {Suffix}";
                }

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Converted '{value}' to '{strRet}' (MaxAmount={MaxAmount}, Prefix={Prefix}, Suffix={Suffix})");
            }
            else
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Passed invalid value: '{value}'! (MaxAmount={MaxAmount}, Prefix={Prefix}, Suffix={Suffix})");
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