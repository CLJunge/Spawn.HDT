using HearthDb.Enums;
using System.Collections.Generic;
using static HearthDb.CardIds.Collectible;

namespace Spawn.HDT.DustUtility
{
    public static class CardSets
    {
        #region Properties
        public static Dictionary<CardSet, string> All { get; }
        public static Dictionary<CardSet, string> AllFullName { get; }
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
                { CardSet.NAXX, "Curse of Naxxramas" },
                { CardSet.BRM, "Blackrock Mountain" },
                { CardSet.LOE, "The League of Explorers" },
                { CardSet.KARA, "One Night in Karazhan" },
                { CardSet.HOF, "Hall of Fame" }
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
                { CardSet.GILNEAS, "Witchwood" }
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
                { CardSet.ICECROWN, "Frozen Throne" },
                { CardSet.LOOTAPALOOZA, "Kobolds" },
                { CardSet.KARA, "Karazhan" },
                { CardSet.GILNEAS, "Witchwood" }
            };

            NonCraftableCardIds = new List<string>
            {
                Neutral.Cthun,
                Neutral.BeckonerOfEvil,
                Neutral.MarinTheFox
            };
        }
        #endregion

        public class CardSetItem
        {
            public string Name { get; set; }
            public CardSet Value { get; set; }
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
                    { CardSet.NAXX, Naxx },
                    { CardSet.BRM, Mountain },
                    { CardSet.LOE, League },
                    { CardSet.KARA, Karazhan }
                };
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
                }
                #endregion
            }
        }
    }
}