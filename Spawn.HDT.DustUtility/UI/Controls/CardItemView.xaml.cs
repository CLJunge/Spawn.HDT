using Spawn.HDT.DustUtility.UI.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardItemView : UserControl
    {
        #region Ctor
        public CardItemView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnLoaded
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CardItem cardItem)
            {
                CardClassImage.SetResourceReference(Image.SourceProperty, $"{cardItem.CardClassString}ClassIcon");

                CardSetImage.SetResourceReference(Image.SourceProperty, $"{cardItem.CardSet.GetShortString()}Icon");

                RarityGemImage.Source = FindResource($"{cardItem.Rarity}Gem") as ImageSource;

                RarityTextBlock.Foreground = DustUtilityPlugin.RarityBrushes[(int)cardItem.Wrapper.DbCard.Rarity];

                if (cardItem.Golden)
                {
                    NameTextBlock.Foreground = Brushes.Goldenrod;
                    ControlBorder.BorderBrush = Brushes.Goldenrod;
                }
                else { }

                if (cardItem.Timestamp.HasValue)
                {
                    TimestampTextBlock.Visibility = Visibility.Visible;
                }
                else { }
            }
            else { }
        }
        #endregion
        #endregion
    }
}