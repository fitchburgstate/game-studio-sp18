using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public enum PropType
    {
        Interactable,
        Destructible
    }

    public enum ElementalType
    {
        Silver,
        Fire,
        Ice,
        Lighting
    }
       
    [RequireComponent(typeof(Animator))]
    public class InteractableProp : MonoBehaviour
    {
        [Header("Will you interact with it or attack it")]
        [SerializeField]
        private PropType propType;
        [Header("Type needed to be interacted with")]
        [SerializeField]
        private ElementalType elementalType;
        private ElementalType typeFromWeapon;
        [Header("Number of items to spawn and items to spawn")]
        [SerializeField]
        private List<Interactable> interactable = new List<Interactable>();
        private Animator anim;
        [Header("Name of Animation")]
        [SerializeField]
        private string animationName;
        private float destructionForce = 200;
        private Vector3 destructionDirection;
        [SerializeField]
        private Rigidbody[] pieces;
        [Header("the broken prop")]
        [SerializeField]
        private GameObject brokenProp;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void Attacked()
        {
            if (typeFromWeapon == elementalType)
            {
                if (PropType.Destructible == propType)
                {
                    Destruct();
                }
                if (PropType.Interactable == propType)
                {
                    ShakeProp();
                }
            }
        }

        private void OnMouseDown()
        {
            Attacked();
        }

        private void ShakeProp() // plays animation based on string name and spawns item
        {
            anim.SetTrigger(animationName);
            SpawnItems();
        }

        private void Destruct()
        {
            BreakItem();
            DestoryProp();
            DisableProp();

        }

        private void BreakItem()
        {
            brokenProp = Instantiate(brokenProp, transform.position, transform.rotation);
        }

        public void DisableProp()
        {
            var mesh = GetComponent<MeshRenderer>().enabled = false;
            var collider = GetComponent<Collider>().enabled = false;
        }

        private void DestoryProp()
        {

            pieces = brokenProp.GetComponentsInChildren<Rigidbody>();

            for(var i = 0; i < pieces.Length; i++)
            {
                pieces[i].isKinematic = false;
            }

            for (var i = 0; i < pieces.Length; i++)
            {
                pieces[i].AddForce(transform.forward * destructionForce);
            }


            SpawnItems();

            StartCoroutine(DestroyPieces());

        }

        private void SpawnItems() // spawns item in list then clears them  bug where sometimes is would spawn multiple of the same items
        {
            for (var i = 0; i < interactable.Count; i++)
            {
                Instantiate(interactable[i], transform.position, transform.rotation);
                if (PropType.Interactable == propType)
                {
                    interactable[i].spawnedFromProp = true;
                }
            }

            interactable.Clear();
        }

        private IEnumerator DestroyPieces()
        {
            yield return new WaitForSeconds(12f);

            for(var i = 0; i < pieces.Length; i++)
            {
                Destroy(pieces[i]);
            }

           // Destroy(gameObject,2f);
        }
    }
}
