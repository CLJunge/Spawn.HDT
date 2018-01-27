using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class CardClassToImageConverter : IValueConverter
    {
        #region Static Fields
        private static ResourceDictionary s_classIcons;
        #endregion

        #region Static Ctor
        static CardClassToImageConverter()
        {
            s_classIcons = new ResourceDictionary
            {
                Source = new Uri("/Spawn.HDT.DustUtility;component/Resources/CardClassIcons.xaml", UriKind.Relative)
            };
        }
        #endregion

        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource retVal = null;

            try
            {
                if (s_classIcons != null && s_classIcons.Count > 0)
                {
                    retVal = (ImageSource)s_classIcons[$"{value}ClassIcon"];
                }
                else { }
            }
            catch
            {
                //Invalid class
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