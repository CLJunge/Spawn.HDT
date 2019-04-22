#region Using
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.UI.Models;
using System;
using System.Collections.Generic;
#endregion

namespace Spawn.HDT.DustUtility.UI
{
    public class SortOrder
    {
        #region Member Variables
        private readonly List<SortOrderItemModel> m_lstItems;
        #endregion

        #region Properties
        #region Indexer
        public SortOrderItemModel this[int index]
        {
            get => m_lstItems[index];
            set => m_lstItems[index] = value;
        }
        #endregion

        #region Count
        public int Count => m_lstItems.Count;
        #endregion
        #endregion

        #region Ctor
        public SortOrder()
        {
            m_lstItems = new List<SortOrderItemModel>();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'SortOrder' instance");
        }
        #endregion

        #region Static Methods
        #region Parse
        public static SortOrder Parse(string strSortOrderString)
        {
            SortOrder retVal = null;

            if (!string.IsNullOrEmpty(strSortOrderString))
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Parsing '{strSortOrderString}'...");

                retVal = new SortOrder();

                string[] vItems = strSortOrderString.Split(';');

                for (int i = 0; i < vItems.Length; i++)
                    retVal.m_lstItems.Add(new SortOrderItemModel((OrderItem)Enum.Parse(typeof(OrderItem), vItems[i])));
            }

            return retVal;
        }
        #endregion

        #region ItemToString
        public static string ItemToString(OrderItem item)
        {
            string strRet = string.Empty;

            switch (item)
            {
                case OrderItem.Count:
                    strRet = "Count";
                    break;

                case OrderItem.Name:
                    strRet = "Name";
                    break;

                case OrderItem.Golden:
                    strRet = "Golden";
                    break;

                case OrderItem.Dust:
                    strRet = "Dust";
                    break;

                case OrderItem.Rarity:
                    strRet = "Rarity";
                    break;

                case OrderItem.CardClass:
                    strRet = "Class";
                    break;

                case OrderItem.CardSet:
                    strRet = "Set";
                    break;

                case OrderItem.ManaCost:
                    strRet = "Mana";
                    break;
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"ItemToString: OrderItem={item} > Result={strRet}");

            return strRet;
        }
        #endregion
        #endregion

        public enum OrderItem
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