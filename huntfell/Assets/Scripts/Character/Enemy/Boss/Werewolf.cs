using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters.AI;
using System;

namespace Hunter.Characters
{
    public class Werewolf : Boss, IMoveable, IAttack, IUtilityBasedAI
    {
        #region Variables
        /// <summary>
        /// Determines the movement speed of the boss.
        /// </summary>
        [Header("Movement Options")]
        [Range(0, 20), Tooltip("The running speed of the character when it is in combat.")]
        public float speed = 5f;

        /// <summary>
        /// Determines the speed at which the boss turns.
        /// </summary>
        [Range(1, 250)]
        public float turnSpeed = 175f;

        /// <summary>
        /// The weapon that belongs in the left hand of the boss.
        /// </summary>
        [Header("Combat Options")]
        [SerializeField]
        private Melee leftClawWeapon;

        /// <summary>
        /// The weapon that belongs in the right hand of the boss.
        /// </summary>
        [SerializeField]
        private Melee rightClawWeapon;

        /// <summary>
        /// The weapon that will be used during the bosses heavy attack.
        /// </summary>
        [SerializeField]
        private Melee doubleSwipeWeapon;

        /// <summary>
        /// Determines whether the wolf should perform a third swing or not.
        /// </summary>
        private bool performThirdSwing = false;

        /// <summary>
        /// The animation clip for the first attack.
        /// </summary>
        [Header("Animation Clips Library")]
        [SerializeField]
        private AnimationClip firstAttackClip;

        /// <summary>
        /// The animation clip for the second attack.
        /// </summary>
        [SerializeField]
        private AnimationClip secondAttackClip;

        /// <summary>
        /// The animation clip for the third attack.
        /// </summary>
        [SerializeField]
        private AnimationClip thirdAttackClip;

        /// <summary>
        /// The animation clip for the third attack.
        /// </summary>
        [SerializeField]
        private AnimationClip howlClip;

        /// <summary>
        /// Determines whether the Attack Coroutine can be played or not.
        /// </summary>
        private IEnumerator attackCR;

        /// <summary>
        /// Determines whether the Howl Coroutine can be played or not.
        /// </summary>
        private IEnumerator howlCR;

        /// <summary>
        /// Used to reference the BossInputModule attached to the boss.
        /// </summary>
        private BossInputModule bossInputModule;

        /// <summary>
        /// Determines whether debug logs relating to the boss will appear in the console window.
        /// </summary>
        [Header("Special Options")]
        public bool showDebugLogs = false;

        /// <summary>
        /// Determines the phase that the boss is in, currently there are a maximum of 3 phases.
        /// </summary>
        // Phase related variables
        [Range(1, 3), SerializeField]
        private int phase = 1;

        /// <summary>
        /// Determines whether the boss should spawn minions or not.
        /// </summary>
        [SerializeField]
        private bool spawnMinions = false;
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
                    StartCoroutine(KillWerewolf());
                    isDying = true;
                }
                // This checks to see if the werewolf has entered phase 2 yet
                else if ((health / totalHealth < .66f) && phase < 2)
                {
                    anim.SetBool("performThirdSwing", true);
                    performThirdSwing = true;
                    phase = 2;
                    invincible = true;
                    InitiateHowl();
                }
                else if ((health / totalHealth < .33f) && phase < 3)
                {
                    phase = 3;
                    invincible = true;
                    InitiateHowl();
                }
            }
        }

        public BossInputModule BossInputModule
        {
            get
            {
                if (bossInputModule == null) { GetComponent<BossInputModule>(); }
                return bossInputModule;
            }
        }
        #endregion

        #region Unity Functions
        protected override void Start()
        {
            base.Start();
            agent.speed = speed;

            rightClawWeapon.gameObject.SetActive(false);
            leftClawWeapon.gameObject.SetActive(false);
            doubleSwipeWeapon.gameObject.SetActive(false);
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }

            bossInputModule = GetComponent<BossInputModule>();
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
                Debug.LogWarning("There is no animator controller; floats dirX and dirY, and bool moving are not being set.");
            }
        }
        #endregion

        #region Werewolf Movement
        public void Move(Transform target)
        {
            if (isDying) { return; }
            BossInputModule.isAttacking = false;
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
        }

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }
        }

        public void Turn(Transform target)
        {
            if (isDying) { return; }
            BossInputModule.isAttacking = false;
            if (target != null)
            {
                RotateTowardsTarget(target.position, turnSpeed);
            }
        }
        #endregion

        #region Werewolf Combat
        /// <summary>
        /// Function used to kill the boss once it's health reaches 0.
        /// </summary>
        private IEnumerator KillWerewolf()
        {
            agent.speed = 0;
            agent.destination = transform.position;
            agent.enabled = false;
            characterController.enabled = false;

            if (attackCR != null)
            {
                StopCoroutine(attackCR);
                attackCR = null;
            }

            #region Don't worry about this
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
            rightClawWeapon.baseDamage = 65;
            rightClawWeapon.critPercent = 75;
            rightClawWeapon.hitBoxFrames = 80;
            anim.SetTrigger("death");
            #endregion

            yield return null;
        }

        /// <summary>
        /// Begins the attack coroutine.
        /// </summary>
        public void Attack()
        {
            if (attackCR != null) { return; }
            if (isDying) { return; }

            attackCR = ComboAttackAnimation();
            // STARTS ATTACK COROUTINE
            StartCoroutine(attackCR);
        }

        /// <summary>
        /// Usually activated within the animation itself, this activates the hitbox of the current weapon.
        /// </summary>
        public void WeaponAnimationEvent()
        {
            CurrentWeapon?.StartAttackFromAnimationEvent();
        }

        public void InitiateHowl()
        {
            if (howlCR != null) { return; }
            if (isDying) { return; }

            howlCR = HowlAnimation();
            StartCoroutine(howlCR);
        }

        /// <summary>
        /// The main logic for the bosses' chain attack. As long as attackCR is not reset, then the coroutine should resume from where it left off.
        /// </summary>
        public IEnumerator ComboAttackAnimation()
        {
            BossInputModule.isAttacking = true;

            // First attack in the combo swing
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("firstAttack");

            yield return new WaitForSeconds(firstAttackClip.length);

            // Second attack in the combo swing
            if (leftClawWeapon != null) { EquipWeaponToCharacter(leftClawWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("secondAttack");

            yield return new WaitForSeconds(secondAttackClip.length);

            if (performThirdSwing)
            {
                // Third attack in the combo swing
                if (doubleSwipeWeapon != null) { EquipWeaponToCharacter(doubleSwipeWeapon); }

                anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
                anim.SetTrigger("thirdAttack");

                yield return new WaitForSeconds(thirdAttackClip.length);
            }

            // Wait for the last swing to finish, and then resetting everything
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            BossInputModule.isAttacking = false;

            attackCR = null;
        }

        private IEnumerator HowlAnimation()
        {
            while (BossInputModule.isAttacking)
            {
                yield return new WaitForEndOfFrame();
            }

            anim.SetTrigger("howl");
            isDying = true;
            yield return new WaitForSeconds(howlClip.length);

            if (phase == 2)
            {
                // Perform phase 2 actions here
            }
            else if (phase == 3)
            {
                // Perform phase 3 actions here
            }

            invincible = false;
            isDying = false;
            howlCR = null;
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
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void CycleWeapons(bool cycleUp)
        {
            if (isDying) { return; }
        }

        public void CycleElements(bool cycleUp)
        {
            if (isDying) { return; }
        }

        public void SwitchWeaponType(bool switchToMelee)
        {
            if (isDying) { return; }
        }
        #endregion
    }
}
