using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactable
{
    public class ItemZone : MonoBehaviour, IDropHandler, IPointerEnterHandler
    {
        public Slot typeOfItem;

        public void OnDrop(PointerEventData eventData) // When a draggableItem is dropped on this slot 
        {   
            var draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();

            if (draggableItem != null)  // make sure there is a draggable item
            {
                if (typeOfItem == draggableItem.typeOfItem) // makes sure that it's of the right type
                {
                    if (transform.childCount == 0) // makes sure there isnt already an item in the slot
                    {
                        draggableItem.parentToReturnTo = transform; // adds it to that slot
                    }
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData) 
        {
            if(eventData.pointerDrag == null) // makes sure there is data 
            {
                return;
            }

            var draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>(); // same code as the onDrop Function

            if(draggableItem != null) 
            {
                if (typeOfItem == draggableItem.typeOfItem)
                {
                    if (transform.childCount == 0)
                    {
                        draggableItem.parentToReturnTo = transform;
                    }
                }
            }
        }
    }
}
