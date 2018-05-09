using Hunter.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Characters
{
    public enum EnemyType
    {
        Wolf,
        Bat,
        Gargoyle,
        Werewolf
    }

    public abstract class Enemy : Character
    {
        #region Variables
        /// <summary>
        /// This is the speed at which the character runs.
        /// </summary>
        [Range(0, 20), Tooltip("The running speed of the character when it is in combat.")]
        public float speed = 5f;

        [Range(1, 250)]
        public float turnSpeed = 175f;

        /// <summary>
        /// The element type of the enemy.
        /// </summary>
        public Element elementType;

        /// <summary>
        /// The element option (what's shown in the inspector) of the enemy.
        /// </summary>
        public ElementOption enemyElementOption;

        /// <summary>
        /// The amount of frames the enemy is invincible for after being damaged.
        /// </summary>
        public int invincibilityFrames = 5;

        /// <summary>
        /// The items to spawn on death.
        /// </summary>
        [SerializeField]
        private List<InventoryItem> itemsToSpawn = new List<InventoryItem>();

        /// <summary>
        /// An instance of the AIInputModule component attached to the enemy.
        /// </summary>
        private AIInputModule aiInputModuleInstance;
        #endregion

        #region Properties
        public AIInputModule AIInputModuleInstance
        {
            get
            {
                if (aiInputModuleInstance == null) { aiInputModuleInstance = GetComponent<AIInputModule>(); }
                return aiInputModuleInstance;
            }
        }
        #endregion

        #region Unity Functions
        protected override void Start ()
        {
            base.Start();
            elementType = Utility.ElementOptionToElement(enemyElementOption);
        }
        #endregion

        #region Combat Related Functions
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

        protected override IEnumerator KillCharacter ()
        {
            agent.enabled = false;
            characterController.enabled = false;
            yield return SpawnInteractableItems();
            yield return base.KillCharacter();
        }
        #endregion

        #region Movement Related Functions
        public void RotateTowardsTarget (Vector3 targetPoint, float turnSpeed)
        {
            if (IsDying) { return; }
            var characterRoot = RotationTransform;
            var dir = targetPoint - transform.position;
            dir.Normalize();

            var yRotEuler = Quaternion.RotateTowards(characterRoot.localRotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime).eulerAngles.y;
            characterRoot.localRotation = Quaternion.Euler(0, yRotEuler, 0);
        }

        public void MoveToCalculations (float turnSpeed, float finalSpeed, Vector3 finalTarget)
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
                    if (AIInputModuleInstance.wander == true)
                    {
                        AIInputModuleInstance.FindNewTargetPoint();
                    }
                    else
                    {
                        Debug.LogWarning("The path is partially invalid.", gameObject);
                    }
                    return;
                }
                else if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
                {
                    Debug.LogWarning("The path was invalid.", gameObject);

                    if (AIInputModuleInstance != null)
                    {
                        if (AIInputModuleInstance.wander == true)
                        {
                            AIInputModuleInstance.FindNewTargetPoint();
                        }
                        else
                        {
                            AIInputModuleInstance.inCombat = false;
                        }
                    }
                    return;
                }
            }
            else
            {
                Debug.LogError("The navmeshpath is null.", gameObject);
            }
        }
        #endregion

        #region Other Functions
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
        #endregion
    }
}
