using Hearthstone_Deck_Tracker;
using System;
using System.Drawing;

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public class HearthstoneInfo
    {
        public HearthstoneInfo()
        {
            HsHandle = User32.GetHearthstoneWindow();
            HsRect = User32.GetHearthstoneRect(false);
            Ratio = (4.0 / 3.0) / ((double)HsRect.Width / HsRect.Height);
            SearchBoxPos = new Point((int)(Hearthstone_Deck_Tracker.Helper.GetScaledXPos(DisenchantConfig.Instance.SearchBoxX, HsRect.Width, Ratio)), (int)(DisenchantConfig.Instance.SearchBoxY * HsRect.Height));
            CardPosX = Hearthstone_Deck_Tracker.Helper.GetScaledXPos(DisenchantConfig.Instance.Card1X, HsRect.Width, Ratio);
            Card2PosX = Hearthstone_Deck_Tracker.Helper.GetScaledXPos(DisenchantConfig.Instance.Card2X, HsRect.Width, Ratio);
            CardPosY = DisenchantConfig.Instance.CardsY * HsRect.Height;
        }

        public IntPtr HsHandle { get; set; }
        public Rectangle HsRect { get; set; }
        public double Ratio { get; set; }
        public Point SearchBoxPos { get; set; }
        public double CardPosX { get; set; }
        public double Card2PosX { get; set; }
        public double CardPosY { get; set; }
    }
}