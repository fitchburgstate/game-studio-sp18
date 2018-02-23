using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactable
{
    public enum Slot // Type of item so object can only go to specific item slots 
    {
        ITEMTYPE1,
        ITEMTYPE2,
        ITEMTYPE3
    } 

    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {      
        [HideInInspector]
        public Slot typeOfItem;
        [HideInInspector]
        public Transform parentToReturnTo; // transform the object gets parented so it knows what parent to return to OnEndDrag
        [HideInInspector]
        public CanvasGroup canvasGroup;
        [HideInInspector]
        public InventorySlot inventorySlot;
        [HideInInspector]
        public Transform parentOfParent; // the parent of the parent of this object

        public void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            inventorySlot = GetComponent<InventorySlot>();
            typeOfItem = inventorySlot.item.typeOfItem; // sets type of item to a type so an type of item can be drop on a item zone
        }

        public void OnBeginDrag(PointerEventData eventData) // When object begins to be draged it gets the parent of the object
        {
            parentToReturnTo = transform.parent;
            canvasGroup.blocksRaycasts = false; // unblocks ray cast so it detect what under the mouse
            parentOfParent = transform.parent.parent;
            transform.SetParent(parentOfParent); // moves the object out of it current parent into the inventory when moving the object around
        }

        public void OnDrag(PointerEventData eventData) // sets position of object to mouse position
        {
            transform.position = eventData.position;
        }

        public virtual void OnEndDrag(PointerEventData eventData) // when the object is let go it goes to last possible parent it touched 
        {
            transform.SetParent(parentToReturnTo); // moves the object into parent
            transform.position = parentToReturnTo.position;
            canvasGroup.blocksRaycasts = true; //reblocks it so i can pick it up again later
        }
    }
}
