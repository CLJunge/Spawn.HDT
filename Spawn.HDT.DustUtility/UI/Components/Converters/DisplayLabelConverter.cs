﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class DisplayLabelConverter : IValueConverter
    {
        #region Properties
        public string Prefix { get; set; }
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