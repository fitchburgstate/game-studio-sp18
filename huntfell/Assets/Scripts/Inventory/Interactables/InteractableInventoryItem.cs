using System.Collections;
using UnityEngine;
using Hunter.Characters;

namespace Hunter
{
    public class InteractableInventoryItem : MonoBehaviour, IInteractable
    {
        //All the relevent data about this item
        public InventoryItem itemData;

        public float itemBounceTime = 0.75f;
        public int itemTotalBounces = 3;
        public float itemBounceHeight = 1.25f;
        [SerializeField]
        private AnimationCurve itemHeightCurve;
        public float itemMaxSpawnDistance = 3;

        public string tutorialText;
        public Sprite tutorialIcon;

        private Collider interactableItemCollider;

        public bool IsImportant
        {
            get
            {
                return true;
            }
        }

        private void Awake ()
        {
            interactableItemCollider = GetComponent<Collider>();
        }

        public virtual void FireInteraction (Character characterTriggeringInteraction)
        {
            var player = characterTriggeringInteraction as Player;
            if (player != null && player.Inventory.TryAddItem(itemData, this))
            {
                if(!string.IsNullOrEmpty(tutorialText) && tutorialIcon != null)
                {
                    HUDManager.instance?.ShowTutorialPrompt(tutorialText, tutorialIcon);
                }
            }
        }

        public void SpawnFromProp (Vector3 spawnPosition)
        {
            interactableItemCollider.enabled = false;

            var tempPosition = Vector3.zero;
            if(Utility.RandomNavMeshPoint(spawnPosition, 1, out tempPosition))
            {
                spawnPosition = tempPosition;
            }
            StartCoroutine(PlaySpawnAnimation(spawnPosition)); 
            
        }

        private IEnumerator PlaySpawnAnimation (Vector3 targetPosition)
        {
            for (var currentBounce = 1; currentBounce <= itemTotalBounces; currentBounce++)
            {
                var startingPosition = transform.position;
                var elapsedTime = 0f;
                var adjustedBounceTime = itemBounceTime / currentBounce;

                while (elapsedTime <= adjustedBounceTime)
                {
                    elapsedTime += Time.deltaTime;
                    var percentComplete = Mathf.Clamp01(elapsedTime / adjustedBounceTime);

                    var newY = Mathf.Lerp(targetPosition.y, targetPosition.y + (itemBounceHeight / currentBounce), itemHeightCurve.Evaluate(percentComplete));

                    var adjustedTargetPosition = new Vector3(targetPosition.x, newY, targetPosition.z);

                    transform.position = Vector3.LerpUnclamped(startingPosition, adjustedTargetPosition, percentComplete);
                    yield return null;
                }
            }
            interactableItemCollider.enabled = true;
            yield return null;
        }

        public void SpawnOnGround (Vector3 spawnPosition)
        {
            var targetPosition = transform.position;

            if (Utility.RandomNavMeshPoint(spawnPosition, 1, out targetPosition))
            {
                var groundPosition = new Vector3(spawnPosition.x, targetPosition.y, spawnPosition.z);
                transform.position = groundPosition;
            }
        }
    }
}
