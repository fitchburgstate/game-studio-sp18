using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public sealed class Wolf : Minion, IMoveable, IAttack, IUtilityBasedAI
    {
        #region Properties
        public override int CurrentHealth
        {
            get
            {
                return health;
            }
            set
            {
                if (health <= 0 && !isDying)
                {
                    //TODO Change this to reflect wether the death anim should be cinematic or not later
                    StartCoroutine(KillWolf(true));
                    isDying = true;
                }
                health = value;
            }
        }
        #endregion

        #region Variables
        bool isDying = false;

        [SerializeField]
        private Melee meleeWeapon;

        private IEnumerator attackCR;
        #endregion

        protected override void Start()
        {
            base.Start();
            if (meleeWeapon != null) { EquipWeaponToCharacter(meleeWeapon); }
        }

        private void Update()
        {
            anim.SetFloat("dirX", agent.velocity.x / runSpeed);
            anim.SetFloat("dirY", agent.velocity.z / runSpeed);
            anim.SetBool("moving", Mathf.Abs(agent.velocity.magnitude) > 0.02f);
        }

        public void Move(Transform target)
        {
            if (isDying) { return; }
            var finalTarget = new Vector3(target.transform.position.x, RotationTransform.transform.localPosition.y, target.transform.position.z);
            agent.speed = runSpeed;
            agent.destination = finalTarget;
        }

        public void Idle()
        {
            if (isDying) { return; }
        }

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }
            agent.speed = runSpeed / 2;
            agent.destination = target;
        }

        private IEnumerator KillWolf(bool isCinematic)
        {
            agent.speed = 0;
            agent.destination = transform.position;
            anim.SetTrigger(isCinematic ? "death" : "cindeath");
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

        public void SwitchWeapon(bool cycleRanged, bool cycleMelee)
        {
            //Wolf only has one weapon so we don't need to switch
            return;
        }

        public IEnumerator PlayAttackAnimation()
        {
            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("combat");
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            attackCR = null;
        }

        public void WeaponAnimationEvent()
        {
            CurrentWeapon.StartAttackFromAnimationEvent();
        }

        #region Unused Functions
        public void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            if (isDying) { return; }
        }

        public void Dash()
        {
            // This should stay empty.
        }
        #endregion
    }
}
