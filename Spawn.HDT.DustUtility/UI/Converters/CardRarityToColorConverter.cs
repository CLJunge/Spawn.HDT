using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Converters
{
    public class CardRarityToColorConverter : IValueConverter
    {
        #region Static Variables
        private static Dictionary<int, SolidColorBrush> s_dColors = new Dictionary<int, SolidColorBrush>();
        #endregion

        #region Properties
        public static Dictionary<int, SolidColorBrush> Brushes => s_dColors;
        #endregion

        #region Static Ctor
        static CardRarityToColorConverter()
        {
            //Basic
            s_dColors.Add(2, new SolidColorBrush(Colors.Black));
            //Common
            s_dColors.Add(1, new SolidColorBrush(Color.FromRgb(38, 168, 16)));
            //Rare
            s_dColors.Add(3, new SolidColorBrush(Color.FromRgb(18, 92, 204)));
            //Epic
            s_dColors.Add(4, new SolidColorBrush(Color.FromRgb(135, 14, 186)));
            //Legendary
            s_dColors.Add(5, new SolidColorBrush(Color.FromRgb(226, 119, 24)));
        }
        #endregion

        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush retVal = s_dColors[2]; //Basic

            try
            {
                retVal = s_dColors[System.Convert.ToInt32(value)];
            }
            catch
            {
                //Invalid rarity
                Log.WriteLine("Passed invalid rarity", LogType.Debug);
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