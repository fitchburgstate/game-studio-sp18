using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interactable
{
    public class DisplayInventory : MonoBehaviour
    {
        [SerializeField]
        [Header("Parent of all Slots")]
        private Transform slotParent;
        [SerializeField]
        [Header("Inventroy Slots")]
        private List<Transform> slotChildParent = new List<Transform>();
        public Inventory inventory;
        public InventorySlot[] slots;

        private void Awake()
        {
            inventory = Inventory.instance;
        }

        private void OnEnable()
        {
            UpdateUI();
        }

        private void ExitInventroy()
        {
            gameObject.SetActive(false);
        }

        private void UpdateUI()
        {
            for (var i = 0; i<inventory.items.Count; i++)
            {
                if (slotChildParent[i].childCount < 1)
                {
                    var newItemObject = new GameObject();
                    newItemObject.AddComponent<Image>();
                    newItemObject.AddComponent<InventorySlot>();
                    newItemObject.AddComponent<CanvasGroup>();
                    newItemObject.transform.SetParent(slotChildParent[i]);
                }
            }

            slots = slotParent.GetComponentsInChildren<InventorySlot>();

            for (var i = 0; i<slots.Length; i++)
            {
                if ( i < inventory.items.Count)
                {
                    slots[i].AddItem(inventory.items[i]);
                }
            }
        }
    }
}

