using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters.AI;

namespace Hunter.Characters
{
    public class Werewolf : Boss, IMoveable, IAttack, IUtilityBasedAI
    {
        #region Variables
        [Header("Movement Options")]
        [Range(0, 20), Tooltip("The running speed of the character when it is in combat.")]
        public float speed = 5f;

        [Range(1, 250)]
        public float turnSpeed = 175f;

        [Header("Combat Options")]
        [SerializeField]
        private Melee leftClawWeapon;
        [SerializeField]
        private Melee rightClawWeapon;
        [SerializeField]
        private Melee doubleSwipeWeapon;

        private IEnumerator attackCR;
        [HideInInspector]
        public bool justFound = false;

        // Phase related variables
        [Range(1, 3)]
        private int phase = 1;
        private bool spawnMinions = false;

        private BossInputModule bossInputModule;
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
                    StartCoroutine(KillWerewolf(true));
                    isDying = true;
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
            bossInputModule = GetComponent<BossInputModule>();
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
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

        #region Werewolf Movement
        public void Move(Transform target)
        {
            if (isDying) { return; }
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
            if (target != null)
            {
                RotateTowardsTarget(target.position, turnSpeed);
            }
        }
        #endregion

        #region Werewolf Attack
        private IEnumerator KillWerewolf(bool isCinematic)
        {
            agent.speed = 0;
            agent.destination = transform.position;
            anim.SetTrigger("death");
            yield return null;
        }

        public void Attack()
        {
            if (isDying) { return; }
            if (attackCR != null) { return; }
            attackCR = PlayAttackAnimation();
            StartCoroutine(attackCR);
        }

        #region Deprecated Code
        //public IEnumerator PlayFirstSwingAnimation()
        //{
        //    if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
        //    anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
        //    anim.SetTrigger("combat");
        //    yield return new WaitForSeconds(.75f);
        //    attackCR = PlaySecondSwingAnimation();
        //    yield return StartCoroutine(attackCR);
        //}

        //public IEnumerator PlaySecondSwingAnimation()
        //{
        //    if (leftClawWeapon != null) EquipWeaponToCharacter(leftClawWeapon);
        //    yield return new WaitForSeconds(.75f);
        //    attackCR = PlayThirdSwingAnimation();
        //    yield return StartCoroutine(attackCR);
        //}

        //public IEnumerator PlayThirdSwingAnimation()
        //{
        //    if (doubleSwipeWeapon != null) EquipWeaponToCharacter(doubleSwipeWeapon);
        //    yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
        //    attackCR = null;
        //    if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
        //}
        #endregion

        public IEnumerator PlayAttackAnimation()
        {
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
            BossInputModule.isAttacking = true;
            var swingCount = 1;
            while (swingCount < 4)
            {
                switch (swingCount)
                {
                    case 1:
                        if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
                        anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
                        anim.SetTrigger("combat");
                        swingCount += 2;
                        yield return new WaitForSeconds(.25f);
                        break;
                    case 2:
                        if (leftClawWeapon != null) EquipWeaponToCharacter(leftClawWeapon);
                        swingCount += 3;
                        yield return new WaitForSeconds(.25f);
                        break;
                    case 3:
                        if (doubleSwipeWeapon != null) EquipWeaponToCharacter(doubleSwipeWeapon);
                        swingCount += 4;
                        yield return new WaitForSeconds(.25f);
                        break;
                }
            }
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
            BossInputModule.isAttacking = false;
            attackCR = null;
        }

        public void WeaponAnimationEvent()
        {
            if (isDying) { return; }
            CurrentWeapon.StartAttackFromAnimationEvent();
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
