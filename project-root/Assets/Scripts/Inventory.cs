using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class Inventory : MonoBehaviour
    {
        [HideInInspector]
        public List<Item> items = new List<Item>();
        [HideInInspector]
        public static Inventory instance;
        [Header("How much space in the inventory")]
        public int inventorySpace;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

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

        public bool RemoveItem(Item item)
        {
            items.Remove(item);
            return true;
        }

    }
}
