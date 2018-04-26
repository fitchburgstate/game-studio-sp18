using UnityEngine;

namespace Hunter
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Generic Item", order = 0)]
    public class InventoryItem : ScriptableObject
    {
        [Header("Name of the item")]
        public string itemName;
        [Header("Inventory item sprite")]
        public Sprite icon;
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
