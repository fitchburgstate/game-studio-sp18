using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class Inventory : MonoBehaviour
    {
        /// <summary>
        /// singleton
        /// </summary>
        [HideInInspector]
        public static Inventory instance;

        /// <summary>
        /// list of items modifiers 
        /// </summary>
        [HideInInspector]
        public List<Item> items = new List<Item>();
        
        /// <summary>
        /// adds item to list if there is space
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(Item item) 
        {
            items.Add(item);
            return true;
        }

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
