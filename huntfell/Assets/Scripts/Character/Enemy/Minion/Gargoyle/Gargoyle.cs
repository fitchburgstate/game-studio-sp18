using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Character
{
    public class Gargoyle : Minion, IMoveable, IUtilityBasedAI, IAttack
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
                    StartCoroutine(KillGargoyle(true));
                    isDying = true;
                }
                health = value;
            }
        }
        #endregion

        #region Variables
        bool isDying = false;
        
        [SerializeField]
        private Range rangedWeapon;

        private IEnumerator attackCR;
        #endregion

        protected override void Start()
        {
            base.Start();
            if (rangedWeapon != null) { EquipWeaponToCharacter(rangedWeapon); }
        }

        public void Attack()
        {
            if (attackCR != null) { return; }
            attackCR = PlayAttackAnimation();
            StartCoroutine(attackCR);
        }

        public void SwitchWeapon(bool cycleRanged, bool cycleMelee)
        {
            //Gargoyle only has one weapon so we don't need to switch
            return;
        }

        public IEnumerator PlayAttackAnimation()
        {
            //anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            //anim.SetTrigger("combat");
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            attackCR = null;
        }

        public void WeaponAnimationEvent()
        {
            CurrentWeapon.StartAttackFromAnimationEvent();
        }

        private IEnumerator KillGargoyle(bool isCinematic)
        {
            anim.SetTrigger("death");
            // TODO Change this later to reflect the animation time
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
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

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }
        }

        public void Move(Transform target)
        {
            if (isDying) { return; }
        }

        public void Idle()
        {
            if (isDying) { return; }
        }
        #endregion
    }
}
