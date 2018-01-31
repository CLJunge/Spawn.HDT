#region Using
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
#endregion

namespace Spawn.HDT.DustUtility.UI
{
    public class SortOrder
    {
        #region Properties
        #region Items
        public ObservableCollection<ItemContainer> Items { get; set; }
        #endregion
        #endregion

        #region Ctor
        public SortOrder()
        {
            Items = new ObservableCollection<ItemContainer>();
        }
        #endregion

        #region Static Methods
        #region Parse
        public static SortOrder Parse(string strSortOrderString)
        {
            SortOrder retVal = null;

            if (!string.IsNullOrEmpty(strSortOrderString))
            {
                retVal = new SortOrder();

                string[] vItems = strSortOrderString.Split(';');

                for (int i = 0; i < vItems.Length; i++)
                {
                    Item item = (Item)Enum.Parse(typeof(Item), vItems[i]);

                    retVal.Items.Add(new ItemContainer(item));
                }
            }
            else { }

            return retVal;
        }
        #endregion

        #region ItemToString
        public static string ItemToString(Item item)
        {
            string strRet = string.Empty;

            switch (item)
            {
                case Item.Count:
                    strRet = "Count";
                    break;

                case Item.Name:
                    strRet = "Name";
                    break;

                case Item.Golden:
                    strRet = "Golden";
                    break;

                case Item.Dust:
                    strRet = "Dust";
                    break;

                case Item.Rarity:
                    strRet = "Rarity";
                    break;

                case Item.CardClass:
                    strRet = "Class";
                    break;

                case Item.CardSet:
                    strRet = "Set";
                    break;

                case Item.ManaCost:
                    strRet = "Mana";
                    break;

                default:
                    break;
            }

            return strRet;
        }
        #endregion
        #endregion

        [DebuggerDisplay("Name={Name} Value={Value}")]
        public class ItemContainer
        {
            #region Properties
            #region Name
            public string Name { get; set; }
            #endregion

            #region Value
            public Item Value { get; set; }
            #endregion
            #endregion

            #region Ctor
            public ItemContainer(Item item)
            {
                Name = ItemToString(item);
                Value = item;
            }
            #endregion
        }

        public enum Item
        {
            Count,
            Name,
            Golden,
            Dust,
            Rarity,
            CardClass,
            CardSet,
            ManaCost
        }
    }
}