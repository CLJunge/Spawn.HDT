using HearthMirror.Objects;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Spawn.HDT.DustUtility.UI.Windows
{
    public partial class DecksInfoWindow
    {
        #region Member Variables
        private Account m_account;
        #endregion

        #region Ctor
        public DecksInfoWindow()
        {
            InitializeComponent();
        }

        public DecksInfoWindow(Account account)
            : this()
        {
            m_account = account;
        }
        #endregion

        #region Events
        #region OnLoaded
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            List<Deck> lstDecks = m_account.LoadDecks();

            for (int i = 0; i < lstDecks.Count; i++)
            {
                Deck deck = lstDecks[i];

                ListViewDeckItem item = new ListViewDeckItem
                {
                    HeroImage = GetHeroImage(deck),
                    Name = deck.Name,
                    CardCount = $"{deck.GetCardCount()}/30",
                    Cost = $"{deck.GetCraftingCost().ToString()} Dust"
                };

                listView.Items.Add(item);
            }
        }
        #endregion
        #endregion

        #region GetHeroImage
        public ImageSource GetHeroImage(Deck deck)
        {
            string strSource = string.Empty;

            switch (deck.Hero.Substring(0, 7))
            {
                case "HERO_01": //Warrior
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Garrosh.png";
                    break;

                case "HERO_02": //Shaman
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Thrall.png";
                    break;

                case "HERO_03": //Rogue
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Valeera.png";
                    break;

                case "HERO_04": //Paladin
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Uther.png";
                    break;

                case "HERO_05": //Hunter
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Rexxar.png";
                    break;

                case "HERO_06": //Druid
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Malfurion.png";
                    break;

                case "HERO_07": //Warlock
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Gul'dan.png";
                    break;

                case "HERO_08": //Mage
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Jaina.png";
                    break;

                case "HERO_09": //Priest
                    strSource = "/Spawn.HDT.DustUtility;component/Resources/Anduin.png";
                    break;
            }

            return new BitmapImage(new Uri(strSource, UriKind.Relative));
        }
        #endregion
    }
}