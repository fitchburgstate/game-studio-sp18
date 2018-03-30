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
        /// <summary>
        /// THIS SHOULD NEVER BE MODIFIED, ONLY USE THIS VARIABLE TO INSTANTIATE AN INSTANCE OF THE INTERACTABLE ITEM. Use interactableInventoryItem instead.
        /// </summary>
        [SerializeField]
        private InteractableInventoryItem interactableItemPrefab;
        public InteractableInventoryItem InteractableItemPrefab
        {
            get
            {
                return interactableItemPrefab;
            }
        }

        [HideInInspector]
        public InteractableInventoryItem interactableInventoryItem;
    }
}
