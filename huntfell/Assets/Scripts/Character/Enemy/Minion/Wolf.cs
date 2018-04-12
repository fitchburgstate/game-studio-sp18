using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Characters
{
    public sealed class Wolf : Minion, IMoveable, IAttack, IUtilityBasedAI
    {
        [HideInInspector]
        public bool justFound = false;

        [SerializeField]
        private Melee meleeWeapon;
        private IEnumerator attackCR;

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
                    //TODO Change this to reflect wether the death anim should be cinematic or not later
                    StartCoroutine(KillWolf(true));
                    isDying = true;
                }
            }
        }

        protected override void Start ()
        {
            base.Start();
            if (meleeWeapon != null) { EquipWeaponToCharacter(meleeWeapon); }
        }

        private void Update ()
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
            // This feature has not yet been implemented.
        }

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }
            agent.speed = runSpeed / 2;
            agent.destination = target;
        }

        public void Move (Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            //fuck you
        }

        public void Dash ()
        {
            // no dash for wolfie boi
        }

        private IEnumerator KillWolf (bool isCinematic)
        {
            agent.speed = 0;
            agent.destination = transform.position;
            anim.SetTrigger(isCinematic ? "cinDeath" : "death");
            minionHealthBarParent?.gameObject.SetActive(false);
            //TODO Change this later to reflect the animation time
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }

        public void Attack ()
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

        public void WolfBiteSoundAnimationEvent()
        {
            Fabric.EventManager.Instance.PostEvent("Wolf Attack", gameObject);
        }

        public void WolfLungeSoundAnimationEvent()
        {
            Fabric.EventManager.Instance.PostEvent("Wolf Lunge Attack", gameObject);
        }

        public void WeaponAnimationEvent ()
        {
            CurrentWeapon.StartAttackFromAnimationEvent();
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
    }
}
