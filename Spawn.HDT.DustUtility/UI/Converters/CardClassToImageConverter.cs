using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Converters
{
    public class CardClassToImageConverter : IValueConverter
    {
        #region Static Stuff
        private static ResourceDictionary s_classIcons;

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

            if (s_classIcons != null && s_classIcons.Count > 0)
            {
                string strKey = $"{value}ClassIcon";

                if (s_classIcons.Contains(strKey))
                {
                    retVal = (ImageSource)s_classIcons[strKey];
                }
                else { }
            }
            else { }

            return retVal;
        }
        #endregion

        #region Convert
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}