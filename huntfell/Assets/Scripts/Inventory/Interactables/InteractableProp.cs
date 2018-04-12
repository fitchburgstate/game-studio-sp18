using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Hunter.Characters;

namespace Hunter
{
    public enum PropType
    {
        Interactable,
        Destructible
    }

    [RequireComponent(typeof(Animator))]
    public class InteractableProp : MonoBehaviour, IDamageable, IInteractable
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

        public string interactionSuccessMessage;
        public string interactionFailMessage;

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

        private bool currentlyInteracting = false;

        // private Animator anim;

        private void Start ()
        {
            // anim = GetComponent<Animator>();
        }

        public void TakeDamage (int damage, bool isCritical, Weapon weaponAttackedWith)
        {
            FireInteraction(weaponAttackedWith.characterHoldingWeapon, weaponAttackedWith);
        }

        // What should the prop do when it is interacted with, also checks to see if there are any elemental constraints
        public void FireInteraction (Character characterFromInteraction)
        {
            if (currentlyInteracting || characterFromInteraction.tag != "Player") { return; }

            if (elementTypeForInteraction == ElementOption.None || propType != PropType.Destructible)
            {
                currentlyInteracting = true;

                ShowSuccessMessage();
                ShakeProp();
                ExecutePropInteractions(characterFromInteraction);
            }
            else
            {
                ShowFailMessage();
            }
        }

        private void FireInteraction (Character characterWhoAttacked, Weapon weaponAttackedWith)
        {
            if (currentlyInteracting || characterWhoAttacked.tag != "Player") { return; }

            Fabric.EventManager.Instance.PostEvent("Player Sword Hit", gameObject);

            var weaponElementOption = Utility.ElementToElementOption(weaponAttackedWith.weaponElement);
            if (elementTypeForInteraction == ElementOption.None || weaponElementOption == elementTypeForInteraction)
            {
                currentlyInteracting = true;
                ShowSuccessMessage();

                switch (propType)
                {
                    case PropType.Destructible:
                        DestructProp(characterWhoAttacked.RotationTransform.forward);
                        break;
                    default:
                        ShakeProp();
                        break;
                }
                ExecutePropInteractions(characterWhoAttacked);
            }
            else
            {
                ShowFailMessage();
            }
        }

        private void ShakeProp ()
        {
            StartCoroutine(ShakeGameObject(0.25f, 0.2f));
        }

        private IEnumerator ShakeGameObject (float duration, float magnitude)
        {
            var elapsed = 0.0f;
            var originalObjectPos = gameObject.transform.localPosition;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                var percentComplete = elapsed / duration;
                var damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
                var x = (Random.value * 2.0f - 1.0f);
                var y = (Random.value * 2.0f - 1.0f);
                x *= magnitude * damper;
                y *= magnitude * damper;

                gameObject.transform.localPosition = new Vector3(originalObjectPos.x + x, originalObjectPos.y + y, originalObjectPos.z);
                yield return null;
            }

            gameObject.transform.localPosition = originalObjectPos;
            yield return null;
            currentlyInteracting = false;
        }

        private void DestructProp (Vector3 forceDirection)
        {
            // Disable the regular prop and swap in the broken prefab
            // GetComponent<MeshRenderer>().enabled = false;
            // GetComponent<Collider>().enabled = false;
            gameObject.SetActive(false);
            brokenPropPrefab = Instantiate(brokenPropPrefab, transform.position, transform.rotation);
            //SendBrokenPropFlying(brokenPropPrefab, forceDirection);
        }

        // Everything to do with Prop Interaction as far as items and firing events on other gameObjects
        private void ExecutePropInteractions (Character characterFromInteraction)
        {
            if (giveItemsDirectly)
            {
                GiveItemsDirectly(characterFromInteraction);
            }
            else
            {
                SpawnInteractableItems();
            }

            if (propEvent != null)
            {
                propEvent.Invoke();
            }
        }

        private void SpawnInteractableItems ()
        {
            if (itemsToSpawn.Count == 0)
            {
                //Debug.Log("Nothing in this prop!");
                return;
            }

            foreach (var item in itemsToSpawn)
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

        private void GiveItemsDirectly (Character characterFromInteraction)
        {
            if(!(characterFromInteraction is Player)) { return; }

            foreach (var item in itemsToSpawn)
            {
                (characterFromInteraction as Player).Inventory.TryAddItem(item);
            }
        }

        // Destructible Prop Pieces Handeling, this should probably be moves to a seperate component that is put on the broken prop prefab
        private void SendBrokenPropFlying (GameObject brokenProp, Vector3 forceDirection)
        {
            var pieces = brokenProp.GetComponentsInChildren<Rigidbody>();

            for (var i = 0; i < pieces.Length; i++)
            {
                pieces[i].isKinematic = false;
            }

            for (var i = 0; i < pieces.Length; i++)
            {
                pieces[i].AddForce(forceDirection * destructionForce);
                // might change to general explosion
                // have object destroy the direct the player facing
            }

            StartCoroutine(DestroyPieces(pieces));
        }

        private IEnumerator DestroyPieces (Rigidbody[] pieces)
        {
            yield return new WaitForSeconds(4f);

            for (var i = 0; i < pieces.Length; i++)
            {
                Destroy(pieces[i].gameObject);
            }
            gameObject.SetActive(false);
        }

        private void ShowSuccessMessage ()
        {
            if (HUDManager.instance != null && !string.IsNullOrEmpty(interactionSuccessMessage)) { HUDManager.instance.ShowPrompt(interactionSuccessMessage); }
        }

        private void ShowFailMessage ()
        {
            if (HUDManager.instance != null && !string.IsNullOrEmpty(interactionFailMessage)) { HUDManager.instance.ShowPrompt(interactionFailMessage); }
        }

        public bool IsImportant ()
        {
            // Important props are props that require elemental interactions or props that have items inside them
            return itemsToSpawn.Count > 0 || elementTypeForInteraction != ElementOption.None;
        }
    }
}
