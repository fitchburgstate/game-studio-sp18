using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hunter.Character
{
    public sealed class Player : Character, IMoveable, IAttack
    {
        #region Variables
        [Header("Combat Options")]
        public Transform weaponContainer;
        public float gunTrailLength = 1.5f;
        [Tooltip("The total ammount of time it should take for the wound bar to catch up to the health bar."), Range(0.1f, 10f)]
        public float healthSubtractionTime = 1;

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
        public LayerMask dashValidLayers;

        [Header("World UI Options")]
        public Image interactPromptImage;

        private bool canMove = true;
        private float speedRamp;
        private IEnumerator attackCR;
        private IEnumerator dashCR;
        private List<IInteractable> itemsPlayerIsStandingIn = new List<IInteractable>();

        #endregion

        #region Unity Messages
        protected override void Start()
        {
            base.Start();
            EquipWeaponToCharacter(InventoryManager.instance.CycleMeleeWeapons(weaponContainer));
            CheckInteractImage();
        }

        private void OnTriggerEnter (Collider other)
        {
            var interactableItem = other.GetComponent<IInteractable>();
            if (interactableItem != null && !itemsPlayerIsStandingIn.Contains(interactableItem))
            {
                itemsPlayerIsStandingIn.Add(interactableItem);
                CheckInteractImage();
            }
        }

        private void OnTriggerExit (Collider other)
        {
            var interactableItem = other.GetComponent<IInteractable>();
            if (interactableItem != null && itemsPlayerIsStandingIn.Contains(interactableItem))
            {
                itemsPlayerIsStandingIn.Remove(interactableItem);
                CheckInteractImage();
            }
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

            var turningSpeedSlow = Mathf.Clamp((moveDirection - lookDirection).magnitude, 1.0f, 1.5f); 

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            agent.destination = characterRoot.position;
            agent.updateRotation = false;

            if (moveDirection.magnitude != 0 || lookDirection.magnitude != 0)
            {
                var targetRotation = new Vector3(characterRoot.localEulerAngles.x, Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg, characterRoot.localEulerAngles.z);

                speedRamp = Mathf.Clamp01(speedRamp + Time.deltaTime);
                var changeChar = rotationSpeedCurve.Evaluate(speedRamp) * rotationMaxSpeed;

                characterRoot.localRotation = Quaternion.RotateTowards(characterRoot.localRotation, Quaternion.Euler(targetRotation), changeChar);
            }
            else
            {
                speedRamp = 0;
            }
            characterController.Move((moveDirection * Time.deltaTime) / turningSpeedSlow);
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
            if (Physics.Raycast(ray, out hit, dashMaxDistance, dashValidLayers))
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
            if (Physics.Raycast(ray, out hit, dashMaxDistance, dashValidLayers))
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
            StartCoroutine(SetStaminaBar(0, 0.3f));
            // PAUSE HERE FOR ANIMATION EVENT
            Debug.Log("Pausing Dash Coroutine to wait for Animation Event...");
            StopCoroutine(dashCR);
            yield return null;

            // COROUTINE RESUMES HERE
            Debug.Log("Animation Event has resumed the Coroutine.");

            var dashDistanceCheckMargin = 0.09f;
            float dashTime = 0;

            // Turn off the normal means of moving / constraining the player since we are doing that ourselves
            //characterController.enabled = false;
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
            //characterController.enabled = true;
            agent.enabled = true;

            // Let the player move again after they reached their destination
            yield return null;
            canMove = true;

            // Dont want players to be able to spam dash, so we have a cooldown which resets the Coroutine reference after (If that reference isn't null, that means we're still dashing)
            StartCoroutine(SetStaminaBar(1, dashCoolDown));
            yield return new WaitForSeconds(dashCoolDown);
            dashCR = null;
        }

        //TODO This needs to be moved into the HUD Manager
        private IEnumerator SetStaminaBar(float targetFill, float totalTime)
        {
            if (HUDManager.instance == null) { yield break; }

            float startFill = HUDManager.instance.staminaBar.fillAmount;
            float startTime = Time.time;
            var percentComplete = 0f;
            while(percentComplete < 1)
            {
                var elapsedTime = Time.time - startTime;
                percentComplete = Mathf.Clamp01(elapsedTime / totalTime);
                HUDManager.instance.staminaBar.fillAmount = Mathf.Lerp(startFill, targetFill, percentComplete);
                yield return null;
            }
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

        public void FootstepSoundAnimationEvent()
        {
            Fabric.EventManager.Instance.PostEvent("Footstep", gameObject);
        }

        public void MeleeWeaponSwingAnimationEvent()
        {
            Fabric.EventManager.Instance.PostEvent("Player Sword Swing", gameObject);
        }

        public void SwordSwingParticleAnimationEvent()
        {
            if(CurrentWeapon != null && CurrentWeapon is Melee)
            {
                (CurrentWeapon as Melee).StartStopParticleSystem();
            }
        }

        public void RangedWeaponFireAnimationEvent()
        {
            Fabric.EventManager.Instance.PostEvent("Player Luger Shot", gameObject);
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
            }
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            attackCR = null;
        }

        public void SwitchWeapon(bool cycleRanged, bool cycleMelee)
        {
            if((cycleMelee && CurrentWeapon is Range) || (cycleRanged && CurrentWeapon is Melee))
            {
                return;
            }

            CurrentWeapon?.gameObject.SetActive(false);

            if (cycleMelee)
            {
                EquipWeaponToCharacter(InventoryManager.instance.CycleRangedWeapons(weaponContainer));
                Fabric.EventManager.Instance.PostEvent("Player Draw Luger", gameObject);
            }
            else if (cycleRanged)
            {
                EquipWeaponToCharacter(InventoryManager.instance.CycleMeleeWeapons(weaponContainer));
                Fabric.EventManager.Instance.PostEvent("Player Draw Sword", gameObject);
            }

            if (CurrentWeapon != null)
            {
                CurrentWeapon?.gameObject.SetActive(true);
                Debug.Log("Equipped the " + CurrentWeapon.name);
            }
        }

        public void SwitchElement (bool cycleUp, bool cycleDown)
        {
            if (cycleUp) { EquipElementToWeapon(InventoryManager.instance.CycleElementsUp()); }
            else if (cycleDown) { EquipElementToWeapon(InventoryManager.instance.CycleElementsDown()); }

            if (CurrentWeapon != null)
            {
                Debug.Log("Equipped the " + Utility.ElementToElementOption(CurrentWeapon.weaponElement) + " to the " + CurrentWeapon.name);
            }
        }

        protected override IEnumerator SubtractHealthFromCharacter (int damage, bool isCritical)
        {
            var startHealth = CurrentHealth;
            var targetHealth = startHealth - damage;

            if (!isCritical || healthSubtractionTime == 0)
            {
                var startTime = Time.time;
                var percentComplete = 0f;
                if (HUDManager.instance != null) { HUDManager.instance.healthBar.fillAmount = targetHealth / totalHealth; }

                while (percentComplete < 1)
                {
                    var elapsedTime = Time.time - startTime;
                    percentComplete = Mathf.Clamp01(elapsedTime / healthSubtractionTime);

                    CurrentHealth = Mathf.Lerp(startHealth, targetHealth, percentComplete);
                    if (HUDManager.instance != null) { HUDManager.instance.woundBar.fillAmount = CurrentHealth / totalHealth; }

                    yield return null;
                }
            }
            else
            {
                CurrentHealth = targetHealth;
                if (HUDManager.instance != null)
                {
                    HUDManager.instance.healthBar.fillAmount = CurrentHealth / totalHealth;
                    HUDManager.instance.woundBar.fillAmount = CurrentHealth / totalHealth;
                }

            }
        }

        public override void RestoreHealthToCharacter(int amount)
        {
            StopCoroutine("SubtractHealthFromCharacter");
            CurrentHealth += amount;
            if (HUDManager.instance != null)
            {
                HUDManager.instance.healthBar.fillAmount = CurrentHealth / totalHealth;
                HUDManager.instance.woundBar.fillAmount = CurrentHealth / totalHealth;
            }
        }

        // TODO PostMaloned until after PAX East
        //public void AimWeapon()
        //{
        //    canMove = false;
        //}
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

        private void CheckInteractImage ()
        {
            if(itemsPlayerIsStandingIn.Count > 0 && !interactPromptImage.enabled)
            {
                interactPromptImage.enabled = true;
            }
            else if(itemsPlayerIsStandingIn.Count == 0 && interactPromptImage.enabled)
            {
                interactPromptImage.enabled = false;
            }
        }

        public void TriggerItemInteractions ()
        {
            foreach (var item in itemsPlayerIsStandingIn)
            {
                item.Interact(this);
            }
            itemsPlayerIsStandingIn.Clear();
            CheckInteractImage();
        }

        public void PlayPickupAnimation (Transform itemTransform)
        {
            var triggerName = "PickupLow";
            if(weaponContainer.transform.position.y < itemTransform.position.y)
            {
                triggerName = "PickupHigh";
            }
            StartCoroutine(PickupAnim(triggerName));
        }

        private IEnumerator PickupAnim (string triggerName)
        {
            var pim = GetComponent<PlayerInputModule>();
            if(pim != null) { pim.characterInputEnabled = false; }
            anim.SetTrigger(triggerName);
            yield return new WaitForSeconds(2.2f);
            if (pim != null) { pim.characterInputEnabled = true; }
        }
        #endregion
    }
}
