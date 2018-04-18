using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hunter.Characters
{
    [RequireComponent(typeof(PlayerInventory))]
    public sealed class Player : Character, IMoveable, IAttack
    {
        #region Variables
        [Header("Combat Options")]
        public Transform weaponContainer;

        public float gunTrailLength = 1.5f;

        [Tooltip("The total ammount of time it should take for the wound bar to catch up to the health bar."), Range(0.1f, 10f)]
        public float healthSubtractionTime = 1;

        public float respawnTime = 3f;

        [Header("Movement and Rotation Options")]
        [Range(1, 20), Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 0 and 20.")]
        public float moveSpeed = 5f;
        [Range(1, 100), Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 0 and 20.")]
        public float maxRotationSpeed = 12f;

        public AnimationCurve rotationSpeedCurve;

        [Header("Dash Options")]
        public float dashMaxDistance = 3;
        public float dashCoolDown = 1;
        public float dashMaxSpeed = 3;
        public AnimationCurve dashSpeedCurve;
        public LayerMask dashValidLayers;

        [Header("World UI Options")]
        public Image interactPromptImage;

        private bool performingAction = false;
        private float rotationCurvePosition;

        private IEnumerator attackCR;
        private IEnumerator dashCR;

        public PlayerInventory Inventory { get; private set; }

        private List<IInteractable> nearbyInteractables = new List<IInteractable>();
        private IInteractable itemToInteractWith;
        #endregion

        #region Properties
        public bool PerformingAction
        {
            get
            {
                return performingAction;
            }

            set
            {
                performingAction = value;
                if (performingAction) { interactPromptImage.enabled = false; }
                else { CheckInteractImage(); }
            }
        }
        #endregion

        #region Unity Functions
        protected override void Awake()
        {
            base.Awake();
            Inventory = GetComponent<PlayerInventory>();
        }

        protected override void Start()
        {
            base.Start();
            transform.forward = Camera.main.transform.forward;
            EquipWeaponToCharacter(Inventory.GetMeleeWeaponAtIndex(0, weaponContainer));
            CheckInteractImage();
        }

        private void OnTriggerEnter(Collider other)
        {
            var interactableItem = other.GetComponent<IInteractable>();
            if (interactableItem != null && !nearbyInteractables.Contains(interactableItem))
            {
                nearbyInteractables.Add(interactableItem);
                CheckInteractImage();
            }

            var tutorialTrigger = other.GetComponent<TutorialTrigger>();
            if(tutorialTrigger != null && HUDManager.instance != null)
            {
                HUDManager.instance.ShowTutorialPrompt(tutorialTrigger.tutorialText, tutorialTrigger.controlSprite);
                tutorialTrigger.gameObject.SetActive(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var interactableItem = other.GetComponent<IInteractable>();
            if (interactableItem != null && nearbyInteractables.Contains(interactableItem))
            {
                nearbyInteractables.Remove(interactableItem);
                CheckInteractImage();
            }
        }
        #endregion

        #region Player Movement
        public void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            // We do not want the player to be able to move during the dash or item pickup
            if (PerformingAction) { return; }

            //Setting animation params
            anim.SetFloat("dirX", moveDirection.x);
            anim.SetFloat("dirY", moveDirection.z);
            anim.SetFloat("lookX", animLookDirection.x);
            anim.SetFloat("lookY", animLookDirection.z);
            anim.SetBool("moving", moveDirection.magnitude != 0);

            //Cacheing Rotation Transform since its used for both movement and rotation
            var characterRoot = RotationTransform;

            //Moving the Player
            //var turningSpeedSlow = Mathf.Clamp((moveDirection - lookDirection).magnitude, 1.0f, 1.5f);

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            agent.destination = characterRoot.position;
            agent.updateRotation = false;

            characterController.Move((moveDirection * Time.deltaTime));

            //Rotating the Player
            if (lookDirection.magnitude != 0)
            {
                var targetRotation = new Vector3(characterRoot.localEulerAngles.x, Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg, characterRoot.localEulerAngles.z);

                rotationCurvePosition = Mathf.Clamp01(rotationCurvePosition + Time.deltaTime);
                var rotationSpeed = rotationSpeedCurve.Evaluate(rotationCurvePosition) * maxRotationSpeed;

                characterRoot.localRotation = Quaternion.RotateTowards(characterRoot.localRotation, Quaternion.Euler(targetRotation), rotationSpeed);
            }
            else
            {
                rotationCurvePosition = 0;
            }

        }

        public void FootstepSoundAnimationEvent()
        {
            Fabric.EventManager.Instance?.PostEvent("Footstep", gameObject);
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
                //Debug.LogWarning("Dash is still on cooldown.");
                return;
            }
            else if (PerformingAction) { return; }

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
            PerformingAction = true;
            var startPosition = eyeLine.position;
            //Debug.Log(startPosition);
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
                PerformingAction = false;
                dashCR = null;
                yield break;
            }

            var closestNavMeshPointToTarget = Utility.GetClosestPointOnNavMesh(floorPointFromDashTarget, agent, transform);
            var dashTarget = closestNavMeshPointToTarget;
            //var dashTarget = new Vector3(closestNavMeshPointToTarget.x, closestNavMeshPointToTarget.y, closestNavMeshPointToTarget.z);

            anim.SetTrigger("DodgeRoll");
            StartCoroutine(SetStaminaBar(0, 0.3f));
            // PAUSE HERE FOR ANIMATION EVENT
            //Debug.Log("Pausing Dash Coroutine to wait for Animation Event...");
            StopCoroutine(dashCR);
            yield return null;

            // COROUTINE RESUMES HERE
            //Debug.Log("Animation Event has resumed the Coroutine.");

            var dashDistanceCheckMargin = 0.09f;
            var dashTime = 0f;

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
            PerformingAction = false;

            // Dont want players to be able to spam dash, so we have a cooldown which resets the Coroutine reference after (If that reference isn't null, that means we're still dashing)
            StartCoroutine(SetStaminaBar(1, dashCoolDown));
            yield return new WaitForSeconds(dashCoolDown);
            dashCR = null;
        }

        //TODO This needs to be moved into the HUD Manager
        private IEnumerator SetStaminaBar(float targetFill, float totalTime)
        {
            if (HUDManager.instance == null) { yield break; }

            var startFill = HUDManager.instance.staminaBar.fillAmount;
            var startTime = Time.time;
            var percentComplete = 0f;
            while (percentComplete < 1)
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
            else if (PerformingAction) { return; }
            attackCR = PlayAttackAnimation();
            StartCoroutine(attackCR);
        }

        public void WeaponAnimationEvent()
        {
            CurrentWeapon.StartAttackFromAnimationEvent();
        }

        public void MeleeWeaponSwingAnimationEvent()
        {
            Fabric.EventManager.Instance?.PostEvent("Player Sword Swing", gameObject);
        }

        public void SwordSwingParticleAnimationEvent()
        {
            if (CurrentWeapon != null && CurrentWeapon is Melee)
            {
                (CurrentWeapon as Melee).StartStopParticleSystem();
            }
        }

        public void RangedWeaponFireAnimationEvent()
        {
            Fabric.EventManager.Instance?.PostEvent("Player Luger Shot", gameObject);
        }

        public IEnumerator PlayAttackAnimation()
        {
            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            if (CurrentWeapon is Melee)
            {
                anim.SetTrigger("melee");
            }
            else if (CurrentWeapon is Ranged)
            {
                anim.SetTrigger("ranged");
            }
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            attackCR = null;
        }

        public void CycleWeapons(bool cycleUp)
        {
            Weapon newWeapon = null;

            if (cycleUp)
            {
                newWeapon = Inventory.CycleWeaponsUp(CurrentWeapon, weaponContainer);
            }
            else
            {
                newWeapon = Inventory.CycleWeaponsDown(CurrentWeapon, weaponContainer);
            }
            if (newWeapon == null || newWeapon == CurrentWeapon)
            {
                Debug.LogWarning("Cannot equip a null weapon or the weapon you are already holding.");
                return;
            }
            //TODO This should really be referencing a clip on the new weapon being equipped and playing that instead
            if (newWeapon is Melee) { Fabric.EventManager.Instance?.PostEvent("Player Draw Sword", gameObject); }
            else if (newWeapon is Ranged) { Fabric.EventManager.Instance?.PostEvent("Player Draw Luger", gameObject); }
            EquipWeaponToCharacter(newWeapon);
        }

        public void CycleElements(bool cycleUp)
        {
            Element newElement = null;
            if (cycleUp) { newElement = Inventory.CycleElementsUp(CurrentWeapon); }
            else { newElement = Inventory.CycleElementsDown(CurrentWeapon); }
            if (CurrentWeapon != null)
            {
                if (newElement == CurrentWeapon.WeaponElement) { return; }
                EquipElementToWeapon(newElement);
                Debug.Log("Equipped the " + Utility.ElementToElementOption(CurrentWeapon.WeaponElement) + " to the " + CurrentWeapon.name);
            }
        }

        public void SwitchWeaponType(bool switchToMelee)
        {
            if (switchToMelee && !(CurrentWeapon is Melee))
            {
                EquipWeaponToCharacter(Inventory.GetMeleeWeaponAtIndex(Inventory.MeleeWeaponIndex, weaponContainer));
            }
            else if (!switchToMelee && !(CurrentWeapon is Ranged))
            {
                EquipWeaponToCharacter(Inventory.GetRangedWeaponAtIndex(Inventory.RangedWeaponIndex, weaponContainer));
            }
        }

        protected override IEnumerator SubtractHealthFromCharacter(int damage, bool isCritical)
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

        public IEnumerator KillPlayer(float respawnTimer)
        {
            PerformingAction = true;
            anim.SetTrigger("Death");
            yield return new WaitForSeconds(respawnTimer);
        }
        #endregion

        #region Player Interaction
        public void Interact()
        {
            if (nearbyInteractables.Count == 0) { return; }

            // Always try to interact with Interactable Items first before Props
            var interactableItem = FirstNearbyInteractableItem();
            if (interactableItem != null)
            {
                PlayPickupAnimation(interactableItem);
            }
            else
            {
                foreach (var item in nearbyInteractables)
                {
                    item.FireInteraction(this);
                }
                nearbyInteractables.Clear();
                CheckInteractImage();
            }
        }

        private InteractableInventoryItem FirstNearbyInteractableItem()
        {
            return nearbyInteractables.OfType<InteractableInventoryItem>().FirstOrDefault();
        }

        private void PlayPickupAnimation(InteractableInventoryItem interactableItem)
        {
            itemToInteractWith = interactableItem;

            var triggerName = "PickupLow";
            if (weaponContainer.transform.position.y < interactableItem.transform.position.y)
            {
                triggerName = "PickupHigh";
            }

            anim.SetTrigger(triggerName);
            PerformingAction = true;
        }

        public void PickupItemAnimationEvent()
        {
            if (itemToInteractWith != null) { itemToInteractWith.FireInteraction(this); }
            nearbyInteractables.Remove(itemToInteractWith);
            PerformingAction = false;
        }

        public void CheckInteractImage()
        {
            if (AnyNearbyImportantItems() && !interactPromptImage.enabled)
            {
                interactPromptImage.enabled = true;
            }
            else if (nearbyInteractables.Count == 0 && interactPromptImage.enabled)
            {
                interactPromptImage.enabled = false;
            }
        }

        private bool AnyNearbyImportantItems()
        {
            if (nearbyInteractables.Count == 0) { return false; }
            foreach (var item in nearbyInteractables)
            {
                if (item.IsImportant()) { return true; }
            }
            return false;
        }
        #endregion

        #region Helper Functions
        private float SignedAngle(Vector3 a, Vector3 b)
        {
            return Vector3.Angle(a, b) * Mathf.Sign(Vector3.Cross(a, b).y);
        }
        #endregion
    }
}
