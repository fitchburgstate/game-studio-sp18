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

        public void Interact()
        {
            ShakeObject(); 
            StartCoroutine(SpawnItems());
        }

        private void ShakeObject() // plays animation based on string name
        {
            anim.SetTrigger(animationName);
        }

        private IEnumerator SpawnItems() // spawns item in list then clears them  bug where sometimes is would spawn multiple of the same items
        {
            for (var i = 0; i < interactableObject.Count; i++)
            {
                Instantiate(interactableObject[i], transform.position, transform.rotation);
                yield return null;
            }

            interactableObject.Clear();
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void OnMouseDown()
        {
            Interact();
        }
    }
}
