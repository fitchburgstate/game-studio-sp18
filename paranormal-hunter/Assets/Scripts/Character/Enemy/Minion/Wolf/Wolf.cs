using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{

    public sealed class Wolf : Minion, IMoveable, IAttack, IHealth
    {
        /// <summary>
        /// Speed at which the character moves.
        /// </summary>
        [Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 20)]
        public float speed = 5f;

        /// <summary>
        /// The speed at which the character turns.
        /// </summary>
        [Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 0 and 2,000.")]
        [Range(0, 2000)]
        public float rotateEnemy = 12f;

        /// <summary>
        /// The speed at which the animation curve plays.
        /// </summary>
        private float speedRamp;

        /// <summary>
        /// This animation curve determines the rate at which the wolf turns.
        /// </summary>
        public AnimationCurve rotateAnimation;

        /// <summary>
        /// This is the wolf's animator controller.
        /// </summary>
        public Animator anim;

        private void Start()
        {
            SetElementType(elementType);

            anim = GetComponent<Animator>();
            if (rangeWeapon != null)
            {
                rangeWeapon.gameObject.SetActive(false);
            }

            SetCurrentWeapon(melee);
        }

        private void Update()
        {
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject characterModel, NavMeshAgent agent)
        {

        }

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject characterModel, NavMeshAgent agent, Transform target)
        {
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            agent.destination = characterModel.transform.position;
            agent.updateRotation = false;

            if (moveDirection.magnitude != 0 || finalDirection.magnitude != 0)
            {
                var targetRotation = new Vector3(characterModel.transform.localEulerAngles.x, Mathf.Atan2(finalDirection.x, finalDirection.z) * Mathf.Rad2Deg, characterModel.transform.localEulerAngles.z);

                speedRamp = Mathf.Clamp(speedRamp + Time.deltaTime, 0, 1);
                var changeChar = rotateAnimation.Evaluate(speedRamp) * rotateEnemy;

                characterModel.transform.localRotation = Quaternion.RotateTowards(characterModel.transform.localRotation, Quaternion.Euler(targetRotation), changeChar);
            }
            else
            {
                speedRamp = 0;
            }

            controller.Move(moveDirection * Time.deltaTime);
        }

        public void Dash(CharacterController controller)
        {
            // This feature has not yet been implemented
        }

        public void Wander(CharacterController controller, Vector3 moveDirection, Vector3 lookDirection, GameObject characterModel, NavMeshAgent agent, Transform target)
        {

        }

        public void Retreat(CharacterController controller, Vector3 moveDirection, Vector3 lookDirection, GameObject characterModel, NavMeshAgent agent, Transform target)
        {

        }

        public void Idle(CharacterController controller)
        {

        }

        public float GetHealth()
        {
            return health;
        }

        public void Attack()
        {
            if (CurrentMeleeWeapon)
            {
                anim.SetTrigger("melee");
            }
            else if (CurrentRangeWeapon)
            {
                anim.SetTrigger("ranged");
            }
        }


        /// <summary>
        /// Enables the hitbox of the currently equipped melee weapon.
        /// </summary>
        public void EnableMeleeHitbox()
        {
            var meleeWeapon = CurrentMeleeWeapon;
            if (meleeWeapon != null)
            {
                meleeWeapon.EnableHitbox();
            }
        }

        /// <summary>
        /// Disables the hitbox of the currently equipped melee weapon.
        /// </summary>
        public void DisableMeleeHitbox()
        {
            var meleeWeapon = CurrentMeleeWeapon;
            if (meleeWeapon != null)
            {
                meleeWeapon.DisableHitbox();
            }
        }

        /// <summary>
        /// Fires the currently equipped range weapon.
        /// </summary>
        public void GunFiring()
        {
            rangeWeapon.Shoot();
        }
    }
}
