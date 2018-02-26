using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Animator))]
    public class InteractableProp : MonoBehaviour
    {
        [Header("Name of Animation")]
        public string animationName;
        [Header("Number of items to spawn and items to spawn")]
        public List<Interactable> interactable = new List<Interactable>();

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
            for (var i = 0; i < interactable.Count; i++)
            {
                Instantiate(interactable[i], transform.position, transform.rotation);
                interactable[i].spawnFromProp = true;
                yield return null;
            }

            interactable.Clear();
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
