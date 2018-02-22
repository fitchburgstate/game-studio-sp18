using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Interactable
{
    public class InventorySlot : DraggableItem
    {
        
        [HideInInspector]
        public Item item;

        public Transform inventoryParent;
        public Transform currentTransform;
        public bool inModSlot = false;

        private RectTransform rectTransform;
        private Image icon;

        public void Awake()
        {
            icon = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        public new void Start()
        {
            rectTransform.localPosition = new Vector3(0, 0, 0);
            canvasGroup = GetComponent<CanvasGroup>();
            inventorySlot = GetComponent<InventorySlot>();
            inventoryParent = transform.parent.parent;
            typeOfItem = inventorySlot.item.typeOfItem;
        }

        public void AddItem(Item newitem)
        {
            item = newitem;
            icon.sprite = newitem.icon;
        }

        public override void OnEndDrag(PointerEventData eventData)
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
