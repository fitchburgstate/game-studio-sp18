using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class Inventory : MonoBehaviour
    {
        /// <summary>
        /// list of items modifiers 
        /// </summary>
        [HideInInspector]
        public List<Item> items = new List<Item>();
        /// <summary>
        /// singleton
        /// </summary>
        [HideInInspector]
        public static Inventory instance;
        [Header("How much space in the inventory")]
        public int inventorySpace;

        /// <summary>
        /// adds item to list if there is space
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(Item item) 
        {
            if (items.Count >= inventorySpace)
            {
                return false;
            }
            else
            {
                items.Add(item);
            }
    
            return true;
        }

        /// <summary>
        /// remove items from list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(Item item) 
        {
            items.Remove(item);
            return true;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

    }
}
