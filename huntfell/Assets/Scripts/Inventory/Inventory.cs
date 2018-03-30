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

        public List<InventoryItem> startingItems;

        private Dictionary<WeaponItem, InteractableInventoryItem> weapons = new Dictionary<WeaponItem, InteractableInventoryItem>();
        private Dictionary<ElementModItem, InteractableInventoryItem> elementMods = new Dictionary<ElementModItem, InteractableInventoryItem>();
        private Dictionary<JournalItem, InteractableInventoryItem> journalEntries = new Dictionary<JournalItem, InteractableInventoryItem>();
        private Dictionary<DiaryItem, InteractableInventoryItem> diaryEntries = new Dictionary<DiaryItem, InteractableInventoryItem>();

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

            if(startingItems != null && startingItems.Count > 0)
            {
                foreach(var item in startingItems)
                {
                    TryAddItem(item);
                }
            }
        }

        //Method for simply giving the player an instance of item data from which we spawn it's interactble prefab too
        public bool TryAddItem(InventoryItem item) 
        {
            if (item is WeaponItem && !weapons.ContainsKey(item as WeaponItem))
            {
                var spawnedItem = SpawnInteractableItem(item);
                weapons.Add(item as WeaponItem, spawnedItem);
            }
            else if (item is ElementModItem && !elementMods.ContainsKey(item as ElementModItem))
            {
                var spawnedItem = SpawnInteractableItem(item);
                elementMods.Add(item as ElementModItem, spawnedItem);
            }
            else if (item is JournalItem && !journalEntries.ContainsKey(item as JournalItem))
            {
                var spawnedItem = SpawnInteractableItem(item);
                journalEntries.Add(item as JournalItem, spawnedItem);
            }
            else if(item is DiaryItem && !diaryEntries.ContainsKey(item as DiaryItem))
            {
                var spawnedItem = SpawnInteractableItem(item);
                diaryEntries.Add(item as DiaryItem, spawnedItem);
            }
            else
            {
                Debug.LogWarning("Tried to add the item to the Inventory but it was not a recognizable item or its already in the Inventory. Check that the Inventory is able to handle that type of item and that it already isnt in the Inventory.");
                return false;
            }
            return true;
        }

        private InteractableInventoryItem SpawnInteractableItem (InventoryItem item)
        {
            if(item.InteractableItemPrefab == null)
            {
                Debug.LogWarning("Couldn't spawn an interactble object from the inventory item data provided. Make sure a prefab reference is set in the scriptable object.");
                return null;
            }
            var spawnedItem = Instantiate(item.InteractableItemPrefab, transform);
            spawnedItem.gameObject.SetActive(false);
            return spawnedItem;
        }

        public bool TryAddItem (InventoryItem item, InteractableInventoryItem spawnedInteractableItem)
        {
            if (item is WeaponItem && !weapons.ContainsKey(item as WeaponItem))
            {
                weapons.Add(item as WeaponItem, spawnedInteractableItem);
            }
            else if (item is ElementModItem && !elementMods.ContainsKey(item as ElementModItem))
            {
                elementMods.Add(item as ElementModItem, spawnedInteractableItem);
            }
            else if (item is JournalItem && !journalEntries.ContainsKey(item as JournalItem))
            {
                journalEntries.Add(item as JournalItem, spawnedInteractableItem);
            }
            else if (item is DiaryItem && !diaryEntries.ContainsKey(item as DiaryItem))
            {
                diaryEntries.Add(item as DiaryItem, spawnedInteractableItem);
            }
            else
            {
                Debug.LogWarning("Tried to add the item to the Inventory but it was not a recognizable item or its already in the Inventory. Check that the Inventory is able to handle that type of item and that it already isnt in the Inventory.");
                return false;
            }
            return true;
        }
    }
}
