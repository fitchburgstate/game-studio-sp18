using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class Inventory : MonoBehaviour
    {
        /// <summary>
        /// singleton
        /// </summary>
        [HideInInspector]
        public static Inventory instance;

       // inventory lists for each type of inventroy item
        [HideInInspector]
        public List<InteractableInventoryItem> weapons = new List<InteractableInventoryItem>();
        [HideInInspector]
        public List<InteractableInventoryItem> elementMods = new List<InteractableInventoryItem>();
        [HideInInspector]
        public List<InteractableInventoryItem> journalEntries = new List<InteractableInventoryItem>();
       
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        /// <summary>
        /// adds object to list based of type of item
        /// </summary>
        /// <param name="interactable"></param>
        /// <returns></returns>
        public bool AddItem(InteractableInventoryItem interactable) 
        {
            interactable.transform.SetParent(transform);

            if (interactable.item.itemType == ItemType.Weapons)
            {
                weapons.Add(interactable);
            }
            else if (interactable.item.itemType == ItemType.ElementalMods)
            {
                elementMods.Add(interactable);
            }
            else if (interactable.item.itemType == ItemType.JournalEntries)
            {
                journalEntries.Add(interactable);
            }
            return true;
        }
    }
}
