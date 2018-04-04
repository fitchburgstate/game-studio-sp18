using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;
using System.Linq;

namespace Hunter
{
    public class InventoryManager : MonoBehaviour
    {
        /// <summary>
        /// singleton
        /// </summary>
        [HideInInspector]
        public static InventoryManager instance;

        public List<InventoryItem> startingItems;

        //TODO Make it so that the InteractableInventoryItem and the seperate actual Weapon are the same thing
        private Dictionary<MeleeWeaponItem, InteractableInventoryItem> meleeWeapons = new Dictionary<MeleeWeaponItem, InteractableInventoryItem>();
        private Dictionary<RangedWeaponItem, InteractableInventoryItem> rangedWeapons = new Dictionary<RangedWeaponItem, InteractableInventoryItem>();
        private Dictionary<ElementModItem, InteractableInventoryItem> elementMods = new Dictionary<ElementModItem, InteractableInventoryItem>();
        private Dictionary<JournalItem, InteractableInventoryItem> journalEntries = new Dictionary<JournalItem, InteractableInventoryItem>();
        private Dictionary<DiaryItem, InteractableInventoryItem> diaryEntries = new Dictionary<DiaryItem, InteractableInventoryItem>();

        private int rangedWeaponIndex = 0;
        private int meleeWeaponIndex = 0;
        private int elementIndex = 0;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
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

        public Range CycleRangedWeapons (Transform weaponContainer)
        {
            //Handles the iterator for cycling through the weapons
            if(rangedWeapons.Count == 0)
            {
                Debug.LogWarning("There are no ranged weapons in your inventory!");
                return null;
            }
            rangedWeaponIndex++;
            if(rangedWeaponIndex >= rangedWeapons.Count) { rangedWeaponIndex = 0; }

            //Checks to see if there is already an instance of the weapon underneath the player's weapon container and if so returns that weapon, otherwise spawn one
            var existingWeapons = weaponContainer.GetComponentsInChildren<Range>(true);
            var rangedItemData = rangedWeapons.Keys.ElementAt(rangedWeaponIndex);
            var rangedWeaponPrefab = rangedItemData.RangedWeaponPrefab;
            if(HUDManager.instance != null) { HUDManager.instance.UpdateWeaponImage(rangedItemData.icon); }
            foreach (var weapon in existingWeapons)
            {
                if(weapon.name == rangedWeaponPrefab.name) {
                    if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(weapon?.weaponElement?.elementHUDSprite); }
                    return weapon;
                }
            }

            var newRanged = Instantiate(rangedWeaponPrefab, weaponContainer);
            newRanged.name = rangedWeaponPrefab.name;
            if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(newRanged?.weaponElement?.elementHUDSprite); }
            return newRanged;
        }

        public Melee CycleMeleeWeapons (Transform weaponContainer)
        {
            //Handles the iterator for cycling through the weapons
            if (meleeWeapons.Count == 0)
            {
                Debug.LogWarning("There are no melee weapons in your inventory!");
                return null;
            }
            meleeWeaponIndex++;
            if (meleeWeaponIndex >= meleeWeapons.Count) { meleeWeaponIndex = 0; }

            //Checks to see if there is already an instance of the weapon underneath the player's weapon container and if so returns that weapon, otherwise spawn one
            var existingWeapons = weaponContainer.GetComponentsInChildren<Melee>(true);
            var meleeItemData = meleeWeapons.Keys.ElementAt(meleeWeaponIndex);
            var meleeWeaponPrefab = meleeItemData.MeleeWeaponPrefab;
            if (HUDManager.instance != null) { HUDManager.instance.UpdateWeaponImage(meleeItemData.icon); }
            foreach (var weapon in existingWeapons)
            {
                if (weapon.name == meleeWeaponPrefab.name) {
                    if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(weapon?.weaponElement?.elementHUDSprite); }
                    return weapon;
                }
            }

            var newMelee = Instantiate(meleeWeapons.Keys.ElementAt(meleeWeaponIndex).MeleeWeaponPrefab, weaponContainer);
            newMelee.name = meleeWeaponPrefab.name;
            if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(newMelee?.weaponElement?.elementHUDSprite); }
            return newMelee;
        }

        public Element CycleElementsUp ()
        {
            if (elementMods.Count == 0)
            {
                Debug.LogWarning("There are no element mods in your inventory!");
                return null;
            }
            elementIndex++;
            if (elementIndex >= elementMods.Count) { elementIndex = 0; }

            return GetElementAtIndex(elementIndex);
        }

        private Element GetElementAtIndex (int elementIndex)
        {
            var elementItemData = elementMods.Keys.ElementAt(elementIndex);
            var element = Utility.ElementOptionToElement(elementItemData.elementOption);

            if (element != null)
            {
                element.elementHUDSprite = elementItemData.icon;
            }
            if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(element?.elementHUDSprite); }
            return element;
        }

        public Element CycleElementsDown ()
        {
            if (elementMods.Count == 0)
            {
                Debug.LogWarning("There are no element mods in your inventory!");
                return null;
            }
            elementIndex--;
            if (elementIndex < 0) { elementIndex = elementMods.Count - 1; }

            return GetElementAtIndex(elementIndex);
        }

        //Method for simply giving the player an instance of item data from which we spawn it's interactble prefab too
        public bool TryAddItem(InventoryItem item) 
        {
            var spawnedItem = SpawnInteractableItem(item);
            return TryAddItem(item, spawnedItem);
        }

        private InteractableInventoryItem SpawnInteractableItem (InventoryItem item)
        {
            if(item == null || item.InteractableItemPrefab == null)
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
            if (item is RangedWeaponItem && !rangedWeapons.ContainsKey(item as RangedWeaponItem))
            {
                rangedWeapons.Add(item as RangedWeaponItem, spawnedInteractableItem);
            }
            else if (item is MeleeWeaponItem && !meleeWeapons.ContainsKey(item as MeleeWeaponItem))
            {
                meleeWeapons.Add(item as MeleeWeaponItem, spawnedInteractableItem);
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
            Debug.Log($"Added the Item {item.itemName} to your inventory.");
            if(HUDManager.instance != null) { HUDManager.instance.ShowItemPickupPrompt(item.itemName); }
            return true;
        }
    }
}
