using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Components.Converters
{
    public class CardSetToImageConverter : IValueConverter
    {
        #region Static Fields
        private static ResourceDictionary s_setIcons;
        #endregion

        #region Static Ctor
        static CardSetToImageConverter()
        {
            s_setIcons = new ResourceDictionary
            {
                Source = new Uri("/Spawn.HDT.DustUtility;component/Resources/CardSetIcons.xaml", UriKind.Relative)
            };
        }
        #endregion

        #region Convert
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource retVal = null;

            if (value is CardSet)
            {
                switch ((CardSet)value)
                {
                    case CardSet.EXPERT1:
                        retVal = (ImageSource)s_setIcons["ExpertIcon"];
                        break;

                    case CardSet.NAXX:
                        retVal = (ImageSource)s_setIcons["NaxxIcon"];
                        break;

                    case CardSet.GVG:
                        retVal = (ImageSource)s_setIcons["GoblinsIcon"];
                        break;

                    case CardSet.BRM:
                        retVal = (ImageSource)s_setIcons["MountainIcon"];
                        break;

                    case CardSet.TGT:
                        retVal = (ImageSource)s_setIcons["TournamentIcon"];
                        break;

                    case CardSet.LOE:
                        retVal = (ImageSource)s_setIcons["LeagueIcon"];
                        break;

                    case CardSet.OG:
                        retVal = (ImageSource)s_setIcons["OldGodsIcon"];
                        break;

                    case CardSet.KARA:
                        retVal = (ImageSource)s_setIcons["KarazhanIcon"];
                        break;

                    case CardSet.GANGS:
                        retVal = (ImageSource)s_setIcons["GadgetzanIcon"];
                        break;

                    case CardSet.UNGORO:
                        retVal = (ImageSource)s_setIcons["UngoroIcon"];
                        break;

                    case CardSet.ICECROWN:
                        retVal = (ImageSource)s_setIcons["FrozenThroneIcon"];
                        break;

                    case CardSet.LOOTAPALOOZA:
                        retVal = (ImageSource)s_setIcons["KoboldsIcon"];
                        break;

                    case CardSet.HOF:
                        retVal = (ImageSource)s_setIcons["HallIcon"];
                        break;
                }
            }
            else
            {
                Log.WriteLine($"Passed invalid value: \"{value}\"", LogType.Error);
            }

            return retVal;
        }
        #endregion

        #region ConvertBack
        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}