using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactable
{
    public class ItemZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public DraggableItem.Slot typeOfItem;

        public void OnDrop(PointerEventData eventData)
        {   
            var draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
            if (draggableItem != null)
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(eventData.pointerDrag == null)
            {
                return;
            }
            var draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();

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

        public void OnPointerExit(PointerEventData eventData)
        {

        }

    }
}
