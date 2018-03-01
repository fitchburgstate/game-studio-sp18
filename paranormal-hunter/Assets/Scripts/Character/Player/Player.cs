using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Hunter.Character
{
    public sealed class Player : Character, IMoveable, IAttack
    {
        /// <summary>
        /// Speed at which the character moves.
        /// </summary>
        [Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 1 and 20.")]
        [Range(1, 20)]
        public float speed = 5f;

        /// <summary>
        /// The speed at which the character turns.
        /// </summary>
        [Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 1 and 2,000.")]
        [Range(1, 2000)]
        public float rotateChar = 500f;

        /// <summary>
        /// The speed at which the animation curve plays.
        /// </summary>
        private float speedRamp;

        /// <summary>
        /// This animation curve determines the rate at which the player turns.
        /// </summary>
        public AnimationCurve rotateAnimation;

        /// <summary>
        /// This is the player's animator controller.
        /// </summary>
        public Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            if (rangeWeapon != null)
            {
                rangeWeapon.gameObject.SetActive(false);
            }

            SetCurrentWeapon(melee);
        }

        /// <summary>
        /// Moves the character controller in the desired direction.
        /// </summary>
        /// <param name="controller">The character controller that will be moved.</param>
        /// <param name="moveDirection">The direction in which the character controller will be moved.</param>
        /// <param name="finalDirection">The direction in which the model will rotate to face. This exists so the character can look independently of movement.</param>
        /// <param name="characterModel">The model that will be rotated either dependently or independently of the movement.</param>
        /// <param name="agent">The navmesh agent on the gameobject.</param>
        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject characterModel, NavMeshAgent agent)
        {
            anim.SetFloat("dirY", Mathf.Abs(moveDirection.magnitude), 0, 1);

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            agent.destination = characterModel.transform.position;
            agent.updateRotation = false;

            if (moveDirection.magnitude != 0 || finalDirection.magnitude != 0)
            {
                var targetRotation = new Vector3(characterModel.transform.localEulerAngles.x, Mathf.Atan2(finalDirection.x, finalDirection.z) * Mathf.Rad2Deg, characterModel.transform.localEulerAngles.z);

                speedRamp = Mathf.Clamp(speedRamp + Time.deltaTime, 0, 1);
                var changeChar = rotateAnimation.Evaluate(speedRamp) * rotateChar;

                characterModel.transform.localRotation = Quaternion.RotateTowards(characterModel.transform.localRotation, Quaternion.Euler(targetRotation), changeChar);
            }
            else
            {
                speedRamp = 0;
            }
            controller.Move(moveDirection * Time.deltaTime);
        }

        /// <summary>
        /// Moves the character controller a short distance rapidly in the desired direction.
        /// </summary>
        /// <param name="controller">The character controller that will be moved.</param>
        public void Dash(CharacterController controller)
        {
            // This feature has not yet been implemented
        }

        /// <summary>
        /// Triggers the attack function on the animator controller.
        /// </summary>
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
