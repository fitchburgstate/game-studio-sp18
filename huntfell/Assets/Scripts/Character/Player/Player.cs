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

        public float respawnTime = 3f;

        [Header("Movement and Rotation Options")]
        [Range(1, 20), Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 0 and 20.")]
        public float moveSpeed = 5f;
        private Vector3 startingPosition;

        [Space]
        [Range(1, 100), Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 0 and 20.")]
        public float maxRotationSpeed = 12f;
        public AnimationCurve rotationSpeedCurve;
        private float rotationCurvePosition;

        [Header("Dash Options")]
        public float dashMaxHorizontalDistance = 3;
        public float dashMaxVerticalDistance = 20;
        [Space]
        public float dashMaxSpeed = 3;
        public AnimationCurve dashSpeedCurve;
        [Space]
        public float dashCoolDown = 1;
        [Space]
        public LayerMask dashInitialCheckBlockingLayers;
        public LayerMask dashFinalCheckBlockingLayers;

        [Header("Potion Options")]
        [Range(1, 5), SerializeField]
        private int potionCount = 1;
        [Range(1, 100)]
        public int maxShardsPerPotion = 30;
        [SerializeField]
        private int potionShardCount;
        [Range(1, 20)]
        public int potionRestoreAmount = 2;
        [Range(0.25f, 10)]
        public float potionRestoreInterval = 0.33f;
        [Range(1, 20)]
        public float potionRestoreLifeTime = 5;

        private int currentPotionIndex;

        [Header("World UI Options")]
        public Image interactPromptImage;

        // Action Coroutine Refs
        private IEnumerator attackAction;
        private IEnumerator attackFinisherCooldown;
        private IEnumerator dashAction;
        private IEnumerator dashCooldown;
        private IEnumerator pickupAction;

        // Inventory and Items
        public PlayerInventory Inventory { get; private set; }
        private List<IInteractable> nearbyInteractables = new List<IInteractable>();
        #endregion

        #region Properties
        public override bool PerformingMajorAction
        {
            get
            {
                return dashAction != null || pickupAction != null || IsDying;
            }
        }

        public override bool PerformingMinorAction
        {
            get
            {
                return attackAction != null || IsDying;
            }
        }

        public bool ActionsOnCooldown
        {
            get
            {
                return attackFinisherCooldown != null || dashCooldown != null;
            }
        }

        public override float CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                base.CurrentHealth = value;
                HUDManager.instance?.SetPlayerCurrentHealthBar(currentHealth / totalHealth);

                //if (currentHealth <= 0)
                //{
                //    var closestSpawnPoint = GameManager.instance?.GetClosestSpawnPoint(transform.position);
                //    Respawn(closestSpawnPoint);
                //}
            }
        }

        public override float TargetHealth
        {
            get
            {
                return targetHealth;
            }
            set
            {
                base.TargetHealth = value;
                HUDManager.instance?.SetPlayerTargetHealthBar(targetHealth / totalHealth);
            }
        }

        public int PotionShardCount
        {
            get
            {
                return potionShardCount;
            }

            set
            {
                potionShardCount = Mathf.Clamp(value, 0, maxShardsPerPotion * PotionCount);
                UpdateDecanters();
            }
        }

        public int PotionCount
        {
            get
            {
                return potionCount;
            }

            set
            {
                potionCount = value;
                HUDManager.instance?.EnableDecanter(potionCount - 1);
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

            if(GameManager.instance != null)
            {
                transform.forward = GameManager.instance.isometricFollowCM.transform.forward;
            }
            else
            {
                transform.forward = Camera.main.transform.forward;
            }
            startingPosition = transform.position;
            if(GameManager.instance != null && !GameManager.instance.loadTitleScreen && Application.isEditor)
            {
                InitPlayerUI();
            }
            CheckInteractImage();
        }

        private void OnTriggerEnter(Collider other)
        {
            var interactableItem = other.GetComponent<IInteractable>();
            if (interactableItem != null)
            {
                if(other.GetComponent<InteractablePotionShard>() != null)
                {
                    interactableItem.FireInteraction(this);
                }
                else if (!nearbyInteractables.Contains(interactableItem))
                {
                    AddNearbyInteractable(interactableItem);
                }
                return;
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
                RemoveNearbyInteractable(interactableItem);
            }
        }
        
        #endregion

        public void InitPlayerUI ()
        {
            Inventory.AddStartingItems();
            UpdateDecanters();
        }

        #region Player Movement
        public void Move(Vector3 moveDirection, Vector3 lookDirection)
        {
            // We do not want the player to be able to move during any big actions (dash, pickup, death, etc)
            if (PerformingMajorAction) { return; }

            //Setting animation params
            anim.SetFloat("dirX", moveDirection.x);
            anim.SetFloat("dirY", moveDirection.z);
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

        #region Non-Core Movement Functions
        public void FootstepSoundAnimationEvent()
        {
            Fabric.EventManager.Instance?.PostEvent("Footstep", gameObject);
        }

        public void Move(Transform target)
        {
            // This should stay empty.
        }
        #endregion

        #endregion

        #region Player Dash
        public void Dash()
        {
            if (PerformingMajorAction || ActionsOnCooldown) { return; }

            dashAction = PlayDashAnimation();
            StartCoroutine(dashAction);
        }

        public void DashAnimationEvent()
        {
            if (dashAction == null)
            {
                Debug.LogError("The Dash Coroutine reference is null despite the animation event being called. This reference should have been set when you gave the dash input.");
                return;
            }
            // RESUMES DASH COROUTINE
            StartCoroutine(dashAction);
        }

        private IEnumerator PlayDashAnimation()
        {
            var startPosition = EyeLineTransform.position;
            var characterForward = RotationTransform.forward;
            var dashDirectionTarget = new Vector3();

            // Raycast to determine target point for dodge destination on the X and Z axis
            var hit = new RaycastHit();
            var ray = new Ray(startPosition, characterForward);
            if (Physics.Raycast(ray, out hit, dashMaxHorizontalDistance, dashInitialCheckBlockingLayers))
            {
                dashDirectionTarget = hit.point;
            }
            else
            {
                dashDirectionTarget = ray.GetPoint(dashMaxHorizontalDistance);
            }
            Debug.DrawLine(startPosition, dashDirectionTarget, Color.red, 5);

            // Raycast to determine target point for dodge destination on the Y axis.
            hit = new RaycastHit();
            ray = new Ray(dashDirectionTarget, Vector3.down);
            // Setting this to the start position because if we RayCast down and dont get a hit, that means you casted off the map. If you do we cancel the dash.
            var floorPointFromDashTarget = startPosition;
            if (Physics.Raycast(ray, out hit, dashMaxVerticalDistance, dashInitialCheckBlockingLayers))
            {
                floorPointFromDashTarget = hit.point;
                Debug.DrawLine(dashDirectionTarget, floorPointFromDashTarget, Color.blue, 5);
            }
            else
            {
                Debug.LogWarning("You tried to Dash into the void. Canceling the dash.");
                dashAction = null;
                yield break;
            }

            var dashTarget = Utility.GetClosestPointOnNavMesh(floorPointFromDashTarget, agent, transform);
            if (Physics.Linecast(startPosition, dashTarget, out hit, dashFinalCheckBlockingLayers) || Mathf.Abs(startPosition.y - dashTarget.y) > dashMaxVerticalDistance)
            {
                Debug.LogWarning($"Your dash would have brought you somewhere you weren't supposed to go!");
                dashAction = null;
                yield break;
            }

            anim.SetTrigger("DodgeRoll");
            StartCoroutine(SetStaminaBar(0, 0.3f));

            // PAUSE HERE FOR ANIMATION EVENT
            StopCoroutine(dashAction);
            yield return null;

            // COROUTINE RESUMES HERE
            var dashDistanceCheckMargin = 0.09f;
            var dashTime = 0f;

            // Turn off the normal means of moving / constraining the player since we are doing that ourselves
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
            agent.enabled = true;

            // Dont want players to be able to spam dash, so we have a cooldown which resets the Coroutine reference after (If that reference isn't null, that means we're still dashing)
            dashCooldown = DashCooldown();
            StartCoroutine(dashCooldown);
            yield return null;

            dashAction = null;
        }

        private IEnumerator DashCooldown ()
        {
            StartCoroutine(SetStaminaBar(1, dashCoolDown));
            yield return new WaitForSeconds(dashCoolDown);
            dashCooldown = null;
        }

        //TODO This needs to be moved into the HUD Manager
        private IEnumerator SetStaminaBar(float targetFill, float totalTime)
        {
            if (HUDManager.instance == null) { yield break; }

            var startFill = HUDManager.instance.playerStaminaBar.fillAmount;
            var startTime = Time.time;
            var percentComplete = 0f;
            while (percentComplete < 1)
            {
                var elapsedTime = Time.time - startTime;
                percentComplete = Mathf.Clamp01(elapsedTime / totalTime);
                HUDManager.instance.playerStaminaBar.fillAmount = Mathf.Lerp(startFill, targetFill, percentComplete);
                yield return null;
            }
        }
        #endregion

        #region Player Combat
        public void Attack()
        {
            if (PerformingMajorAction || PerformingMinorAction || CurrentWeapon == null) { return; }

            attackAction = PlayAttackAnimation();
            StartCoroutine(attackAction);
        }

        public void AttackAnimationEvent()
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
            else if (CurrentWeapon is Ranged)
            {
                anim.SetTrigger("ranged");
            }

            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            attackAction = null;
        }

        #region Non-Core Attack Functions
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

        public void CycleWeapons(bool cycleUp)
        {
            if (PerformingMinorAction) { return; }

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
            //else if (newWeapon is Ranged) { Fabric.EventManager.Instance?.PostEvent("Player Draw Luger", gameObject); }
            EquipWeaponToCharacter(newWeapon);
        }

        public void CycleElements(bool cycleUp)
        {
            if (PerformingMinorAction) { return; }

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

        public void SwitchWeaponType (bool switchToMelee)
        {
            //if (switchToMelee && !(CurrentWeapon is Melee))
            //{
            //    EquipWeaponToCharacter(Inventory.GetMeleeWeaponAtIndex(Inventory.MeleeWeaponIndex, weaponContainer));
            //}
            //else if (!switchToMelee && !(CurrentWeapon is Ranged))
            //{
            //    EquipWeaponToCharacter(Inventory.GetRangedWeaponAtIndex(Inventory.RangedWeaponIndex, weaponContainer));
            //}
        }
        #endregion

        #endregion

        #region Player Health

        public void UsePotion ()
        {
            if(PerformingMinorAction || PotionShardCount < maxShardsPerPotion || TargetHealth == totalHealth) {
                Debug.LogWarning("Cannot use potion at this time.");
                return;
            }
            gameObject.AddComponent<HealOverTime>().InitializeEffect(potionRestoreAmount, potionRestoreInterval, potionRestoreLifeTime, null, this, null);
            PotionShardCount -= maxShardsPerPotion;
        }

        private void UpdateDecanters ()
        {
            for (int i = 0; i < potionCount; i++)
            {
                HUDManager.instance?.SetDecanterInfo(i, PotionShardCount, maxShardsPerPotion);
            }
        }

        protected override IEnumerator SubtractHealthFromCharacter (int damage, bool isCritical)
        {
            Fabric.EventManager.Instance?.PostEvent("Player Hit", gameObject);
            yield return base.SubtractHealthFromCharacter(damage, isCritical);
        }

        protected override IEnumerator AddHealthToCharacter (int restoreAmount, bool isCritical)
        {
            yield return base.AddHealthToCharacter(restoreAmount, isCritical);
        }

        protected override IEnumerator KillCharacter ()
        {
            invincible = true;
            agent.enabled = false;
            characterController.enabled = false;
            GameManager.instance.DeviceManager.gameInputEnabled = false;

            anim.SetFloat("dirX", 0);
            anim.SetFloat("dirY", 0);
            anim.SetBool("moving", false);
            anim.SetTrigger("isDead");

            yield return GameManager.instance?.FadeScreen(Color.black, FadeType.Out);

            var spawnPoint = GameManager.instance?.GetClosestSpawnPoint(transform.position);
            var spawnPosition = startingPosition;
            if (spawnPoint != null) { spawnPosition = spawnPoint.transform.position; }

            transform.position = Utility.GetClosestPointOnNavMesh(spawnPosition, agent, transform);

            yield return new WaitForSeconds(respawnTime);

            var statusEffects = GetComponents<StatusEffect>();
            for (var i = 0; i < statusEffects.Length; i++)
            {
                Destroy(statusEffects[i]);
            }
            TargetHealth = totalHealth;

            PotionShardCount = 0;
            UpdateDecanters();

            yield return GameManager.instance?.FadeScreen(Color.black, FadeType.In);

            invincible = false;
            agent.enabled = true;
            characterController.enabled = true;
            GameManager.instance.DeviceManager.gameInputEnabled = true;

            deathAction = null;
            yield return null;
        }
        #endregion

        #region Player Interaction
        public void Interact()
        {
            if (PerformingMajorAction) { return; }

            if (nearbyInteractables.Count == 0) { return; }

            // Always try to interact with Interactable Items first before Props
            var interactableItem = FirstNearbyInteractableItem();
            if (interactableItem != null)
            {
                pickupAction = PlayPickupAnimation(interactableItem);
                StartCoroutine(pickupAction);
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

        public void PickupAnimationEvent()
        {
            if (pickupAction == null)
            {
                Debug.LogError("The Pickup Coroutine reference is null despite the animation event being called. This reference should have been set when you gave the pickup input.");
                return;
            }
            // RESUMES PICKUP COROUTINE
            StartCoroutine(pickupAction);
        }

        private IEnumerator PlayPickupAnimation(InteractableInventoryItem interactableItem)
        {
            var footHeightDif = Mathf.Abs(interactableItem.transform.position.y - transform.position.y);
            var handHeightDif = Mathf.Abs(interactableItem.transform.position.y - weaponContainer.transform.position.y);
            var triggerName = "PickupLow";

            if (handHeightDif < footHeightDif)
            {
                triggerName = "PickupHigh";
            }

            anim.SetTrigger(triggerName);

            // PAUSE HERE FOR ANIMATION EVENT
            StopCoroutine(pickupAction);
            yield return null;

            // COROUTINE RESUMES HERE
            interactableItem.FireInteraction(this);
            RemoveNearbyInteractable(interactableItem);

            pickupAction = null;
        }

        #region Non-Core Interaction Functions
        private InteractableInventoryItem FirstNearbyInteractableItem()
        {
            return nearbyInteractables.OfType<InteractableInventoryItem>().FirstOrDefault();
        }

        public void CheckInteractImage()
        {
            if (AnyNearbyImportantItems())
            {
                interactPromptImage.enabled = true;
            }
            else
            {
                interactPromptImage.enabled = false;
            }
        }

        private bool AnyNearbyImportantItems()
        {
            if (nearbyInteractables.Count == 0) { return false; }
            foreach (var item in nearbyInteractables)
            {
                if (item.IsImportant) { return true; }
            }
            return false;
        }

        private void AddNearbyInteractable (IInteractable interactableItem)
        {
            nearbyInteractables.Add(interactableItem);
            CheckInteractImage();
        }

        public void RemoveNearbyInteractable (IInteractable interactableItem)
        {
            nearbyInteractables.Remove(interactableItem);
            CheckInteractImage();
        }
        #endregion

        #endregion

        #region Helper / Unused Functions
        private float SignedAngle(Vector3 a, Vector3 b)
        {
            return Vector3.Angle(a, b) * Mathf.Sign(Vector3.Cross(a, b).y);
        }

        public IEnumerator AttackAnimation()
        {
            yield return null;
        }

        public void Move(Vector3 target, float finalSpeed)
        {
            if (IsDying) { return; }
            // This should be left empty.
        }
        #endregion
    }
}
