using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Hunter;
using Hunter.Character;


namespace Interactables
{
    public enum PropType
    {
        Interactable,
        Destructible
    }

    [RequireComponent(typeof(Animator))]
    public class InteractableProp : MonoBehaviour, IDamageable
    {
        public Player player;

        [Header("The items that will spawn from the prop")]
        [SerializeField]
        private List<Interactable> interactable = new List<Interactable>(); // list of iteractables to spawn
        [Header("Destructable prop or Prop that Shakes")]
        [SerializeField]
        private PropType propType;
        [Header("Does it need a mod element to be triggered")]
        [SerializeField]
        private bool elementRequired;  
        [Header("Type of element it needs to be triggered")]
        [SerializeField]
        private OPTIONS elementalType;
        [Header("Item needed it prop needs item to activate")]
        [SerializeField]
        private Interactable itemNeeded;
        private Animator anim;
        [Header("Name of Trigger Animation")]
        [SerializeField]
        private string animationName;
        [Header("How much force is use to destroy the object")]
        [SerializeField]
        private float destructionForce = 50;
        private Vector3 destructionDirection; // based on player
        private Rigidbody[] pieces; // the ridgidbodies of the broken pieces of the destructible prop
        [Header("broken prefab if its destructible")]
        [SerializeField]
        private GameObject brokenProp;
        [Header("object and function is this prop activates another")]
        [SerializeField]
        private UnityEvent propEvent;
        private OPTIONS typeFromWeapon; // type attribute of weapon player is holding


        public void TakeDamage(int damage)
        {
            Interact();
        }

        public void Interact()
        {
            NeedElement();
        }

        public void StartEvent()
        {
            propEvent.Invoke();
        }

        private void OnMouseDown()
        {
            Attacked();
        }

        private void Start()
        {
            if (propEvent == null)
            {
                propEvent = new UnityEvent();
            }

            anim = GetComponent<Animator>();
            player = FindObjectOfType<Player>();
        }

        /// <summary>
        /// cheks if prop is interactable or destructible the calls the appropriate function
        /// </summary>
        private void Attacked()
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

        /// <summary>
        /// plays animation based on string name and spawns item
        /// </summary>
        private void ShakeProp()  
        {
            anim.SetTrigger(animationName);
            SpawnItems();
        }

        /// <summary>
        /// Destroys prop
        /// </summary>
        private void Destruct()
        {
            BreakItem();
            DestoryProp();
            DisableProp();
        }

        /// <summary>
        /// spawns the destroyed version of the interactable
        /// </summary>
        private void BreakItem()
        {
            brokenProp = Instantiate(brokenProp, transform.position, transform.rotation);
        }

        /// <summary>
        /// diasable the propComponent mesh and collider
        /// </summary>
        private void DisableProp()
        {
            var mesh = GetComponent<MeshRenderer>().enabled = false;
            var collider = GetComponent<Collider>().enabled = false;
        }

        /// <summary>
        /// makes destroyed pieces non kinematic and add force to them
        /// </summary>
        private void DestoryProp()
        {
            destructionDirection = player.transform.position;
            pieces = brokenProp.GetComponentsInChildren<Rigidbody>();

            for(var i = 0; i < pieces.Length; i++)
            {
                pieces[i].isKinematic = false;
            }

            for (var i = 0; i < pieces.Length; i++)
            {
                pieces[i].AddForce(destructionDirection * destructionForce);
                //might change to general explosion
                // have object destroy the direct the player facing
            }

            SpawnItems();
            StartCoroutine(DestroyPieces());
        }

        /// <summary>
        /// destroyed prop pieces after a certian amount of time
        /// </summary>
        /// <returns></returns>
        private IEnumerator DestroyPieces()
        {
            yield return new WaitForSeconds(4f);

            for (var i = 0; i < pieces.Length; i++)
            {
                Destroy(pieces[i]);
            }

            Destroy(gameObject,2f);
        }

        /// <summary>
        /// spawns item in list then clears them  bug where sometimes is would spawn multiple of the same items
        /// </summary>
        private void SpawnItems() 
        {
            for (var i = 0; i < interactable.Count; i++)
            {
                Instantiate(interactable[i], transform.position, transform.rotation);
                if (PropType.Interactable == propType)
                {
                    interactable[i].spawnedFromProp = true;
                }
                else if (PropType.Destructible == propType)
                {
                    interactable[i].spawnedFromDestruct = true;
                }
            }

            interactable.Clear();
        }

        /// <summary>
        /// checks if element is need or not and gets element from current weapon
        /// </summary>
        private void NeedElement()
        {
            if (elementRequired == true)
            {
                if (player.CurrentMeleeWeapon != null)
                { 
                    typeFromWeapon = player.CurrentMeleeWeapon.elementType;
                }
                if (player.CurrentRangeWeapon != null)
                {
                    typeFromWeapon = player.CurrentRangeWeapon.elementType;
                }
               
                CheckWeaponType(typeFromWeapon);
            }
            else if (elementRequired == false)
            {
                Attacked();
            }          
        }

        /// <summary>
        /// checks if weapon element is the same as the one the prop needs
        /// </summary>
        /// <param name="weaponElement"></param>
        private void CheckWeaponType(OPTIONS weaponElement)
        {
            if (weaponElement == elementalType)
            {
                Attacked();
            }
        }

        /// <summary>
        /// called if prop need and item and checks if it the right item
        /// </summary>
        /// <param name="item"></param>
        private void DropItemInProp(Interactable item)
        {
            if (item.Equals(itemNeeded))
            {
                //delete item from iventory
            }
        }

        /// <summary>
        /// called if prop gives player and item
        /// </summary>
        /// <param name="item"></param>
        private void GiveItemToPlayer(Interactable item)
        {
            item.transform.SetParent(Inventory.instance.transform);
        }
    } 
}
