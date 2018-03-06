using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Hunter.Character
{
    public sealed class Player : Character, IMoveable
    {
        // ---------- SET THESE IN THE INSPECTOR ---------- \\
        [Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 20)]
        public float speed = 5f;

        [Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 2000)]
        public float rotateChar = 12f;

        private float speedRamp;
        public AnimationCurve rotateAnimation;

        public Animator anim;
        // ------------------------------------------------ \\ 

        private void Start()
        {
            anim = GetComponent<Animator>();
            if (range != null)
            {
                range.gameObject.SetActive(false);
            }

            SetCurrentWeapon(melee);
        }

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject playerRoot, NavMeshAgent agent)
        {
            anim.SetFloat("dirY", Mathf.Abs(moveDirection.magnitude), 0, 1);

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            agent.destination = playerRoot.transform.position;
            agent.updateRotation = false;

            if (moveDirection.magnitude != 0 || finalDirection.magnitude != 0)
            {
                var targetRotation = new Vector3(playerRoot.transform.localEulerAngles.x, Mathf.Atan2(finalDirection.x, finalDirection.z) * Mathf.Rad2Deg, playerRoot.transform.localEulerAngles.z);

                speedRamp = Mathf.Clamp(speedRamp + Time.deltaTime, 0, 1);
                var changeChar = rotateAnimation.Evaluate(speedRamp) * rotateChar;

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

        public void EnableMeleeHitbox()
        {
            var mw = CurrentMeleeWeapon;
            if (mw != null)
            {
                mw.EnableHitbox();
            }
        }

        public void DisableMeleeHitbox()
        {
            var mw = CurrentMeleeWeapon;
            if (mw != null)
            {
                mw.DisableHitbox();
            }
        }

        // ----------- Animation Event Methods ----------- \\
        public void GunFiring()
        {
            range.Shoot();
        }

        // -------------------------------------------------- \\

        // ----------- Sound Event Methods ----------- \\
        // Player footsteps
        void PlayFootsteps(){
            Fabric.EventManager.Instance.PostEvent("Footstep", gameObject);
        }
        // For when the player gets hit
        void PlayPlayerHit(){
            Fabric.EventManager.Instance.PostEvent("Player Hit", gameObject);
        }
        // -------------------------------------------------- \\
    }
}
