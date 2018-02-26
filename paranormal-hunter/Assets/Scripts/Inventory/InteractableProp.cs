using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public enum PropType
    {
        Interactable,
        Destructable
    }
       
    [RequireComponent(typeof(Animator))]
    public class InteractableProp : MonoBehaviour
    {
        [Header("Will you interact with it or attack it")]
        [SerializeField]
        private PropType propType;
        [Header("Number of items to spawn and items to spawn")]
        [SerializeField]
        private List<Interactable> interactable = new List<Interactable>();
        private Animator anim;
        [Header("Name of Animation")]
        [SerializeField]
        private string animationName;
        private float destructionForce = 2000;
        private Vector3 destructionDirection;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void Attacked()
        {          
            if (PropType.Destructable == propType)
            {
                // destruction math
                DestoryProp();
            }
        }

        public void Interact()
        {
            if (PropType.Interactable == propType)
            {
                ShakeProp();
                StartCoroutine(SpawnItems());
            }
        }

        private void OnMouseDown()
        {
            Interact();
        }

        private void ShakeProp() // plays animation based on string name and spawns item
        {
            anim.SetTrigger(animationName);
            StartCoroutine(SpawnItems());
        }

        private void DestoryProp()
        {
            var pieces = GetComponentsInChildren<Rigidbody>();

            for(var i = 0; i < pieces.Length; i++)
            {
                pieces[i].isKinematic = false;
            }

            for (var i = 0; i < pieces.Length; i++)
            {
                pieces[i].AddForce(destructionDirection * destructionForce);
            }
            
        }

        private IEnumerator SpawnItems() // spawns item in list then clears them  bug where sometimes is would spawn multiple of the same items
        {
            for (var i = 0; i < interactable.Count; i++)
            {
                Instantiate(interactable[i], transform.position, transform.rotation);
                if (PropType.Interactable == propType)
                {
                    interactable[i].spawnFromProp = true;
                }
                yield return null;
            }

            interactable.Clear();
        }
    }
}
