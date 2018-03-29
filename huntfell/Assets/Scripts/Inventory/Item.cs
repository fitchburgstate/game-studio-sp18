using UnityEngine;

namespace Interactables
{
    public enum ItemType
    {
        Weapons,
        ElementalMods,
        JournalEntries
    }

    [CreateAssetMenu(menuName ="InventroyItem")]
    public class Item : ScriptableObject
    {
        [Header("Type of Item")]
        public ItemType itemType;
        [Header("Name of the item")]
        public string itemName;
        [Header("Inventory item sprite")]
        public Sprite icon;
        [Header("Description of item")]
        [TextArea]
        public string itemDescription;
    }
}
