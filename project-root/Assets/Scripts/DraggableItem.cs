using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactable
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public enum Slot {ITEMTYPE1, ITEMTYPE2, ITEMTYPE3}
        [HideInInspector]
        public Slot typeOfItem;

        [HideInInspector]
        public Transform parentToReturnTo;
        public CanvasGroup canvasGroup;
        public InventorySlot inventorySlot;

        public Transform parentOfParent;

        public void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            inventorySlot = GetComponent<InventorySlot>();

            typeOfItem = inventorySlot.item.typeOfItem;

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            parentToReturnTo = transform.parent;
            canvasGroup.blocksRaycasts = false;
            parentOfParent = transform.parent.parent;
            transform.SetParent(parentOfParent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(parentToReturnTo);
            transform.position = parentToReturnTo.position;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
