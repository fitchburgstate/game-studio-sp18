using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Hunter.Character
{
    public sealed class Player : Character, IMoveable
    {

        /// <summary>
        /// Current Melee Weapon Equipped on the Player, to be set in the inspector
        /// </summary>
        [SerializeField]
        private Melee meleeWeapon;

        /// <summary>
        /// Current Ranged Weapon Equipped on the Player, to be set in the inspector
        /// </summary>
        [SerializeField]
        private Range rangedWeapon;

        [Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 20)]
        public float moveSpeed = 5f;

        [Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 2000)]
        public float rotationSpeed = 12f;

        public AnimationCurve rotateAnimationCurve; 

        private float speedRamp;
        private Animator anim;
        private Coroutine attackCR;

        private void Start()
        {
            anim = GetComponent<Animator>();
            if (rangedWeapon != null)
            {
                rangedWeapon.gameObject.SetActive(false);
            }
            //Always start with your melee weapon
            SetCurrentWeapon(meleeWeapon);
        }

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject playerRoot, NavMeshAgent agent)
        {
            anim.SetFloat("dirY", Mathf.Abs(moveDirection.magnitude), 0, 1);

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            agent.destination = playerRoot.transform.position;
            agent.updateRotation = false;

            if (moveDirection.magnitude != 0 || finalDirection.magnitude != 0)
            {
                var targetRotation = new Vector3(playerRoot.transform.localEulerAngles.x, Mathf.Atan2(finalDirection.x, finalDirection.z) * Mathf.Rad2Deg, playerRoot.transform.localEulerAngles.z);

                speedRamp = Mathf.Clamp(speedRamp + Time.deltaTime, 0, 1);
                var changeChar = rotateAnimationCurve.Evaluate(speedRamp) * rotationSpeed;

                playerRoot.transform.localRotation = Quaternion.RotateTowards(playerRoot.transform.localRotation, Quaternion.Euler(targetRotation), changeChar);
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

        public void SwitchWeapon ()
        {
            if (CurrentWeapon is Melee)
            {
                meleeWeapon.gameObject.SetActive(false);
                rangedWeapon.gameObject.SetActive(true);
                SetCurrentWeapon(rangedWeapon);
            }
            else if (CurrentWeapon is Range)
            {
                rangedWeapon.gameObject.SetActive(false);
                meleeWeapon.gameObject.SetActive(true);
                SetCurrentWeapon(meleeWeapon);
            }
        }

        public void Attack()
        {
            if(attackCR != null) { return; }
            attackCR = StartCoroutine(PlayAttackAnimation());
        }

        private IEnumerator PlayAttackAnimation ()
        {
            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            if (CurrentWeapon is Melee)
            {
                anim.SetTrigger("melee");
            }
            else if (CurrentWeapon is Range)
            {
                anim.SetTrigger("ranged");
            }
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            attackCR = null;
        }

        public void WeaponAnimationEvent ()
        {
            CurrentWeapon.StartAttackFromAnimationEvent();
        }
    }
}
