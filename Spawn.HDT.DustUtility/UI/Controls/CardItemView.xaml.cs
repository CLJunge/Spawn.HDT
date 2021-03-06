﻿#region Using
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.UI.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
#endregion

namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardItemView
    {
        #region Ctor
        public CardItemView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'CardItemView' instance");
        }
        #endregion

        #region Events
        #region OnDataContextChanged
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is CardItemModel cardItem)
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Updating data context to '{cardItem.Name}...");

                DateTextBlock.Visibility = cardItem.Date.HasValue ? Visibility.Visible : Visibility.Collapsed;

                RarityGemImage.Source = FindResource($"{cardItem.RarityString}Gem") as ImageSource;

                if (cardItem.CardClass == HearthDb.Enums.CardClass.DEMONHUNTER)
                    CardClassImage.SetResourceReference(Image.SourceProperty, "DemonHunterClassIcon");
                else
                    CardClassImage.SetResourceReference(Image.SourceProperty, $"{cardItem.CardClassString}ClassIcon");

                CardSetImage.SetResourceReference(Image.SourceProperty, $"{cardItem.CardSet.GetShortString()}Icon");

                DateTextBlock.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
                CountTextBlock.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
                ManaCostTextBlock.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
                DustTextBlock.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
                CardClassTextBlock.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
                CardSetTextBlock.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");

                if (cardItem.Golden)
                    ControlBorder.BorderBrush = Brushes.Goldenrod;
                else
                    ControlBorder.SetResourceReference(Border.BorderBrushProperty, "BlackBrush");

                if (DustUtilityPlugin.Config.ColoredCardLabels)
                {
                    RarityTextBlock.Foreground = DustUtilityPlugin.RarityBrushes[(int)cardItem.Wrapper.DbCard.Rarity];

                    NameTextBlock.Foreground = cardItem.Golden ? Brushes.Goldenrod : DustUtilityPlugin.RarityBrushes[(int)cardItem.Wrapper.DbCard.Rarity];

                    if (cardItem.ColoredCount)
                    {
                        if (cardItem.Count > 0)
                            CountTextBlock.Foreground = Brushes.Lime;
                        else if (cardItem.Count < 0)
                            CountTextBlock.Foreground = Brushes.Red;
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}