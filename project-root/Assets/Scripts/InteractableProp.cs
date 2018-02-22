using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    [RequireComponent(typeof(Animator))]
    public class InteractableProp : MonoBehaviour
    {
        [Header("Name of Animation")]
        public string animationName;
        [Header("Number of items to spawn and items to spawn")]
        public List<InteractableObject> interactableObject = new List<InteractableObject>();

        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void OnMouseDown()
        {
            Interact();
        }

        public void Interact()
        {
            ShakeObject();
            StartCoroutine(SpawnItems());
        }

        private void ShakeObject()
        {
            anim.SetTrigger(animationName);
        }

        private IEnumerator SpawnItems()
        {
            for (var i = 0; i < interactableObject.Count; i++)
            {
                Instantiate(interactableObject[i], transform.position, transform.rotation);
                yield return null;
            }
            interactableObject.Clear();
        }

    }
}
