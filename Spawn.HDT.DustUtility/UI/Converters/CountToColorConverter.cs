using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Converters
{
    public class CountToColorConverter : IMultiValueConverter
    {
        #region Convert
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush retVal = Brushes.Black;

            if (values.Length == 2 && (values[1] is bool && System.Convert.ToBoolean(values[1])))
            {
                try
                {
                    int nValue = System.Convert.ToInt32(values[0]);

                    if (nValue > 0)
                    {
                        retVal = Brushes.Green;
                    }
                    else if (nValue < 0)
                    {
                        retVal = Brushes.Red;
                    }
                    else { }
                }
                catch
                {
                    //invalid value
                }
            }
            else { }

            return retVal;
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