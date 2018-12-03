#region Using
using HearthDb.Enums;
using Spawn.HDT.DustUtility.Logging;
using System.Collections.Generic;
using static HearthDb.CardIds.Collectible;
#endregion

namespace Spawn.HDT.DustUtility.Hearthstone
{
    public static class CardSets
    {
        #region Properties
        public static Dictionary<CardSet, string> All { get; }
        public static Dictionary<CardSet, string> AllFullName { get; }
        public static Dictionary<CardSet, string> AllDisplayName { get; }
        public static Dictionary<CardSet, string> AllShortName { get; }
        public static Dictionary<CardSet, string> Expansions { get; }
        public static Dictionary<CardSet, string> Adventures { get; }
        public static Dictionary<CardSet, string> Standard { get; }
        public static List<string> NonCraftableCardIds { get; }
        #endregion

        #region Ctor
        static CardSets()
        {
            All = new Dictionary<CardSet, string>
            {
                { CardSet.EXPERT1, "Classic" },
                { CardSet.GVG, "Goblins vs Gnomes" },
                { CardSet.TGT, "Grand Tournament" },
                { CardSet.OG, "Old Gods" },
                { CardSet.GANGS, "Gadgetzan" },
                { CardSet.UNGORO, "Un'Goro" },
                { CardSet.ICECROWN, "Frozen Throne" },
                { CardSet.LOOTAPALOOZA, "Kobolds" },
                { CardSet.GILNEAS, "Witchwood" },
                { CardSet.BOOMSDAY, "Boomsday" },
                { CardSet.TROLL, "Rastakhan" },
                { CardSet.NAXX, "Naxxramas" },
                { CardSet.BRM, "Blackrock Mountain" },
                { CardSet.LOE, "League of Explorers" },
                { CardSet.KARA, "Karazhan" },
                { CardSet.HOF, "Hall of Fame" }
            };

            AllFullName = new Dictionary<CardSet, string>
            {
                { CardSet.EXPERT1, "Classic" },
                { CardSet.GVG, "Goblins vs Gnomes" },
                { CardSet.TGT, "The Grand Tournament" },
                { CardSet.OG, "Whispers of the Old Gods" },
                { CardSet.GANGS, "Mean Streets of Gadgetzan" },
                { CardSet.UNGORO, "Journey to Un'Goro" },
                { CardSet.ICECROWN, "Knights of the Frozen Throne" },
                { CardSet.LOOTAPALOOZA, "Kobolds and Catacombs" },
                { CardSet.GILNEAS, "The Witchwood" },
                { CardSet.BOOMSDAY, "The Boomsday Project" },
                { CardSet.TROLL, "Rastakhan Rumble" },
                { CardSet.NAXX, "Curse of Naxxramas" },
                { CardSet.BRM, "Blackrock Mountain" },
                { CardSet.LOE, "The League of Explorers" },
                { CardSet.KARA, "One Night in Karazhan" },
                { CardSet.HOF, "Hall of Fame" }
            };

            AllDisplayName = new Dictionary<CardSet, string>
            {
                { CardSet.EXPERT1, "Classic" },
                { CardSet.GVG, "GvG" },
                { CardSet.TGT, "TGT" },
                { CardSet.OG, "Old Gods" },
                { CardSet.GANGS, "MSG" },
                { CardSet.UNGORO, "Un'goro" },
                { CardSet.ICECROWN, "KFT" },
                { CardSet.LOOTAPALOOZA, "Kobolds" },
                { CardSet.GILNEAS, "Witchwood" },
                { CardSet.BOOMSDAY, "Boomsday" },
                { CardSet.TROLL, "Rastakhan" },
                { CardSet.NAXX, "Naxx" },
                { CardSet.BRM, "BRM" },
                { CardSet.LOE, "LOE" },
                { CardSet.KARA, "Karazhan" },
                { CardSet.HOF, "Hall Of Fame" }
            };

            AllShortName = new Dictionary<CardSet, string>
            {
                { CardSet.EXPERT1, "Expert" },
                { CardSet.GVG, "Goblins" },
                { CardSet.TGT, "Tournament" },
                { CardSet.OG, "OldGods" },
                { CardSet.GANGS, "Gadgetzan" },
                { CardSet.UNGORO, "Ungoro" },
                { CardSet.ICECROWN, "FrozenThrone" },
                { CardSet.LOOTAPALOOZA, "Kobolds" },
                { CardSet.GILNEAS, "Witchwood" },
                { CardSet.BOOMSDAY, "Boomsday" },
                { CardSet.TROLL, "Rastakhan" },
                { CardSet.NAXX, "Naxx" },
                { CardSet.BRM, "Mountain" },
                { CardSet.LOE, "League" },
                { CardSet.KARA, "Karazhan" },
                { CardSet.HOF, "Hall" }
            };

            Expansions = new Dictionary<CardSet, string>
            {
                { CardSet.EXPERT1, "Classic" },
                { CardSet.GVG, "Goblins vs Gnomes" },
                { CardSet.TGT, "Grand Tournament" },
                { CardSet.OG, "Old Gods" },
                { CardSet.GANGS, "Gadgetzan" },
                { CardSet.UNGORO, "Un'Goro" },
                { CardSet.ICECROWN, "Frozen Throne" },
                { CardSet.LOOTAPALOOZA, "Kobolds" },
                { CardSet.GILNEAS, "Witchwood" },
                { CardSet.BOOMSDAY, "Boomsday" },
                { CardSet.TROLL, "Rastakhan" }
            };

            Adventures = new Dictionary<CardSet, string>
            {
                { CardSet.NAXX, "Naxxramas" },
                { CardSet.BRM, "Blackrock Mountain" },
                { CardSet.LOE, "League of Explorers" },
                { CardSet.KARA, "Karazhan" }
            };

            Standard = new Dictionary<CardSet, string>
            {
                { CardSet.EXPERT1, "Classic" },
                { CardSet.UNGORO, "Un'Goro" },
                { CardSet.ICECROWN, "Frozen Throne" },
                { CardSet.LOOTAPALOOZA, "Kobolds" },
                { CardSet.GILNEAS, "Witchwood" },
                { CardSet.BOOMSDAY, "Boomsday" },
                { CardSet.TROLL, "Rastakhan" }
            };

            NonCraftableCardIds = new List<string>
            {
                Neutral.Cthun,
                Neutral.BeckonerOfEvil,
                Neutral.MarinTheFox
            };

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Initialized 'CardSets'");
        }
        #endregion

        public class CardSetItem
        {
            #region Properties
            #region Name
            public string Name { get; set; }
            #endregion

            #region Value
            public CardSet Value { get; set; }
            #endregion
            #endregion
        }

        public static class Info
        {
            #region Properties
            public static InfoItem Expert { get; }
            public static InfoItem Goblins { get; }
            public static InfoItem Tournament { get; }
            public static InfoItem OldGods { get; }
            public static InfoItem Gadgetzan { get; }
            public static InfoItem Ungoro { get; }
            public static InfoItem FrozenThrone { get; }
            public static InfoItem Kobolds { get; }
            public static InfoItem Witchwood { get; }
            public static InfoItem Boomsday { get; }
            public static InfoItem Rastakhan { get; }
            public static InfoItem Naxx { get; }
            public static InfoItem Mountain { get; }
            public static InfoItem League { get; }
            public static InfoItem Karazhan { get; }
            public static InfoItem Hall { get; }

            public static Dictionary<CardSet, InfoItem> Dictionary { get; }
            #endregion

            #region Ctor
            static Info()
            {
                Expert = new InfoItem(182, 160, 74, 31);
                Goblins = new InfoItem(80, 74, 52, 20);
                Tournament = new InfoItem(98, 72, 54, 20);
                OldGods = new InfoItem(100, 72, 54, 21);
                Gadgetzan = new InfoItem(98, 72, 54, 20);
                Ungoro = new InfoItem(98, 72, 54, 23);
                FrozenThrone = new InfoItem(98, 72, 54, 23);
                Kobolds = new InfoItem(98, 72, 54, 23);
                Witchwood = new InfoItem(98, 72, 54, 23);
                Boomsday = new InfoItem(98, 72, 54, 23);
                Rastakhan = new InfoItem(98, 72, 54, 23);
                Naxx = new InfoItem(36, 8, 4, 6);
                Mountain = new InfoItem(30, 22, 0, 5);
                League = new InfoItem(50, 26, 4, 5);
                Karazhan = new InfoItem(54, 24, 2, 5);
                Hall = new InfoItem(6, 2, 2, 5);

                Dictionary = new Dictionary<CardSet, InfoItem>
                {
                    { CardSet.HOF, Hall },
                    { CardSet.EXPERT1, Expert },
                    { CardSet.GVG, Goblins },
                    { CardSet.TGT, Tournament },
                    { CardSet.OG, OldGods },
                    { CardSet.GANGS, Gadgetzan },
                    { CardSet.UNGORO, Ungoro },
                    { CardSet.ICECROWN, FrozenThrone },
                    { CardSet.LOOTAPALOOZA, Kobolds },
                    { CardSet.GILNEAS, Witchwood },
                    { CardSet.BOOMSDAY, Boomsday },
                    { CardSet.TROLL, Rastakhan },
                    { CardSet.NAXX, Naxx },
                    { CardSet.BRM, Mountain },
                    { CardSet.LOE, League },
                    { CardSet.KARA, Karazhan }
                };

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Initialized 'CardSets.Info'");
            }
            #endregion

            public class InfoItem
            {
                #region Properties
                public int TotalCount => MaxCommonsCount + MaxRaresCount + MaxEpicsCount + MaxLegendariesCount;
                public int MaxCommonsCount { get; private set; }
                public int MaxRaresCount { get; private set; }
                public int MaxEpicsCount { get; private set; }
                public int MaxLegendariesCount { get; private set; }
                #endregion

                #region Ctor
                public InfoItem(int maxCommonsCount, int maxRaresCount, int maxEpicsCount, int maxLegendariesCount)
                {
                    MaxCommonsCount = maxCommonsCount;
                    MaxRaresCount = maxRaresCount;
                    MaxEpicsCount = maxEpicsCount;
                    MaxLegendariesCount = maxLegendariesCount;

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'CardSets.Info.InfoItem' instance");
                }
                #endregion
            }
        }
    }
}