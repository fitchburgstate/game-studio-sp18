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

    public class InteractableProp : MonoBehaviour, IDamageable, IInteractable
    {
        [Header("Interaction Options")]
        [Tooltip("The items that will spawn from the prop or be given to the player. If this is left empty the prop will simply shake in place.")]
        [SerializeField]
        private List<InventoryItem> itemsToSpawn = new List<InventoryItem>();

        [Tooltip("Instead of the item being dropped on the ground, should this item be automatically given to the player instead?")]
        [SerializeField]
        private bool giveItemsDirectly;

        [Tooltip("Type of element it needs to be triggered. If this is set to None any kind of interaction will trigger this prop.")]
        [SerializeField]
        private ElementOption elementTypeForInteraction;

        public bool overrideImportance;
        public string interactionSuccessMessage;
        public string interactionFailMessage;

        [Header("Destructible Options")]
        [Tooltip("Are you able to destroy this prop?")]
        [SerializeField]
        private PropType propType = 0;

        [Tooltip("If the object is destructable, how much force should be applied to the broken pieces.")]
        [SerializeField, Range(0, 50)]
        private float destructionForce = 10;

        [Tooltip("The prefab to instant")]
        [SerializeField]
        private GameObject brokenPropPrefab;

        [Header("Event Options")]
        [Tooltip("When this prop is interacted with, should it call a function on another object?")]
        [SerializeField]
        private UnityEvent propEvent;

        private bool currentlyInteracting = false;
        private IEnumerator spawnAction;

        public bool IsImportant
        {
            get
            {
                return overrideImportance || itemsToSpawn.Count > 0 || elementTypeForInteraction != ElementOption.None;
            }
        }

        public float CurrentHealth
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        public float TargetHealth {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        public void Damage (int damage, bool isCritical, Weapon weaponAttackedWith)
        {
            FireInteraction(weaponAttackedWith.characterHoldingWeapon, weaponAttackedWith);
        }

        public void Damage (int damage, bool isCritical, Element damageElement)
        {
            // We dont want interactions to trigger when hit by non-weapon damage such as AOE effects
            return;
        }

        public void Heal (int restore, bool isCritical)
        {
            // We dont want props to heal lol
            return;
        }

        public void Kill ()
        {
            //no murdering props pls
            return;
        }

        // What should the prop do when it is interacted with through the interact input
        public void FireInteraction (Character characterFromInteraction)
        {
            if (currentlyInteracting || characterFromInteraction.tag != "Player") { return; }
            //If there is an element required you have to hit it with your weapon
            if (elementTypeForInteraction == ElementOption.None && propType == PropType.Interactable)
            {
                currentlyInteracting = true;
                ShakeProp();
                ShowSuccessMessage();
                ExecutePropInteractions(characterFromInteraction);
            }
            else
            {
                ShowFailMessage();
            }
        }

        // What should the prop do when it is attacked, considering elemental types as well since elements are equipped to weapons
        private void FireInteraction (Character characterWhoAttacked, Weapon weaponAttackedWith)
        {
            if (currentlyInteracting || characterWhoAttacked.tag != "Player") { return; }

            Fabric.EventManager.Instance?.PostEvent("Player Sword Hit", gameObject);

            var weaponElementOption = Utility.ElementToElementOption(weaponAttackedWith.WeaponElement);
            if (elementTypeForInteraction == ElementOption.None || weaponElementOption == elementTypeForInteraction)
            {
                currentlyInteracting = true;
                ShowSuccessMessage();

                ExecutePropInteractions(characterWhoAttacked);
                switch (propType)
                {
                    case PropType.Destructible:
                        DestructProp(characterWhoAttacked.RotationTransform.forward);
                        //With destructible objects, on trigger exit doesnt get called when you swap out the prop for its destroyed counterpart so we have to manually remove it from the players nearby items list
                        if(characterWhoAttacked is Player) { (characterWhoAttacked as Player).RemoveNearbyInteractable(this); }
                        break;
                    default:
                        ShakeProp();
                        break;
                }
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

            while (elapsed < duration || spawnAction != null)
            {
                elapsed += Time.deltaTime;

                var percentComplete = Mathf.Clamp01(elapsed / duration);
                //var damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
                var damper = 0.5f -percentComplete;
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
            var spawnedBrokenProp = Instantiate(brokenPropPrefab, transform.position, transform.rotation);
            SendBrokenPropFlying(spawnedBrokenProp, forceDirection);
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
                if (itemsToSpawn.Count > 0)
                {
                    spawnAction = SpawnInteractableItems(characterFromInteraction);
                    StartCoroutine(spawnAction);
                }
            }

            if (propEvent != null)
            {
                propEvent.Invoke();
            }
        }

        private IEnumerator SpawnInteractableItems (Character characterFromInteraction)
        {
            foreach (var item in itemsToSpawn)
            {
                var spawnedItem = Instantiate(item.InteractableItemPrefab, transform.position, transform.rotation);

                switch (propType)
                {
                    case PropType.Interactable:
                        spawnedItem.SpawnFromProp(characterFromInteraction.transform.position);
                        yield return new WaitForSeconds(0.25f);
                        break;
                    default:
                        spawnedItem.SpawnOnGround(transform.position);
                        break;
                }
            }

            itemsToSpawn.Clear();
            spawnAction = null;
            yield return null;
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
            foreach(var piece in pieces)
            {
                piece.isKinematic = false;
                piece.AddForce(forceDirection * destructionForce, ForceMode.VelocityChange);
            }
        }

        private void ShowSuccessMessage ()
        {
            if (HUDManager.instance != null && !string.IsNullOrEmpty(interactionSuccessMessage)) { HUDManager.instance.ShowHintPrompt(interactionSuccessMessage); }
        }

        private void ShowFailMessage ()
        {
            if (HUDManager.instance != null && !string.IsNullOrEmpty(interactionFailMessage)) { HUDManager.instance.ShowHintPrompt(interactionFailMessage); }
        }
    }
}
