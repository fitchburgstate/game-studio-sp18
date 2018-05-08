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
        //private Dictionary<RangedWeaponItem, InteractableInventoryItem> rangedWeapons = new Dictionary<RangedWeaponItem, InteractableInventoryItem>();
        private Dictionary<ElementModItem, InteractableInventoryItem> elementMods = new Dictionary<ElementModItem, InteractableInventoryItem>();
        private Dictionary<JournalItem, InteractableInventoryItem> journalEntries = new Dictionary<JournalItem, InteractableInventoryItem>();
        private Dictionary<DiaryItem, InteractableInventoryItem> diaryEntries = new Dictionary<DiaryItem, InteractableInventoryItem>();
        private Dictionary<BestiaryItem, InteractableInventoryItem> bestiaryEntries = new Dictionary<BestiaryItem, InteractableInventoryItem>();
        private Dictionary<MapItem, InteractableInventoryItem> mapEntries = new Dictionary<MapItem, InteractableInventoryItem>();

        private Dictionary<Weapon, int> weaponAndElementIndex = new Dictionary<Weapon, int>();

        //public int RangedWeaponIndex { get; private set; } = 0;
        //public int RangedElementIndex { get; private set; } = 0;
        public int MeleeWeaponIndex { get; private set; } = 0;
        public int MeleeElementIndex { get; private set; } = 0;

        public void AddStartingItems ()
        {
            if (startingItems != null && startingItems.Count > 0)
            {
                foreach (var item in startingItems)
                {
                    TryAddItem(item);
                }
            }
        }

        #region Weapons
        public Weapon CycleWeaponsUp(Weapon currentWeapon, Transform weaponContainer)
        {
            Weapon newWeapon = null;

            //Dont cycle anything if there are no weapons in the inventory
            if (meleeWeapons.Count < 1)
            {
                Debug.LogWarning("Unable to cycle melee weapons.");
                return null;
            }

            MeleeWeaponIndex++;
            if (MeleeWeaponIndex >= meleeWeapons.Count) { MeleeWeaponIndex = 0; }
            newWeapon = GetMeleeWeaponAtIndex(MeleeWeaponIndex, weaponContainer);
            
            return newWeapon;
        }

        public Weapon CycleWeaponsDown (Weapon currentWeapon, Transform weaponContainer)
        {
            Weapon newWeapon = null;
            //Dont cycle anything if there are no weapons in the inventory
            if (meleeWeapons.Count < 1)
            {
                Debug.LogWarning("Unable to cycle melee weapons.");
                return null;
            }

            MeleeWeaponIndex--;
            if (MeleeWeaponIndex < 0) { MeleeWeaponIndex = meleeWeapons.Count - 1; }
            newWeapon = GetMeleeWeaponAtIndex(MeleeWeaponIndex, weaponContainer);
            
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

            Melee resultMelee = null;

            foreach (var weapon in existingWeapons)
            {
                if (weapon.name == meleeWeaponPrefab.name)
                {
                    resultMelee = weapon;
                    break;
                }
            }

            if(resultMelee == null)
            {
                resultMelee = Instantiate(meleeWeaponPrefab, weaponContainer);
                resultMelee.name = meleeWeaponPrefab.name;
            }

            int elementIndex = 0;
            if (GetIndexOfElement(resultMelee.WeaponElement, out elementIndex))
            {
                if (!weaponAndElementIndex.ContainsKey(resultMelee))
                {
                    weaponAndElementIndex.Add(resultMelee, elementIndex);
                }

                MeleeElementIndex = elementIndex;

                if (HUDManager.instance != null)
                {
                    var indexList = weaponAndElementIndex.Values.ToList();
                    indexList.Remove(elementIndex);
                    HUDManager.instance.DimElementSockets(indexList);
                    HUDManager.instance.MoveWeaponWheel(index);
                    HUDManager.instance.MoveElementWheel(elementIndex);
                }
            }

            return resultMelee;
        }

        #endregion

        #region Elements
        public Element CycleElementsUp (Weapon currentWeapon)
        {
            if (elementMods.Count < 2 || currentWeapon == null)
            {
                Debug.LogWarning("Unable to cycle through element mods.");
                return null;
            }
            Element newElement = null;

            for (int i = MeleeElementIndex + 1; i != MeleeElementIndex; i++)
            {
                if (i >= elementMods.Count) { i = 0; }
                if (weaponAndElementIndex.ContainsValue(i) && i != 0) { continue; }
                MeleeElementIndex = i;
                newElement = GetElementAtIndex(MeleeElementIndex, currentWeapon);
                break;
            }
            
            return newElement;
        }

        public Element CycleElementsDown (Weapon currentWeapon)
        {
            if (elementMods.Count < 2 || currentWeapon == null)
            {
                Debug.LogWarning("Unable to cycle through element mods.");
                return null;
            }

            Element newElement = null;

            for(int i = MeleeElementIndex -1; i != MeleeElementIndex; i--)
            {
                if (i < 0) { i = elementMods.Count - 1; }
                if (weaponAndElementIndex.ContainsValue(i) && i != 0) { continue; }
                MeleeElementIndex = i;
                newElement = GetElementAtIndex(MeleeElementIndex, currentWeapon);
                break;
            }

            return newElement;
        }

        public Element GetElementAtIndex (int elementIndex, Weapon currentWeapon)
        {
            var elementItemData = elementMods.Keys.ElementAt(elementIndex);
            var element = Utility.ElementOptionToElement(elementItemData.elementOption);

            if (element != null)
            {
                element.elementHUDSprite = elementItemData.icon;
            }

            weaponAndElementIndex[currentWeapon] = elementIndex;

            if (HUDManager.instance != null) { HUDManager.instance.MoveElementWheel(elementIndex); }
            return element;
        }

        public bool GetIndexOfElement(Element element, out int index)
        {
            var elementOption = Utility.ElementToElementOption(element);
            index = 0;
            foreach(var pair in elementMods)
            {
                if(pair.Key.elementOption == elementOption)
                {
                    return true;
                }
                index++;
            }
            return false;
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
            if (item is MeleeWeaponItem && !meleeWeapons.ContainsKey(item as MeleeWeaponItem))
            {
                meleeWeapons.Add(item as MeleeWeaponItem, spawnedInteractableItem);
                HUDManager.instance?.AddNewWeaponToSocket(item.icon);
                //Jank but w/e
                if(meleeWeapons.Count == 1) { GetComponent<Player>()?.CycleWeapons(true); }
            }
            else if (item is ElementModItem && !elementMods.ContainsKey(item as ElementModItem))
            {
                elementMods.Add(item as ElementModItem, spawnedInteractableItem);
                HUDManager.instance?.AddNewElementToSocket(item.icon);
            }
            else if (item is JournalItem && !journalEntries.ContainsKey(item as JournalItem))
            {
                journalEntries.Add(item as JournalItem, spawnedInteractableItem);
            }
            else if (item is DiaryItem && !diaryEntries.ContainsKey(item as DiaryItem))
            {
                diaryEntries.Add(item as DiaryItem, spawnedInteractableItem);
            }
            else if(item is BestiaryItem && !bestiaryEntries.ContainsKey(item as BestiaryItem))
            {
                bestiaryEntries.Add(item as BestiaryItem, spawnedInteractableItem);
            }
            else if(item is MapItem && !mapEntries.ContainsKey(item as MapItem))
            {
                mapEntries.Add(item as MapItem, spawnedInteractableItem);
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
