using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interactable
{
    public class DisplayInventory : MonoBehaviour
    {
        /// <summary>
        /// creates a new variable of type Inventory
        /// </summary>
        public Inventory inventory;
        /// <summary>
        ///  list of inventory slots
        /// </summary>
        public InventorySlot[] slots;

        /// <summary>
        ///  the parent of all the possible slots
        /// </summary>
        [SerializeField]
        [Header("Parent of all Slots")]
        private Transform slotParent;
        /// <summary>
        /// a list of all the slots items can go in to from the inventory
        /// </summary>
        [SerializeField]
        [Header("Inventroy Slots")]
        private List<Transform> slotChildParent = new List<Transform>(); 
       
        private void Awake()
        {
            inventory = Inventory.instance;
        }

        private void OnEnable()
        {
            UpdateUI(); 
        }

        private void UpdateUI() // updates the ui when enablled
        {
            for (var i = 0; i<inventory.items.Count; i++) // runs through a list of slots based on how many items in the inventory 
            {
                if (slotChildParent[i].childCount < 1) // if there is no items in that slot add new items to that slot
                {
                    var newItemObject = new GameObject(); // creates newItem and add the necessary components
                    newItemObject.AddComponent<Image>();
                    newItemObject.AddComponent<InventorySlot>();
                    newItemObject.AddComponent<CanvasGroup>();
                    newItemObject.transform.SetParent(slotChildParent[i]); //sets newItem parent to slot
                }
            }

            slots = slotParent.GetComponentsInChildren<InventorySlot>(); // adds the compoent to the list of slots

            // runs though a list of slot at add until it at where slots is greture then items
            for (var i = 0; i<slots.Length; i++) 
            {
                if ( i < inventory.items.Count)
                {
                    slots[i].AddItem(inventory.items[i]); // adds inventory item to the inventroy slot
                }
            }
        }
    }
}

