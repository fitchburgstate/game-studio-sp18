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

        public List<WeaponItem> weapons = new List<WeaponItem>();
        public List<ElementModItem> elementMods = new List<ElementModItem>();
        public List<JournalItem> journalEntries = new List<JournalItem>();
        public List<DiaryItem> diaryEntries = new List<DiaryItem>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool TryAddItem(InventoryItem item) 
        {
            //So if we have a prefab instance of an interactable items set but no live instance, pre spawn one and set it under the Inventory
            if(item.InteractableItemPrefab != null && item.interactableInventoryItem == null)
            {
                item.interactableInventoryItem = Instantiate(item.InteractableItemPrefab, transform);
                item.interactableInventoryItem.originItem = item;
            }

            if (item is WeaponItem)
            {
                weapons.Add(item as WeaponItem);
            }
            else if (item is ElementModItem)
            {
                elementMods.Add(item as ElementModItem);
            }
            else if (item is JournalItem)
            {
                journalEntries.Add(item as JournalItem);
            }
            else if(item is DiaryItem)
            {
                diaryEntries.Add(item as DiaryItem);
            }
            else
            {
                Debug.LogError("Tried to add the item to the Inventory but it was not a recognizable item. Check that the Inventory is able to handle that type of item.");
                return false;
            }
            return true;
        }
    }
}
