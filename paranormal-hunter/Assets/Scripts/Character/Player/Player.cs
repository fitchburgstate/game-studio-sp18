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

        private IEnumerator TestDash(Vector3 target)
        {
            var dashComplete = 0.09f;
            float dashTime = 0;
            while(Vector3.Distance(transform.position, target) > dashComplete)
            {
                dashTime += dashSpeed * Time.deltaTime;
                var dashAmount = dashAnimation.Evaluate(dashTime);
                transform.position = Vector3.Lerp(transform.position, target, dashAmount);
                yield return null;
            }
        }



        private Vector3 OnNavMesh(Vector3 target)
        {
            var hit = new NavMeshHit();
            if(NavMesh.Raycast(transform.position, target, out hit, NavMesh.AllAreas))
            {
                Debug.Log("off");
                Debug.Log(target);
                target = hit.position;
                //compare y values for target and current postion
                target = new Vector3(target.x, target.y + 1, target.z);
                return target;
            }
            else
            {
                Debug.Log("on");
                Debug.Log("Transform: " + transform.position);
                Debug.Log("Target: " + target);
                Debug.Log("Hit: " + hit.position);
                target = new Vector3(target.x, hit.position.y + 1, target.z);
                return target;
            }
            
            /*if((NavMesh.FindClosestEdge(target, out hit, NavMesh.AllAreas)))
            {
                target = hit.position;
                Debug.Log(target);
                Debug.Log("on");
                return target;
            }
            else
            {
                Debug.Log("off");
                
                return transform.position;
            }*/
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
            var fwd = playerRoot.transform.forward;
            var target = new Vector3();
            var hit = new RaycastHit();
            var ray = new Ray(transform.position, fwd);


            if(Physics.Raycast(ray, out hit, 7))
            {
                target = hit.point;
                Debug.Log("Blocked");
            }
            else
            {
                target = ray.GetPoint(7);
                Debug.Log("No Block");
            }

            Debug.DrawRay(transform.position, fwd * 7, Color.red);

            target = OnNavMesh(target);

            StartCoroutine(TestDash(target));

            //Debug.Log(startTime);
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
