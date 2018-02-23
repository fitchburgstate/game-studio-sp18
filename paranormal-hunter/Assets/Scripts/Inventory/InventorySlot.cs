using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Interactable
{
    public class InventorySlot : DraggableItem
    { 
        [HideInInspector]
        public Item item;
        /// <summary>
        ///  transform of the inventory
        /// </summary>
        [HideInInspector]
        public Transform inventoryParent;
       
        /// <summary>
        /// if item it in Moddifier slot or not
        /// </summary>
        private bool inModSlot = false;
        private RectTransform rectTransform;
        private Image icon;

        public void Awake()
        {
            icon = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        public new void Start()
        {
            rectTransform.localPosition = new Vector3(0, 0, 0); //it will set it transform to infront of its parents becuase its a child of it
            canvasGroup = GetComponent<CanvasGroup>();
            inventorySlot = GetComponent<InventorySlot>();
            inventoryParent = transform.parent.parent; // sets inventory parent to inventory
            typeOfItem = inventorySlot.item.typeOfItem;  // sets type of item
        }

        public void AddItem(Item newitem) // adds item information to inventory slot and item
        {
            item = newitem;
            icon.sprite = newitem.icon;
        }

        public override void OnEndDrag(PointerEventData eventData) // removes or adds item from inventory if it is added to mod slot or brought back to inventory 
        {
            base.OnEndDrag(eventData);

            if(transform.parent.parent != inventoryParent)
            {
                Inventory.instance.RemoveItem(item);
                inModSlot = true;
            }

            if(inModSlot && transform.parent.parent == inventoryParent)
            {
                inModSlot = false;
                Inventory.instance.AddItem(item);
            } 
        }
    }
}
