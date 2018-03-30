using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Hunter.Character;

namespace Hunter
{
    public enum PropType
    {
        Interactable,
        Destructible
    }

    [RequireComponent(typeof(Animator))]
    public class InteractableProp : MonoBehaviour, IDamageable
    {
        [HideInInspector]
        public InteractableInventoryItem itemGiven; // the object the player gave

        private Player player;

        [Header("The items that will spawn from the prop")]
        [SerializeField]
        private List<InteractableInventoryItem> interactable = new List<InteractableInventoryItem>(); // list of iteractables to spawn

        [Header("Destructable prop or Prop that Shakes")]
        [SerializeField]
        private PropType propType;

        [Header("Does it need a mod element to be triggered")]
        [SerializeField]
        private bool elementRequired;

        [Header("Type of element it needs to be triggered")]
        [SerializeField]
        private Element elementalType; 

        [Header("Does it spawn a item when interacted with")]
        [SerializeField]
        private bool spawnItem;

        //[Header("Does the player have to drop a item in")]
        //[SerializeField]
        //private bool dropItemIn;
        [Header("Does this prop give a player a item")]
        [SerializeField]
        private bool givePlayerItem;

        [Header("Does this prop activate another")]
        [SerializeField]
        private bool activateAnotherProp;

        //[Header("Item needed it prop needs item to activate")]
        //[SerializeField]
        //private InteractableInventoryItem itemNeeded;
        [Header("Item to give player if it gives item")]
        [SerializeField]
        private InteractableInventoryItem itemToGive;

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

        public void TakeDamage(int damage, bool isCritical, Element weaponElement)
        {
            Interact(weaponElement);
        }

        public void Interact(Element elementFromInteraction)
        {
            if (player.CurrentWeapon != null)
            {
                if (CheckWeaponType(elementFromInteraction) || elementRequired == false)
                {
                    Attacked();
                }
            }
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
        /// which interaction(s) will the prop use
        /// </summary>
        private void ChooseInteraction()
        {
            //if (dropItemIn == true)
            //{
            //    DropItemInProp(itemGiven);
            //}
            if (givePlayerItem == true)
            {
                GiveItemToPlayer(itemToGive);
            }
            if(spawnItem == true)
            {
                SpawnItems();
            }
            if (activateAnotherProp == true)
            {
                StartEvent();
            }
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
            else if (PropType.Interactable == propType)
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
            ChooseInteraction();
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
            // disadbles because object is still doing calculations in corutine but the option shouldnt be seen or interacted on, is disabled in corutine
        }

        /// <summary>
        /// makes destroyed pieces non kinematic and add force to them
        /// </summary>
        private void DestoryProp()
        {
            destructionDirection = -player.transform.position;
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

            ChooseInteraction();
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

            gameObject.SetActive(false);
        }

        /// <summary>
        /// spawns item in list then clears them  bug where sometimes is would spawn multiple of the same items
        /// </summary>
        private void SpawnItems() 
        {
            for (var i = 0; i < interactable.Count; i++)
            {
                var go = Instantiate(interactable[i], transform.position, transform.rotation);
                if (PropType.Interactable == propType)
                {
                    go.SpawnFromProp();
                }
                else if (PropType.Destructible == propType)
                {
                    go.SpawnOnGround();
                }
            }

            interactable.Clear();
        }

        /// <summary>
        /// checks if weapon element is the same as the one the prop needs
        /// </summary>
        /// <param name="weaponElement"></param>
        private bool CheckWeaponType(Element elementFromInteraction)
        {
            return elementFromInteraction.GetType() == elementalType.GetType();
        }

        /// <summary>
        /// called if prop need and item and checks if it the right item
        /// </summary>
        /// <param name="item"></param>
        //private void DropItemInProp(InteractableInventoryItem item)
        //{
        //    if (item.Equals(itemNeeded))
        //    {
        //        //delete item from iventory
        //        //does something
        //    }
        //}

        /// <summary>
        /// called if prop gives player and item
        /// </summary>
        /// <param name="item"></param>
        private void GiveItemToPlayer(InteractableInventoryItem item)
        {
            item.transform.SetParent(Inventory.instance.transform);
            Inventory.instance.AddItem(item);
        }
    } 
}
