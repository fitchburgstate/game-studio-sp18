using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;
using System.Linq;

namespace Hunter
{
    public class PlayerInventory : MonoBehaviour
    {
        public List<InventoryItem> startingItems;

        //TODO Make it so that the InteractableInventoryItem and the seperate actual Weapon are the same thing
        private Dictionary<MeleeWeaponItem, InteractableInventoryItem> meleeWeapons = new Dictionary<MeleeWeaponItem, InteractableInventoryItem>();
        private Dictionary<RangedWeaponItem, InteractableInventoryItem> rangedWeapons = new Dictionary<RangedWeaponItem, InteractableInventoryItem>();
        private Dictionary<ElementModItem, InteractableInventoryItem> elementMods = new Dictionary<ElementModItem, InteractableInventoryItem>();
        private Dictionary<JournalItem, InteractableInventoryItem> journalEntries = new Dictionary<JournalItem, InteractableInventoryItem>();
        private Dictionary<DiaryItem, InteractableInventoryItem> diaryEntries = new Dictionary<DiaryItem, InteractableInventoryItem>();

        public int RangedWeaponIndex { get; private set; } = 0;
        public int RangedElementIndex { get; private set; } = 0;
        public int MeleeWeaponIndex { get; private set; } = 0;
        public int MeleeElementIndex { get; private set; } = 0;

        #region Unity Messages
        private void Awake()
        {
            if(startingItems != null && startingItems.Count > 0)
            {
                foreach(var item in startingItems)
                {
                    TryAddItem(item);
                }
            }
        }
        #endregion

        #region Weapons
        public Weapon CycleWeaponsUp(Weapon currentWeapon, Transform weaponContainer)
        {
            var isMelee = currentWeapon is Melee;
            var isRanged = currentWeapon is Ranged;
            Weapon newWeapon = null;

            //Dont cycle anything if there are no weapons or only 1 weapon in the inventory
            if (isMelee && meleeWeapons.Count < 2)
            {
                Debug.LogWarning("Unable to cycle melee weapons.");
                return null;
            }
            else if(isRanged && rangedWeapons.Count < 2)
            {
                Debug.LogWarning("Unable to cycle ranged weapons.");
                return null;
            }

            if (isMelee)
            {
                MeleeWeaponIndex++;
                if (MeleeWeaponIndex >= meleeWeapons.Count) { MeleeWeaponIndex = 0; }
                newWeapon = GetMeleeWeaponAtIndex(MeleeWeaponIndex, weaponContainer);
            }
            else if (isRanged)
            {
                RangedWeaponIndex++;
                if (RangedWeaponIndex >= rangedWeapons.Count) { RangedWeaponIndex = 0; }
                newWeapon = GetRangedWeaponAtIndex(RangedWeaponIndex, weaponContainer);
            }
            return newWeapon;
        }

        public Weapon CycleWeaponsDown (Weapon currentWeapon, Transform weaponContainer)
        {
            var isMelee = currentWeapon is Melee;
            var isRanged = currentWeapon is Ranged;
            Weapon newWeapon = null;
            //Dont cycle anything if there are no weapons or only 1 weapon in the inventory
            if (isMelee && meleeWeapons.Count < 2)
            {
                Debug.LogWarning("Unable to cycle melee weapons.");
                return null;
            }
            else if (isRanged && rangedWeapons.Count < 2)
            {
                Debug.LogWarning("Unable to cycle ranged weapons.");
                return null;
            }

            if (isMelee)
            {
                MeleeWeaponIndex--;
                if (MeleeWeaponIndex < 0) { MeleeWeaponIndex = meleeWeapons.Count - 1; }
                newWeapon = GetMeleeWeaponAtIndex(MeleeWeaponIndex, weaponContainer);
            }
            else if (isRanged)
            {
                RangedWeaponIndex--;
                if (RangedWeaponIndex < 0) { RangedWeaponIndex = rangedWeapons.Count - 1; }
                newWeapon = GetRangedWeaponAtIndex(RangedWeaponIndex, weaponContainer);
            }
            return newWeapon;
        }

        public Melee GetMeleeWeaponAtIndex (int index, Transform weaponContainer)
        {
            if (meleeWeapons.Count == 0 || meleeWeapons.Count <= index)
            {
                Debug.LogWarning("Unable get melee weapon at index " + index);
                return null;
            }
            var existingWeapons = weaponContainer.GetComponentsInChildren<Melee>(true);
            var meleeItemData = meleeWeapons.Keys.ElementAt(index);
            var meleeWeaponPrefab = meleeItemData.MeleeWeaponPrefab;
            if (HUDManager.instance != null) { HUDManager.instance.UpdateWeaponImage(meleeItemData.icon); }

            foreach (var weapon in existingWeapons)
            {
                if (weapon.name == meleeWeaponPrefab.name)
                {
                    if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(weapon?.WeaponElement?.elementHUDSprite); }
                    return weapon;
                }
            }

            var newMelee = Instantiate(meleeWeaponPrefab, weaponContainer);
            newMelee.name = meleeWeaponPrefab.name;
            if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(newMelee?.WeaponElement?.elementHUDSprite); }
            return newMelee;
        }

        public Ranged GetRangedWeaponAtIndex (int index, Transform weaponContainer)
        {
            if (rangedWeapons.Count == 0 || rangedWeapons.Count <= index)
            {
                Debug.LogWarning("Unable get ranged weapon at index " + index);
                return null;
            }
            var existingWeapons = weaponContainer.GetComponentsInChildren<Ranged>(true);
            var rangedItemData = rangedWeapons.Keys.ElementAt(index);
            var rangedWeaponPrefab = rangedItemData.RangedWeaponPrefab;
            if (HUDManager.instance != null) { HUDManager.instance.UpdateWeaponImage(rangedItemData.icon); }
            foreach (var weapon in existingWeapons)
            {
                if (weapon.name == rangedWeaponPrefab.name)
                {
                    if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(weapon?.WeaponElement?.elementHUDSprite); }
                    return weapon;
                }
            }

            var newRanged = Instantiate(rangedWeaponPrefab, weaponContainer);
            newRanged.name = rangedWeaponPrefab.name;
            if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(newRanged?.WeaponElement?.elementHUDSprite); }
            return newRanged;
        }
        #endregion

        #region Elements
        public Element CycleElementsUp (Weapon currentWeapon)
        {
            if (elementMods.Count < 2)
            {
                Debug.LogWarning("Unable to cycle through element mods.");
                return null;
            }
            var isMelee = currentWeapon is Melee;
            var isRanged = currentWeapon is Ranged;
            Element newElement = null;

            if (isMelee)
            {
                MeleeElementIndex++;
                if (MeleeElementIndex >= elementMods.Count) { MeleeElementIndex = 0; }
                if(MeleeElementIndex != 0 && MeleeElementIndex == RangedElementIndex) { MeleeElementIndex++; }
                newElement = GetElementAtIndex(MeleeElementIndex);
            }
            else if (isRanged)
            {
                RangedElementIndex++;
                if (RangedElementIndex >= elementMods.Count) { RangedElementIndex = 0; }
                //if (RangedElementIndex == MeleeElementIndex) { RangedElementIndex++; }
                newElement = GetElementAtIndex(RangedElementIndex);
            }
            return newElement;
        }

        public Element CycleElementsDown (Weapon currentWeapon)
        {
            if (elementMods.Count < 2)
            {
                Debug.LogWarning("Unable to cycle through element mods.");
                return null;
            }
            var isMelee = currentWeapon is Melee;
            var isRanged = currentWeapon is Ranged;
            Element newElement = null;

            if (isMelee)
            {
                MeleeElementIndex--;
                if (MeleeElementIndex < 0) { MeleeElementIndex = elementMods.Count - 1; }
                if (MeleeElementIndex != 0 && MeleeElementIndex == RangedElementIndex) { MeleeElementIndex--; }
                newElement = GetElementAtIndex(MeleeElementIndex);
            }
            else if (isRanged)
            {
                RangedElementIndex--;
                if (RangedElementIndex < 0) { RangedElementIndex = elementMods.Count - 1; }
                //if (RangedElementIndex == MeleeElementIndex) { RangedElementIndex--; }
                newElement = GetElementAtIndex(RangedElementIndex);
            }
            return newElement;
        }

        public Element GetElementAtIndex (int elementIndex)
        {
            var elementItemData = elementMods.Keys.ElementAt(elementIndex);
            var element = Utility.ElementOptionToElement(elementItemData.elementOption);

            if (element != null)
            {
                element.elementHUDSprite = elementItemData.icon;
            }
            if (HUDManager.instance != null) { HUDManager.instance.UpdateElementImage(element?.elementHUDSprite); }
            Fabric.EventManager.Instance?.PostEvent("UI Navigation Blip");
            return element;
        }
        #endregion

        #region Books
        public List<JournalItem> GetAllJournals ()
        {
            return new List<JournalItem>(journalEntries.Keys);
        }

        public List<DiaryItem> GetAllDiaries ()
        {
            return new List<DiaryItem>(diaryEntries.Keys);
        }
        #endregion

        //Method for simply giving the player an instance of item data from which we spawn it's interactble prefab too
        public bool TryAddItem(InventoryItem item) 
        {
            var spawnedItem = SpawnInteractableItem(item);
            if(!TryAddItem(item, spawnedItem))
            {
                Destroy(spawnedItem);
                return false;
            }
            return true;
        }

        private InteractableInventoryItem SpawnInteractableItem (InventoryItem item)
        {
            if(item == null || item.InteractableItemPrefab == null)
            {
                Debug.LogWarning($"Couldn't spawn an interactble object from the inventory item ({item.itemName} / {item.name}) provided. Make sure a prefab reference is set in the scriptable object.");
                return null;
            }
            return Instantiate(item.InteractableItemPrefab);
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
                Debug.LogWarning($"Tried to add the item ({item.itemName} / {item.name}) to the Inventory but it was not a an item that can be kept in the inventory or it already exists in the inventory.");
                return false;
            }

            Debug.Log($"Added the item ({item.itemName} / {item.name}) to your inventory.");
            if(HUDManager.instance != null) { HUDManager.instance.ShowItemPickupPrompt(item.itemName, item.icon); }

            spawnedInteractableItem?.transform.SetParent(transform);
            spawnedInteractableItem?.gameObject.SetActive(false);

            return true;
        }
    }
}
