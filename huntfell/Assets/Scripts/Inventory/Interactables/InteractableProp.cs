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
        [Header("Interaction Options")]
        [Tooltip("The items that will spawn from the prop or be given to the player. If this is left empty the prop will simply shake in place.")]
        [SerializeField]
        private List<InventoryItem> itemsToSpawn = new List<InventoryItem>();


        [Tooltip("Type of element it needs to be triggered. If this is set to None any kind of interaction will trigger this prop.")]
        [SerializeField]
        private ElementOption elementTypeForInteraction; 

        [Tooltip("Name of the animation clip that the prop should play when it is interacted with.")]
        [SerializeField]
        private string animationName;

        [Tooltip("Instead of the item being dropped on the ground, should this item be automatically given to the player instead?")]
        [SerializeField]
        private bool giveItemsDirectly;

        [Header("Destructible Options")]
        [Tooltip("Are you able to destroy this prop?")]
        [SerializeField]
        private PropType propType = 0;

        [Tooltip("If the object is destructable, how much force should be applied to the broken pieces.")]
        [SerializeField]
        private float destructionForce = 50;

        [Tooltip("The prefab to instant")]
        [SerializeField]
        private GameObject brokenPropPrefab;

        [Header("Event Options")]
        [Tooltip("When this prop is interacted with, should it call a function on another object?")]
        [SerializeField]
        private UnityEvent propEvent;

        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void TakeDamage(int damage, bool isCritical, Weapon weaponAttackedWith)
        {
            Interact(weaponAttackedWith);
        }

        //What should the prop do when it is interacted with, also checks to see if there are any elemental constraints
        public void Interact(Weapon weaponAttackedWith)
        {
            var weaponElementOption = Utility.ElementToElementOption(weaponAttackedWith.weaponElement);
            if (elementTypeForInteraction == ElementOption.None ||  weaponElementOption == elementTypeForInteraction)
            {
                switch (propType)
                {
                    case PropType.Destructible:
                        DestructProp(weaponAttackedWith.characterHoldingWeapon.RotationTransform.forward);
                        break;
                    default:
                        ShakeProp();
                        break;
                }
            }
        }

        private void ShakeProp()  
        {
            anim.SetTrigger(animationName);
            ExecutePropInteractions();
        }

        private void DestructProp(Vector3 forceDirection)
        {
            //Disable the regular prop and swap in the broken prefab
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            brokenPropPrefab = Instantiate(brokenPropPrefab, transform.position, transform.rotation);

            ExecutePropInteractions();

            SendBrokenPropFlying(brokenPropPrefab, forceDirection);
        }

        //Everything to do with Prop Interaction as far as items and firing events on other gameObjects
        private void ExecutePropInteractions()
        {
            if (giveItemsDirectly)
            {
                GiveItemsDirectly();
            }
            else
            {
                SpawnItems();
            }

            if (propEvent != null)
            {
                StartEvent();
            }
        }

        private void SpawnItems() 
        {
            if(itemsToSpawn.Count == 0) {
                Debug.Log("Nothing in this prop!");
                return;
            }

            foreach(var item in itemsToSpawn)
            {
                var spawnedItem = Instantiate(item.InteractableItemPrefab, transform.position, transform.rotation);

                switch (propType)
                {
                    case PropType.Interactable:
                        spawnedItem.SpawnFromProp();
                        break;
                    default:
                        spawnedItem.SpawnOnGround();
                        break;
                }
            }

            itemsToSpawn.Clear();
        }

        private void GiveItemsDirectly ()
        {
            foreach (var item in itemsToSpawn)
            {
                //item.transform.SetParent(Inventory.instance.transform);
                Inventory.instance.TryAddItem(item);
            }
        }

        public void StartEvent()
        {
            propEvent.Invoke();
        }
        
        //Destructible Prop Pieces Handeling, this should probably be moves to a seperate component that is put on the broken prop prefab
        private void SendBrokenPropFlying(GameObject brokenProp, Vector3 forceDirection)
        {
            var pieces = brokenProp.GetComponentsInChildren<Rigidbody>();

            for(var i = 0; i < pieces.Length; i++)
            {
                pieces[i].isKinematic = false;
            }

            for (var i = 0; i < pieces.Length; i++)
            {
                pieces[i].AddForce(forceDirection * destructionForce);
                //might change to general explosion
                // have object destroy the direct the player facing
            }

            StartCoroutine(DestroyPieces(pieces));
        }

        private IEnumerator DestroyPieces(Rigidbody[] pieces)
        {
            yield return new WaitForSeconds(4f);

            for (var i = 0; i < pieces.Length; i++)
            {
                Destroy(pieces[i].gameObject);
            }
            gameObject.SetActive(false);
        }
    } 
}
