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
        public AnimationCurve rotateAnimation;
        public AnimationCurve dashAnimation;

        public Animator anim;
        public float dashSpeed;
        public float dashLength;
        public float dashCoolDown;
        private bool canDash = true;
        private Vector3 dashTarget;
        private Vector3 lookDirection;
        private Vector3 lastLeft;

        // ------------------------------------------------ \\ 

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
            anim.SetFloat("dirX", moveDirection.x);
            anim.SetFloat("dirY", moveDirection.z);
            anim.SetFloat("lookX", LookDirection.x);
            anim.SetFloat("lookY", LookDirection.z);

            //if (moveDirection == Vector3.zero)
            //{
            //    anim.SetBool("Moving", false);
            //}
            //else
            //{
            //    anim.SetBool("Moving", true);
            //}

            

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

        private float SignedAngle(Vector3 a, Vector3 b)
        {
            return Vector3.Angle(a, b) * Mathf.Sign(Vector3.Cross(a, b).y);
        }

        /// <summary>
        /// Dashes the Player in the direction they are facing
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="moveDirection"></param>
        /// <param name="finalDirection"></param>
        /// <param name="playerRoot"></param>
        /// <param name="agent"></param>
        public void Dash(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject playerRoot, NavMeshAgent agent)
        {
            if (!canDash)
            {
                return;
            }
            canDash = false;

            var start = transform.position;
            var temp = start;
            var fwd = playerRoot.transform.forward;
            var target = new Vector3();
            var hit = new RaycastHit();
            var ray = new Ray(transform.position, fwd);

            //Raycast to determine target point for dodge destination
            if(Physics.Raycast(ray, out hit, dashLength))
            {
                target = hit.point;
            }
            else
            {
                target = ray.GetPoint(dashLength);
            }

            target = OnNavMesh(target, playerRoot);
            dashTarget = target;
            anim.SetTrigger("DodgeRoll");

            //StartCoroutine(PlayerDash(target));

            //Debug.Log(startTime);
            //nextDodge = Time.time + dodgeRate;
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

        // -------------------------------------------------- \\


        /// <summary>
        /// Determines if the point the player wants to dash to is on the navmesh
        /// if not the target point is changed to the point they can dash to
        /// </summary>
        /// <param name="target"></param>
        /// <param name="playerRoot"></param>
        /// <returns></returns>
        private Vector3 OnNavMesh(Vector3 target, GameObject playerRoot)
        {
            var hit = new NavMeshHit();
            if (NavMesh.Raycast(transform.position, target, out hit, NavMesh.AllAreas))
            {
                target = hit.position;
                target = new Vector3(target.x, target.y + 1, target.z);
                return target;
            }
            else
            {
                Debug.DrawLine(transform.position, hit.position, Color.blue, dashLength);
                target = new Vector3(target.x, hit.position.y + (playerRoot.transform.localPosition.y * -1), target.z);
                return target;
            }
        }

        public void StartDash()
        {
            StartCoroutine(PlayerDash());
        }

        /// <summary>
        /// Lerps the Player from their current postion to the dodge target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private IEnumerator PlayerDash()
        {
            var dashComplete = 0.09f;
            float dashTime = 0;
            while (Vector3.Distance(transform.position, dashTarget) > dashComplete)
            {
                dashTime += dashSpeed * Time.deltaTime;
                var dashAmount = dashAnimation.Evaluate(dashTime);
                transform.position = Vector3.Lerp(transform.position, dashTarget, dashAmount);
                yield return null;
            }
            yield return new WaitForSeconds(dashCoolDown);
            canDash = true;
        }

        public Vector3 LookDirection
        {
            get
            {
                return lookDirection;
            }
            set
            {
                lookDirection = value;
            }
        }
    }
}
