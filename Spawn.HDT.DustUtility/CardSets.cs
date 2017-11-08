using HearthDb.Enums;
using System.Collections.Generic;

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
        #endregion

        #region Ctor
        static CardSets()
        {
            All = new Dictionary<CardSet, string>
            {
                { CardSet.HOF, "Hall of Fame" },
                { CardSet.EXPERT1, "Classic" },
                { CardSet.GVG, "Goblins vs Gnomes" },
                { CardSet.TGT, "Grand Tournament" },
                { CardSet.OG, "Old Gods" },
                { CardSet.GANGS, "Gadgetzan" },
                { CardSet.UNGORO, "Un'Goro" },
                { CardSet.ICECROWN, "Frozen Throne" },
                { CardSet.LOOTAPALOOZA, "Kobolds" },
                { CardSet.NAXX, "Naxxramas" },
                { CardSet.BRM, "Blackrock Mountain" },
                { CardSet.LOE, "League of Explorers" },
                { CardSet.KARA, "Karazhan" }
            };

            AllFullName = new Dictionary<CardSet, string>
            {
                { CardSet.HOF, "Hall of Fame" },
                { CardSet.EXPERT1, "Classic" },
                { CardSet.GVG, "Goblins vs Gnomes" },
                { CardSet.TGT, "The Grand Tournament" },
                { CardSet.OG, "Whispers of the Old Gods" },
                { CardSet.GANGS, "Mean Streets of Gadgetzan" },
                { CardSet.UNGORO, "Journey to Un'Goro" },
                { CardSet.ICECROWN, "Knights of the Frozen Throne" },
                { CardSet.LOOTAPALOOZA, "Kobolds and Catacombs" },
                { CardSet.NAXX, "Curse of Naxxramas" },
                { CardSet.BRM, "Blackrock Mountain" },
                { CardSet.LOE, "The League of Explorers" },
                { CardSet.KARA, "One Night in Karazhan" }
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
                { CardSet.LOOTAPALOOZA, "Kobolds" }
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
                { CardSet.OG, "Old Gods" },
                { CardSet.GANGS, "Gadgetzan" },
                { CardSet.UNGORO, "Un'Goro" },
                { CardSet.ICECROWN, "Frozen Throne" },
                { CardSet.LOOTAPALOOZA, "Kobolds" },
                { CardSet.KARA, "Karazhan" }
            };
        }
        #endregion

        public class CardSetItem
        {
            public string Name { get; set; }
            public CardSet Value { get; set; }
        }
    }
}