using UnityEngine;

namespace Interactable
{
    [CreateAssetMenu(menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        [Header("Name of the item")]
        public string itemName;
        [Header("Description of item")]
        [TextArea]
        public string itemDescription;
        [Header("InventoryItem")]
        public Sprite icon;
        [Header("Type of ITem")]
        public DraggableItem.Slot typeOfItem;
    }
}
