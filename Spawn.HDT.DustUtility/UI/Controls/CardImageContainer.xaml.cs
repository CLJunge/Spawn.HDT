using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.Net;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardImageContainer
    {
        #region Member Variables
        private CardWrapper m_wrapper;
        private ImageSource m_defaultImageSource;
        private Thickness m_defaultImageMargin;
        private Stream m_currentImageStream;
        #endregion

        #region Ctor
        public CardImageContainer()
        {
            InitializeComponent();

            m_defaultImageSource = image.Source;
            m_defaultImageMargin = image.Margin;
        }
        #endregion

        #region UpdateCardWrapperAsync
        public async Task UpdateCardWrapperAsync(CardWrapper wrapper)
        {
            if (wrapper != null && (wrapper.Card.Id != m_wrapper?.Card.Id || wrapper.Card.Premium != m_wrapper?.Card.Premium))
            {
                m_wrapper = wrapper;

                image.Source = m_defaultImageSource;
                image.Margin = m_defaultImageMargin;

                loadingLabel.Visibility = Visibility.Visible;

                if (m_currentImageStream != null)
                {
                    Log.WriteLine("Disposing current image...", LogType.Debug);

                    m_currentImageStream.Dispose();
                    m_currentImageStream = null;
                }
                else { }

                if (m_wrapper != null && Visibility == Visibility.Visible)
                {
                    Log.WriteLine($"Loading image for {m_wrapper.Card.Id} (Premium={m_wrapper.Card.Premium})", LogType.Debug);

                    m_currentImageStream = (await HearthstoneCardImageManager.GetStreamAsync(m_wrapper.Card.Id, m_wrapper.Card.Premium));

                    if (m_currentImageStream != null)
                    {
                        loadingLabel.Visibility = Visibility.Hidden;

                        if (m_wrapper.Card.Premium)
                        {
                            SetAsGif(m_currentImageStream);
                        }
                        else
                        {
                            image.Source = (Image.FromStream(m_currentImageStream) as Bitmap).ToBitmapImage();
                        }

                        SetMargin();
                    }
                    else { }
                }
                else { }
            }
            else { }
        }
        #endregion

        #region SetMargin
        private void SetMargin()
        {
            if (m_wrapper.DbCard.Type == HearthDb.Enums.CardType.HERO)
            {
                image.Margin = new Thickness(-10, -35, 0, 25);
            }
            else if (!m_wrapper.Card.Premium &&
                    (m_wrapper.DbCard.Id.Equals("CFM_321")
                    || m_wrapper.DbCard.Id.Equals("CFM_619")
                    || m_wrapper.DbCard.Id.Equals("CFM_621")
                    || m_wrapper.DbCard.Id.Equals("CFM_649")
                    || m_wrapper.DbCard.Id.Equals("CFM_685")
                    || m_wrapper.DbCard.Id.Equals("CFM_902")))
            {
                image.Margin = new Thickness(0, 0, 0, -25);
            }
            else if (m_wrapper.Card.Premium)
            {
                if (m_wrapper.DbCard.Type == HearthDb.Enums.CardType.ABILITY && m_wrapper.DbCard.Rarity == HearthDb.Enums.Rarity.LEGENDARY)
                {
                    image.Margin = new Thickness(-15, -30, 0, 0);
                }
                else if (m_wrapper.DbCard.Type == HearthDb.Enums.CardType.ABILITY || m_wrapper.DbCard.Type == HearthDb.Enums.CardType.WEAPON)
                {
                    image.Margin = new Thickness(0, -30, 0, 0);
                }
                else if (m_wrapper.DbCard.Type == HearthDb.Enums.CardType.MINION && m_wrapper.DbCard.Rarity != HearthDb.Enums.Rarity.LEGENDARY)
                {
                    image.Margin = new Thickness(0, -20, -10, 0);
                }
                else
                {
                    image.Margin = new Thickness();
                }
            }
            else if (!m_wrapper.Card.Premium)
            {
                image.Margin = new Thickness(0, -25, 0, 0);
            }
            else
            {
                image.Margin = new Thickness();
            }
        }
        #endregion

        #region SetAsGif
        private void SetAsGif(Stream stream)
        {
            Log.WriteLine("Setting current image as GIF", LogType.Debug);

            image.SetValue(XamlAnimatedGif.AnimationBehavior.SourceStreamProperty, stream);
        }
        #endregion
    }
}