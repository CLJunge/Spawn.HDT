using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class CardRarityToColorConverter : IValueConverter
    {
        #region Static Fields
        private static Dictionary<int, SolidColorBrush> s_dColors = new Dictionary<int, SolidColorBrush>();
        #endregion

        #region Static Properties
        #region Brushes
        public static Dictionary<int, SolidColorBrush> Brushes => s_dColors;
        #endregion
        #endregion

        #region Static Ctor
        static CardRarityToColorConverter()
        {
            //Common
            s_dColors.Add(1, new SolidColorBrush(Color.FromRgb(38, 168, 16)));
            //Rare
            //s_dColors.Add(3, new SolidColorBrush(Color.FromRgb(18, 92, 204)));
            s_dColors.Add(3, new SolidColorBrush(Color.FromRgb(30, 113, 255)));
            //Epic
            //s_dColors.Add(4, new SolidColorBrush(Color.FromRgb(135, 14, 186)));
            s_dColors.Add(4, new SolidColorBrush(Color.FromRgb(163, 58, 234)));
            //Legendary
            //s_dColors.Add(5, new SolidColorBrush(Color.FromRgb(226, 119, 24)));
            s_dColors.Add(5, new SolidColorBrush(Color.FromRgb(255, 153, 0)));
        }
        #endregion

        #region Properties
        #region DefaultColorBrush
        public SolidColorBrush DefaultColorBrush { get; set; }
        #endregion
        #endregion

        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush retVal = DefaultColorBrush;

            try
            {
                retVal = s_dColors[System.Convert.ToInt32(value)];
            }
            catch
            {
                //Invalid rarity
                Log.WriteLine($"Passed invalid value: \"{value}\"", LogType.Error);
            }

            return retVal;
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