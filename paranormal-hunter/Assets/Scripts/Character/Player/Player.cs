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
        public AnimationCurve dashAnimation;

        public Animator anim;
        public float dashSpeed;

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

        //DO NOT DELETE Currently Works
        /*public IEnumerator TestDash(Vector3 target)
        {
            var startTime = Time.time;
            while(Vector3.Distance(transform.position, target) != 0)
            {
                transform.position = Vector3.Lerp(transform.position, target, (Time.time - startTime) * 2);
                if(Vector3.Distance(transform.position, target) == 0)
                {
                    break;
                }
                yield return null;
            }

            yield return new WaitForSeconds(3.0f);
        }*/

        private IEnumerator TestDash(Vector3 target)
        {
            var dashComplete = 0.09f;
            float dashTime = 0;
            while(Vector3.Distance(transform.position, target) > dashComplete)
            {
                Debug.Log(Vector3.Distance(transform.position, target));
                dashTime += dashSpeed * Time.deltaTime;
                var dashAmount = dashAnimation.Evaluate(dashTime);
                transform.position = Vector3.Lerp(transform.position, target, dashAmount);
                yield return null;
            }
        }

        public void Dash(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject playerRoot, NavMeshAgent agent)
        {
            // This feature has not yet been implemented
            /*
             * TODO:
             * Perfect Diagnol on Movement causes dashes not to work (may or may not need to fix)
             * move timed bit to coroutine
             */
            var start = transform.position;
            var temp = start;
            //Debug.Log(startTime);
            if (finalDirection.x <= .5 && finalDirection.x >= -.5 && finalDirection.z > 0)
            {
                Debug.Log("Dash Foward");
                temp.z = start.z + 5;
                temp.x = start.x + 5;
                StartCoroutine(TestDash(temp));
            }
            /*else if(finalDirection.x <= .5 && finalDirection.x >= -.5 && finalDirection.z < 0)
            {
                Debug.Log("Dash Backward");
                transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z - 5);//Will need to change with animation when added
            }
            else if(finalDirection.z <= .5 && finalDirection.z >= -.5 && finalDirection.x > 0)
            {
                Debug.Log("Dash Right");
                transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z - 5);//Will need to change with animation when added
            }
            else if (finalDirection.z <= .5 && finalDirection.z >= -.5 && finalDirection.x < 0)
            {
                Debug.Log("Dash Left");
                transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z + 5);//Will need to change with animation when added
            }
            else if(finalDirection.x <= .8 && finalDirection.x >=.2 && finalDirection.z > 0)
            {
                Debug.Log("Diag Up Right");
                transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
            }
            else if(finalDirection.x <= .8 && finalDirection.x >= .2 && finalDirection.z < 0)
            {
                Debug.Log("Diag Down Right");
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);
            }
            else if (finalDirection.z <= .8 && finalDirection.z >= .2 && finalDirection.x > 0)
            {
                Debug.Log("Diag Up Left");
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z  + 5);
            }
            else if (finalDirection.z <= .8 && finalDirection.z >= .2 && finalDirection.x < 0)
            {
                Debug.Log("Diag Down Left");
                transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);
            }*/
            //nextDodge = Time.time + dodgeRate;
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
    }
}
