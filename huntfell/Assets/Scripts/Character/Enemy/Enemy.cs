using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Hunter.Characters
{
    public abstract class Enemy : Character
    {
        public Element elementType;
        public ElementOption enemyElementOption;
        public int invincibilityFrames = 5;

        [SerializeField]
        private List<InventoryItem> itemsToSpawn = new List<InventoryItem>();

        protected override void Start ()
        {
            base.Start();
            elementType = Utility.ElementOptionToElement(enemyElementOption);
        }

        protected override IEnumerator SubtractHealthFromCharacter (int damage, bool isCritical)
        {
            Fabric.EventManager.Instance?.PostEvent("Player Sword Hit", gameObject);
            yield return base.SubtractHealthFromCharacter(damage, isCritical);
            StartCoroutine(InvincibilityFrames());
        }

        protected IEnumerator InvincibilityFrames ()
        {
            invincible = true;
            for (var i = 0; i < invincibilityFrames; i++)
            {
                yield return null;
            }
            invincible = false;
        }

        public void RotateTowardsTarget(Vector3 targetPoint, float turnSpeed)
        {
            if (IsDying) { return; }
            var characterRoot = RotationTransform;
            var dir = targetPoint - transform.position;
            dir.Normalize();

            var yRotEuler = Quaternion.RotateTowards(characterRoot.localRotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime).eulerAngles.y;
            characterRoot.localRotation = Quaternion.Euler(0, yRotEuler, 0);
        }

        public void MoveToCalculations(float turnSpeed, float finalSpeed, Vector3 finalTarget)
        {
            var navMeshPath = new NavMeshPath();

            RotateTowardsTarget(agent.steeringTarget, turnSpeed);

            agent.CalculatePath(finalTarget, navMeshPath);
            if (navMeshPath != null)
            {
                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    agent.speed = finalSpeed;
                    agent.destination = finalTarget;
                }
                else if (navMeshPath.status == NavMeshPathStatus.PathPartial)
                {
                    // Put code here to perform something as a backup
                    return;
                }
                else if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
                {
                    return;
                }
            }
            else
            {
                Debug.LogError("The navmeshpath is null.", gameObject);
            }
        }

        protected IEnumerator SpawnInteractableItems ()
        {
            if (itemsToSpawn.Count == 0) { yield break; }

            foreach (var item in itemsToSpawn)
            {
                var spawnedItem = Instantiate(item.InteractableItemPrefab, transform.position, transform.rotation);
                spawnedItem.SpawnFromProp(transform.position);
                yield return new WaitForSeconds(0.25f);
            }

            itemsToSpawn.Clear();
            yield return null;
        }

        protected override IEnumerator KillCharacter ()
        {
            agent.enabled = false;
            characterController.enabled = false;
            yield return SpawnInteractableItems();
            yield return base.KillCharacter();
        }
    }
}
