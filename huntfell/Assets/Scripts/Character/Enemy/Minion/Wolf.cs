using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Characters
{
    public sealed class Wolf : Minion, IMoveable, IAttack, IUtilityBasedAI
    {
        #region Properties
        public override float CurrentHealth
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                if (health <= 0 && !isDying)
                {
                    // TODO Change this to reflect wether the death anim should be cinematic or not later
                    StartCoroutine(KillWolf(true));
                    isDying = true;
                }
            }
        }
        #endregion

        #region Variables
        /// <summary>
        /// Has the wolf just found the player?
        /// </summary>
        [HideInInspector]
        public bool justFound = false;

        /// <summary>
        /// The melee weapon that the wolf will use.
        /// </summary>
        [SerializeField]
        private Melee meleeWeapon;

        /// <summary>
        /// The coroutine for the wolf's attack.
        /// </summary>
        private IEnumerator attackCR;

        [Range(1, 250)]
        public float turnSpeed = 80f;
        #endregion

        protected override void Start()
        {
            base.Start();
            if (meleeWeapon != null) { EquipWeaponToCharacter(meleeWeapon); }
            agent.updateRotation = false;
        }

        private void Update()
        {
            if (anim != null)
            {
                anim.SetFloat("dirX", agent.velocity.x / speed);
                anim.SetFloat("dirY", agent.velocity.z / speed);
                anim.SetBool("moving", Mathf.Abs(agent.velocity.magnitude) > 0.02f);
            }
            else
            {
                Debug.LogWarning("There is no animator controller; floats dirX and dirY as well as bool moving are not being set.");
            }
        }

        public void Move(Transform target)
        {
            if (isDying) { return; }

            if (target != null)
            {
                var finalTarget = new Vector3(target.transform.position.x, RotationTransform.localPosition.y, target.transform.position.z);
                var navMeshPath = new NavMeshPath();

                RotateTowardsTarget(agent.steeringTarget);

                agent.CalculatePath(finalTarget, navMeshPath);
                if (navMeshPath != null)
                {
                    if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        agent.speed = speed;
                        agent.destination = finalTarget;
                    }
                    else if (navMeshPath.status == NavMeshPathStatus.PathPartial)
                    {
                        Debug.LogWarning("There is a navmesh path but the AI can't reach the destination.");
                        // Put code here to perform something as a backup
                        return;
                    }
                    else if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
                    {
                        Debug.LogError("There is no valid navmesh path that the AI can take to reach the destination.");
                        return;
                    }
                }
                else
                {
                    Debug.LogError("The navmeshpath is null.");
                }
            }
            else
            {
                Debug.LogError("The target is null.");
            }
        }

        public void Turn(Transform target)
        {
            if (isDying) { return; }
            if (target != null)
            {
                RotateTowardsTarget(target.position);
            }
        }

        public void Idle()
        {
            // This feature has not yet been implemented.
        }

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }

            var navMeshPath = new NavMeshPath();

            agent.CalculatePath(target, navMeshPath);
            if (navMeshPath != null)
            {
                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    agent.speed = speed / 2;
                    agent.destination = target;
                }
                else if (navMeshPath.status == NavMeshPathStatus.PathPartial)
                {
                    Debug.LogWarning("There is a navmesh path but the AI can't reach the destination.");
                    // Put code here to perform something as a backup
                    return;
                }
                else if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
                {
                    Debug.LogError("There is no valid navmesh path that the AI can take to reach the destination.");
                    return;
                } 
            }
            else
            {
                Debug.LogError("The navmeshpath is null.");
            }
        }

        public void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            // This feature will not be implemented.
        }

        public void Dash()
        {
            // This feature will not be implemented.
        }

        private IEnumerator KillWolf(bool isCinematic)
        {
            agent.speed = 0;
            agent.destination = transform.position;
            anim.SetTrigger(isCinematic ? "cinDeath" : "death");
            minionHealthBarParent?.gameObject.SetActive(false);
            // TODO Change this later to reflect the animation time
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }

        public void Attack()
        {
            if (attackCR != null) { return; }
            attackCR = PlayAttackAnimation();
            StartCoroutine(attackCR);
        }

        public IEnumerator PlayAttackAnimation ()
        {
            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("combat");
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            attackCR = null;
        }

        #region Miscellaneous Functions
        public void WolfBiteSoundAnimationEvent()
        {
            Fabric.EventManager.Instance?.PostEvent("Wolf Attack", gameObject);
        }

        public void WolfLungeSoundAnimationEvent()
        {
            Fabric.EventManager.Instance?.PostEvent("Wolf Lunge Attack", gameObject);
        }

        public void WeaponAnimationEvent()
        {
            CurrentWeapon?.StartAttackFromAnimationEvent();
        }

        public void Interact ()
        {
            //Wolves cannot interact with stuff!
            return;
        }

        public void CycleWeapons (bool cycleUp)
        {
            return;
        }

        public void CycleElements (bool cycleUp)
        {
            return;
        }

        public void SwitchWeaponType (bool switchToMelee)
        {
            return;
        }

        private void RotateTowardsTarget(Vector3 targetPoint)
        {
            var characterRoot = RotationTransform;
            var dir = targetPoint - transform.position;
            dir.Normalize();

            var yRotEuler = Quaternion.RotateTowards(characterRoot.localRotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime).eulerAngles.y;
            characterRoot.localRotation = Quaternion.Euler(0, yRotEuler, 0);
        }
        #endregion
    }
}
