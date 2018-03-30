using UnityEngine;

namespace Hunter
{
    public abstract class InventoryItem : ScriptableObject
    {
        [Header("Name of the item")]
        public string itemName;
        [Header("Inventory item sprite")]
        public Sprite icon;
        [Header("Description of item")]
        [TextArea]
        public string itemDescription;
        [SerializeField]
        private InteractableInventoryItem interactableItemPrefab;
        public InteractableInventoryItem InteractableItemPrefab
        {
            get
            {
                return interactableItemPrefab;
            }
        }
    }
}
