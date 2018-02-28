using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter;
using Hunter.Character;

namespace Interactables
{
    public enum PropType
    {
        Interactable,
        Destructible
    }

    public enum NeedElementType
    {
        ElementRequired,
        NoneRequired
    }
       
    [RequireComponent(typeof(Animator))]
    public class InteractableProp : MonoBehaviour
    {
        [Header("Will you interact with it or attack it")]
        [SerializeField]
        private PropType propType;
        [Header("Type needed to be interacted with")]
        [SerializeField]
        private OPTIONS elementalType;
        [SerializeField]
        private NeedElementType needElement;
        [Header("Number of items to spawn and items to spawn")]
        [SerializeField]
        private List<Interactable> interactable = new List<Interactable>();
        private Animator anim;
        [Header("Name of Animation")]
        [SerializeField]
        private string animationName;
        private float destructionForce = 20;
        private Vector3 destructionDirection; // based on player
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
            if (PropType.Destructible == propType)
            {
                Destruct();
            }
            if (PropType.Interactable == propType)
            {
                ShakeProp();
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
                // have object destroy the direct the player facing
            }


            SpawnItems();

            StartCoroutine(DestroyPieces());

        }

        private void CheckWeaponType(OPTIONS weaponElement)
        {
            if (weaponElement == elementalType)
            {
                Attacked();
            }
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

        private void OnTriggerEnter(Collider other)
        {
            OPTIONS typeFromWeapon;

            if (other.gameObject.GetComponent<Melee>() != null)
            {
                if (needElement == NeedElementType.ElementRequired)
                {
                    typeFromWeapon = other.gameObject.GetComponent<Melee>().elementType;

                    CheckWeaponType(typeFromWeapon);
                }
                else if (needElement == NeedElementType.NoneRequired)
                {
                    Attacked();
                }
            } 
        }

        private void HitByRay(Ray ray)
        {
            

        }

        private void CheckForRay(Ray ray)
        {
            var rangedWeapon = ray.origin;
            Physics.r

        }
    }
}
