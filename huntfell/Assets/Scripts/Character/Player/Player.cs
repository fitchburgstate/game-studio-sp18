using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public sealed class Player : Character, IMoveable, IAttack
    {
        #region Variables
        [Header("Player Weapons")]
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
        public float gunTrailLength = 1f;

        [Header("Movement and Rotation Options")]
        [Range(1, 20), Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 0 and 20.")]
        public float moveSpeed = 5f;
        [Range(1, 2000), Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 0 and 20.")]
        public float rotationMaxSpeed = 12f;

        public AnimationCurve rotationSpeedCurve;

        [Header("Dash Options")]
        public float dashMaxDistance = 3;
        public float dashCoolDown = 1;
        public float dashMaxSpeed = 3;
        public AnimationCurve dashSpeedCurve;
        public float dashMaxHeight = 2;
        public AnimationCurve dashHeightCurve;

        private bool canMove = true;
        private float speedRamp;
        private IEnumerator attackCR;
        private IEnumerator dashCR;
        private IEnumerator gunTrailCR;
        #endregion

        #region Unity Messages
        protected override void Start()
        {
            base.Start();

            if (rangedWeapon != null)
            {
                rangedWeapon.gameObject.SetActive(false);
            }
            // Always start with your melee weapon
            EquipWeaponToCharacter(meleeWeapon);
        }
        #endregion

        #region Player Movement
        public void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            // We do not want the player to be able to move during the dash
            if (!canMove)
            {
                return;
            }

            anim.SetFloat("dirX", moveDirection.x);
            anim.SetFloat("dirY", moveDirection.z);
            anim.SetFloat("lookX", animLookDirection.x);
            anim.SetFloat("lookY", animLookDirection.z);
            anim.SetBool("moving", moveDirection.magnitude != 0);

            var characterRoot = RotationTransform;

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            agent.destination = characterRoot.position;
            agent.updateRotation = false;

            if (moveDirection.magnitude != 0 || lookDirection.magnitude != 0)
            {
                var targetRotation = new Vector3(characterRoot.localEulerAngles.x, Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg, characterRoot.localEulerAngles.z);

                speedRamp = Mathf.Clamp(speedRamp + Time.deltaTime, 0, 1);
                var changeChar = rotationSpeedCurve.Evaluate(speedRamp) * rotationMaxSpeed;

                characterRoot.localRotation = Quaternion.RotateTowards(characterRoot.localRotation, Quaternion.Euler(targetRotation), changeChar);
            }
            else
            {
                speedRamp = 0;
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }

        public void Move(Transform target)
        {
            // This should stay empty.
        }
        #endregion

        #region Player Dash
        /// <summary>
        /// Dashes the Player in the direction they are facing.
        /// </summary>
        public void Dash()
        {
            if (dashCR != null)
            {
                Debug.LogWarning("Dash is still on cooldown.");
                return;
            }
            dashCR = PlayDashAnimation();
            // STARTS DASH COROUTINE
            StartCoroutine(dashCR);
        }

        public void DashAnimationEvent()
        {
            if (dashCR == null)
            {
                Debug.LogError("The Dash Coroutine reference is null despite the animation event being called. This reference should have been set when you gave the dash input.");
                return;
            }
            // RESUMES DASH COROUTINE
            StartCoroutine(dashCR);
        }

        /// <summary>
        /// Lerps the Player from their current postion to the dodge target.
        /// </summary>
        private IEnumerator PlayDashAnimation()
        {
            //No moving during the dash movement
            canMove = false;
            var startPosition = eyeLine.position;
            Debug.Log(startPosition);
            var characterForward = RotationTransform.forward;
            var dashDirectionTarget = new Vector3();

            // Raycast to determine target point for dodge destination on the X and Z axis
            var hit = new RaycastHit();
            var ray = new Ray(startPosition, characterForward);
            if (Physics.Raycast(ray, out hit, dashMaxDistance))
            {
                dashDirectionTarget = hit.point;
            }
            else
            {
                dashDirectionTarget = ray.GetPoint(dashMaxDistance);
            }
            Debug.DrawLine(startPosition, dashDirectionTarget, Color.red, 5);

            // Raycast to determine target point for dodge destination on the Y axis.
            hit = new RaycastHit();
            ray = new Ray(dashDirectionTarget, Vector3.down);
            // Setting this to the start position because if we RayCast down and dont get a hit, that means you casted off the map. If you do we cancel the dash.
            var floorPointFromDashTarget = startPosition;
            if (Physics.Raycast(ray, out hit, dashMaxDistance))
            {
                floorPointFromDashTarget = hit.point;
                Debug.DrawLine(dashDirectionTarget, floorPointFromDashTarget, Color.blue, 5);
            }
            else
            {
                Debug.LogWarning("You tried to Dash into the void. Canceling the dash.");
                canMove = true;
                dashCR = null;
                yield break;
            }

            var closestNavMeshPointToTarget = GetClosestPointOnNavMesh(floorPointFromDashTarget);
            var dashTarget = closestNavMeshPointToTarget;
            //var dashTarget = new Vector3(closestNavMeshPointToTarget.x, closestNavMeshPointToTarget.y, closestNavMeshPointToTarget.z);

            anim.SetTrigger("DodgeRoll");

            // PAUSE HERE FOR ANIMATION EVENT
            Debug.Log("Pausing Dash Coroutine to wait for Animation Event...");
            StopCoroutine(dashCR);
            yield return null;

            // COROUTINE RESUMES HERE
            Debug.Log("Animation Event has resumed the Coroutine.");

            var dashDistanceCheckMargin = 0.09f;
            float dashTime = 0;

            // Turn off the normal means of moving / constraining the player since we are doing that ourselves
            characterController.enabled = false;
            agent.enabled = false;

            // TODO: Make it so the player properly lerps to the position using the Animation Curves
            // This is where we actually move the player
            while (Vector3.Distance(transform.position, dashTarget) > dashDistanceCheckMargin)
            {
                dashTime += dashMaxSpeed * Time.deltaTime;
                var dashAmount = dashSpeedCurve.Evaluate(dashTime);
                transform.position = Vector3.Lerp(transform.position, dashTarget, dashAmount);
                yield return null;
            }
            characterController.enabled = true;
            agent.enabled = true;

            // Let the player move again after they reached their destination
            yield return null;
            canMove = true;

            // Dont want players to be able to spam dash, so we have a cooldown which resets the Coroutine reference after (If that reference isn't null, that means we're still dashing)
            yield return new WaitForSeconds(dashCoolDown);
            dashCR = null;
        }
        #endregion

        #region Player Combat
        public void Attack()
        {
            if (attackCR != null) { return; }
            attackCR = PlayAttackAnimation();
            StartCoroutine(attackCR);
        }

        public void WeaponAnimationEvent()
        {
            CurrentWeapon.StartAttackFromAnimationEvent();
        }

        public IEnumerator PlayAttackAnimation()
        {
            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            if (CurrentWeapon is Melee)
            {
                anim.SetTrigger("melee");
            }
            else if (CurrentWeapon is Range)
            {
                anim.SetTrigger("ranged");
                if (gunTrailCR == null && rangedWeapon.lastTarget != null)
                {
                    gunTrailCR = MakeGunTrail();
                    StartCoroutine(gunTrailCR);
                }
                var gunTrail = gameObject.GetComponent<LineRenderer>();
                if (gunTrail != null && gunTrailCR == null)
                {
                    StopCoroutine(gunTrailCR);
                }
            }
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            attackCR = null;
        }

        public void SwitchWeapon(bool cycleRanged, bool cycleMelee)
        {
            if (CurrentWeapon is Melee && cycleMelee)
            {
                meleeWeapon.gameObject.SetActive(false);
                rangedWeapon.gameObject.SetActive(true);
                EquipWeaponToCharacter(rangedWeapon);
            }
            else if (CurrentWeapon is Range && cycleRanged)
            {
                rangedWeapon.gameObject.SetActive(false);
                meleeWeapon.gameObject.SetActive(true);
                EquipWeaponToCharacter(meleeWeapon);
            }
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// Determines if the point the player wants to dash to is on the navmesh,
        /// if not, the target point is changed to the point they can dash to.
        /// </summary>
        private Vector3 GetClosestPointOnNavMesh(Vector3 target)
        {
            var hit = new NavMeshHit();
            // This gives us a sample radius for the NavMesh check relative to our NavMesh agent size, so given either scenerio where we are passed a floor point or the character's position, we should be able to find a point on the NavMesh
            var sampleRadius = agent.height + agent.baseOffset;

            if (NavMesh.SamplePosition(target, out hit, sampleRadius, NavMesh.AllAreas))
            {
                target = hit.position;
                Debug.Log("Hit Position of NavMesh Sample from RayCast: " + target);
            }
            else if (NavMesh.SamplePosition(transform.position, out hit, sampleRadius, NavMesh.AllAreas))
            {
                target = hit.position;
                Debug.LogWarning("Could not find a NavMesh point with the given target. Falling back to the character's current position. Hit Position of NavMesh Sample from current position: " + target);
            }
            else
            {
                target = transform.position;
                Debug.LogError("Could not find a closest point on the NavMesh from either the RayCast Hit Position or the character's current location. Are you sure the character is on the NavMesh?");
            }
            return target;
        }

        private float SignedAngle(Vector3 a, Vector3 b)
        {
            return Vector3.Angle(a, b) * Mathf.Sign(Vector3.Cross(a, b).y);
        }

        private IEnumerator MakeGunTrail()
        {
            var gunTrail = gameObject.AddComponent<LineRenderer>();
            var width = 0.1f;
            var startPoint = rangedWeapon.transform.position;
            var endPoint = new Vector3();
            if (rangedWeapon.lastTarget != null)
            {
                endPoint = rangedWeapon.lastTarget.transform.position;
            }

            gunTrail.startColor = Color.grey;
            gunTrail.endColor = Color.grey;
            gunTrail.material = new Material(Shader.Find("Particles/Additive"));
            gunTrail.startWidth = width;
            gunTrail.endWidth = width;
            gunTrail.positionCount = 2;
            gunTrail.numCapVertices = 1;
            gunTrail.useWorldSpace = false;

            gunTrail.SetPosition(0, startPoint);
            gunTrail.SetPosition(1, endPoint);

            yield return new WaitForSeconds(gunTrailLength);
            Destroy(gunTrail);
            gunTrailCR = null;
        }
        #endregion
    }
}
