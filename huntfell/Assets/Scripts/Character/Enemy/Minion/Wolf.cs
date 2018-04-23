using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Characters
{
    public sealed class Wolf : Minion, IMoveable, IAttack, IUtilityBasedAI
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
        /// The melee weapon that the wolf will use.
        /// </summary>
        [SerializeField]
        private Melee meleeWeapon;

        /// <summary>
        /// The coroutine for the wolf's attack.
        /// </summary>

        private IEnumerator attackCR;

        /// <summary>
        /// Has the wolf just found the player?
        /// </summary>
        [HideInInspector]
        public bool justFound = false;
        #endregion

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

        #region Unity Functions
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
        #endregion

        #region Wolf Movement
        public void Move(Transform target)
        {
            Move(target, speed);
        }

        public void Move(Transform target, float finalSpeed)
        {
            if (isDying) { return; }
            var finalTarget = new Vector3(target.position.x, RotationTransform.localPosition.y, target.position.z);

            if (target != null)
            {
                MoveToCalculations(turnSpeed, finalSpeed, finalTarget);
            }
            else
            {
                Debug.LogError("The target is null.");
            }
        }

        public void Move(Vector3 target, float finalSpeed)
        {
            if (isDying) { return; }
            var finalTarget = new Vector3(target.x, RotationTransform.localPosition.y, target.z);

            MoveToCalculations(turnSpeed, finalSpeed, finalTarget);
        }

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }
            Move(target, (speed / 2));
        }

        public void Turn(Transform target)
        {
            if (isDying) { return; }
            if (target != null)
            {
                RotateTowardsTarget(target.position, turnSpeed);
            }
        }
        #endregion

        #region Wolf Attack
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
            if (isDying) { return; }
            if (attackCR != null) { return; }
            attackCR = PlayAttackAnimation();
            StartCoroutine(attackCR);
        }

        public IEnumerator PlayAttackAnimation()
        {
            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("combat");
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            attackCR = null;
        }

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
        #endregion

        #region Unused Functions
        public void Idle()
        {
            if (isDying) { return; }
            // This feature has not yet been implemented.
        }

        public void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void Dash()
        {
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void Interact()
        {
            //Wolves cannot interact with stuff!
            return;
        }

        public void CycleWeapons(bool cycleUp)
        {
            return;
        }

        public void CycleElements(bool cycleUp)
        {
            return;
        }

        public void SwitchWeaponType(bool switchToMelee)
        {
            return;
        }
        #endregion
    }
}
