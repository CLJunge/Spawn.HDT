#region Using
using Spawn.HDT.DustUtility.UI.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
#endregion

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
        #region OnDataContextChanged
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is CardItem cardItem)
            {
                CardClassImage.SetResourceReference(Image.SourceProperty, $"{cardItem.CardClassString}ClassIcon");

                CardSetImage.SetResourceReference(Image.SourceProperty, $"{cardItem.CardSet.GetShortString()}Icon");

                RarityGemImage.Source = FindResource($"{cardItem.Rarity}Gem") as ImageSource;

                if (DustUtilityPlugin.Config.ColoredCardItems)
                {
                    RarityTextBlock.Foreground = DustUtilityPlugin.RarityBrushes[(int)cardItem.Wrapper.DbCard.Rarity];

                    if (cardItem.Golden)
                    {
                        NameTextBlock.Foreground = Brushes.Goldenrod;
                        ControlBorder.BorderBrush = Brushes.Goldenrod;
                    }
                    else
                    {
                        NameTextBlock.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
                        ControlBorder.SetResourceReference(Border.BorderBrushProperty, "BlackBrush");
                    }
                }
                else { }

                if (cardItem.Timestamp.HasValue)
                {
                    TimestampTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    TimestampTextBlock.Visibility = Visibility.Collapsed;
                }
            }
            else { }
        }
        #endregion
        #endregion
    }
}