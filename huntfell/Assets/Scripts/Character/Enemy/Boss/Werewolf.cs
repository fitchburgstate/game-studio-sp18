using Hunter.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Characters
{
    public class Werewolf : Boss, IMoveable, IAttack, IUtilityBasedAI
    {
        #region Variables
        /// <summary>
        /// Determines whether debug logs relating to the boss will appear in the console window.
        /// </summary>
        [Header("Toggles"), Tooltip("Determines whether debug logs relating to the boss will appear in the console window.")]
        public bool showDebugLogs = false;

        /// <summary>
        /// A boolean to determine whether the boss can apply elements to himself during every phase change.
        /// </summary>
        [Tooltip("Determines whether the boss can change elements throughout the fight.")]
        public bool canChangeElements = true;

        /// <summary>
        /// A boolean to determine whether the boss moves faster during his attack phase or not.
        /// </summary>
        [Tooltip("Determines whether the boss gains speed while attacking or not.")]
        public bool moveFasterWhileAttacking = true;

        /// <summary>
        /// Determines how much speed is added to the werewolf while attacking.
        /// </summary>
        [Range(0f, 10f), Tooltip("How much faster the boss gets while attacking."), Header("Movement Options")]
        public float speedBoost = 2f;

        /// <summary>
        /// The mobs that will spawn in phase 2. This should be set in the inspector.
        /// </summary>
        [Header("Phases Options"), Tooltip("The mobs that will spawn when the boss enters phase 2.")]
        public List<GameObject> phaseTwoMobs = new List<GameObject>();

        /// <summary>
        /// The mobs that will spawn in phase 3. This should be set in the inspector.
        /// </summary>
        [Tooltip("The mobs that will spawn when the boss enters phase 3.")]
        public List<GameObject> phaseThreeMobs = new List<GameObject>();

        /// <summary>
        /// The weapon that belongs in the left hand of the boss.
        /// </summary>
        public Melee leftClawWeapon;

        /// <summary>
        /// The weapon that belongs in the right hand of the boss.
        /// </summary>
        public Melee rightClawWeapon;

        /// <summary>
        /// The weapon that will be used during the bosses heavy attack.
        /// </summary>
        public Melee doubleSwipeWeapon;

        /// <summary>
        /// The max distance that the boss can lunge.
        /// </summary>
        [Header("Lunge Options")]
        public float lungeMaxDistance = 3f;

        /// <summary>
        /// The cooldown time before the boss can lunge again.
        /// </summary>
        public float lungeCooldownTime = 5f;

        /// <summary>
        /// The max speed that the boss can lunge at.
        /// </summary>
        public float lungeMaxSpeed = 3f;

        /// <summary>
        /// The animation curve that the boss will follow while lunging.
        /// </summary>
        public AnimationCurve lungeSpeedCurve;

        /// <summary>
        /// The valid layers that the lunge can raycast to.
        /// </summary>
        public LayerMask lungeValidLayers;

        #region Variables not shown in the Inspector
        /// <summary>
        /// A boolean to determine whether the boss can lunge or not.
        /// </summary>
        [HideInInspector]
        public bool canlunge = true;

        /// <summary>
        /// Determines whether the boss is already attacking or not.
        /// </summary>
        private bool isAttacking = false;

        /// <summary>
        /// Determines whether the boss is already howling or not.
        /// </summary>
        private bool isHowling = false;

        /// <summary>
        /// Determines whether the boss is already lunging or not.
        /// </summary>
        private bool isLunging = false;

        /// <summary>
        /// Determines whether the boss is outside of the arena or not. This
        /// should only be activated during a phase change.
        /// </summary>
        private bool outsideOfArena = false;

        /// <summary>
        /// Determines the phase that the boss is in, currently there are a maximum of 3 phases.
        /// </summary>
        [Range(1, 3), HideInInspector]
        public int phase = 1;

        /// <summary>
        /// A list of all of the elements that the werewolf can randomly enable during each phase.
        /// </summary>
        private List<Element> randomElementsList = new List<Element>();

        [Header("Healing Options")]
        public int healAmount = 5;
        public float healInterval = 1.5f;
        public float silverDebuffLength = 15f;
        private float silverDebuffElapsed = 0;
        private IEnumerator silverDebuffAction;
        /// <summary>
        /// Determines whether the Attack Coroutine can be started or not.
        /// </summary>
        private IEnumerator attackAction;

        /// <summary>
        /// Determines whether the Howl Coroutine can be started or not.
        /// </summary>
        private IEnumerator howlAction;

        /// <summary>
        /// Determines whether the Lunge Coroutine can be started or not.
        /// </summary>
        private IEnumerator lungeAction;

        /// <summary>
        /// Determines whether the LungeCooldown Coroutine can be started or not.
        /// </summary>
        private IEnumerator lungeCooldown;

        /// <summary>
        /// Determines whether the ArenaEnter or ArenaExit Coroutines can be started or not.
        /// </summary>
        private IEnumerator arenaExitAction;

        /// <summary>
        /// Used to reference the BossInputModule attached to the boss.
        /// </summary>
        private BossInputModule bossInputModuleInstance;
        #endregion

        #endregion

        #region Properties
        public override bool PerformingMajorAction
        {
            get
            {
                return arenaExitAction != null || howlAction != null || lungeAction != null || IsDying;
            }
        }

        public override bool PerformingMinorAction
        {
            get
            {
                return attackAction != null || IsDying;
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
                if (PerformingMajorAction) { return; }

                // This checks to see if the werewolf has entered phase 2 yet.
                else if ((TargetHealth / totalHealth < .66f) && phase < 2)
                {
                    phase = 2;
                    if (showDebugLogs) { Debug.Log("Entering phase " + phase); }

                    Howl();
                    InitiateArenaExitOrEnter();
                    if (showDebugLogs) { Debug.Log("Exiting the arena."); }
                }
                // This checks to see if the werewolf has entered phase 3 yet.
                else if ((TargetHealth / totalHealth < .33f) && phase < 3)
                {
                    phase = 3;
                    if (showDebugLogs) { Debug.Log("Entering phase " + phase); }

                    Howl();
                }
            }
        }

        public BossInputModule BossInputModuleInstance
        {
            get
            {
                if (bossInputModuleInstance == null) { bossInputModuleInstance = GetComponent<BossInputModule>(); }
                return bossInputModuleInstance;
            }
        }
        #endregion

        #region Unity Functions
        protected override void Start()
        {
            base.Start();
            agent.speed = speed;
            agent.updateRotation = false;

            rightClawWeapon.gameObject.SetActive(false);
            leftClawWeapon.gameObject.SetActive(false);
            doubleSwipeWeapon.gameObject.SetActive(false);

            randomElementsList.Add(new Element.Fire());
            randomElementsList.Add(new Element.Ice());
            randomElementsList.Add(new Element.Electric());

            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
            if (canChangeElements) { ChangeWeaponElements(); }
            bossInputModuleInstance = GetComponent<BossInputModule>();
            AddWerewolfHealing();
        }

        private void Update()
        {
            if (anim != null)
            {
                anim.SetFloat("dirX", agent.velocity.x / speed);
                anim.SetFloat("dirY", agent.velocity.z / speed);
                anim.SetBool("moving", Mathf.Abs(agent.velocity.magnitude) > 0.02f);
            }
            else
            {
                Debug.LogWarning("There is no animator controller; floats dirX and dirY, and bool moving are not being set.");
            }

            switch (phase)
            {
                case 2:
                    if (outsideOfArena)
                    {
                        for (var i = 0; i < phaseTwoMobs.Count; i++)
                        {
                            if (phaseTwoMobs[i] == null)
                            {
                                phaseTwoMobs.RemoveAt(i);
                            }

                            if (phaseTwoMobs.Count < 1)
                            {
                                InitiateArenaExitOrEnter();
                                if (showDebugLogs) { Debug.Log("Returning to the arena."); }
                            }
                        }
                    }
                    break;
                case 3:
                    if (phaseThreeMobs.Count > 0)
                    {
                        for (var i = 0; i < phaseThreeMobs.Count; i++)
                        {
                            if (phaseThreeMobs[i] == null)
                            {
                                phaseThreeMobs.RemoveAt(i);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Werewolf Movement
        public void Move(Transform target)
        {
            if (PerformingMajorAction) { return; }
            isAttacking = false;
            Move(target, speed);
        }

        public void Move(Transform target, float finalSpeed)
        {
            if (PerformingMajorAction) { return; }
            var finalTarget = new Vector3(target.position.x, target.position.y, target.position.z);

            if (target != null)
            {
                MoveToCalculations(turnSpeed, finalSpeed, finalTarget);
            }
            else
            {
                Debug.LogError("The target is null.");
            }
        }

        public void Turn(Transform target)
        {
            if (PerformingMajorAction) { return; }
            isAttacking = false;
            if (target != null)
            {
                RotateTowardsTarget(target.position, turnSpeed);
            }
        }
        #endregion

        #region Werewolf Combat

        private void AddWerewolfHealing()
        {
            // Only one heal at a time
            if (GetComponent<HealOverTime>() != null) { return; }

            gameObject.AddComponent<HealOverTime>().InitializeEffect(healAmount, healInterval, 0, null, this, null);
        }

        private void RemoveWerewolfHealing()
        {
            var hot = GetComponent<HealOverTime>();
            if (hot == null) { return; }
            Destroy(hot);
        }

        private IEnumerator SilverDebuff ()
        {
            RemoveWerewolfHealing();
            while(silverDebuffElapsed < silverDebuffLength)
            {
                silverDebuffElapsed += Time.deltaTime;
                yield return null;
            }
            AddWerewolfHealing();
            silverDebuffAction = null;
        }

        public override void Damage(int damage, bool isCritical, Element damageElement)
        {
            base.Damage(damage, isCritical, damageElement);
            if (damageElement is Element.Silver)
            {
                silverDebuffElapsed = 0;

                if (silverDebuffAction != null) { return; }
                silverDebuffAction = SilverDebuff();
                StartCoroutine(silverDebuffAction);
            }
        }
        /// <summary>
        /// Begins the attack coroutine.
        /// </summary>
        public void Attack()
        {
            if (attackAction != null) { return; }
            else if (IsDying) { return; }

            attackAction = AttackAnimation();
            StartCoroutine(attackAction);
        }

        /// <summary>
        /// Usually activated within the animation itself, this activates the hitbox of the current weapon.
        /// </summary>
        public void WeaponAnimationEvent()
        {
            CurrentWeapon?.StartAttackFromAnimationEvent();
        }

        /// <summary>
        /// The main logic for the bosses' chain attack.
        /// The reason that we are using WaitForSeconds instead of an animation event is because with the 
        /// animation blending that's occurring during the animations, sometimes the animation events are 
        /// getting cut off before they can be completed.
        /// </summary>
        public IEnumerator AttackAnimation()
        {
            yield return new WaitUntil(WerewolfHowlingCheck);
            yield return new WaitUntil(WerewolfLungingCheck);

            isAttacking = true;
            if (moveFasterWhileAttacking) { speed += speedBoost; }

            // First attack in the combo swing
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("firstAttack");

            yield return new WaitForSeconds(BossInputModuleInstance.firstAttackClip.length);
            anim.ResetTrigger("firstAttack");

            // Second attack in the combo swing
            if (leftClawWeapon != null) { EquipWeaponToCharacter(leftClawWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("secondAttack");

            yield return new WaitForSeconds(BossInputModuleInstance.secondAttackClip.length);
            anim.ResetTrigger("secondAttack");

            // Third attack in the combo swing
            if (doubleSwipeWeapon != null) { EquipWeaponToCharacter(doubleSwipeWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("thirdAttack");

            yield return new WaitForSeconds(BossInputModuleInstance.thirdAttackClip.length);
            anim.ResetTrigger("thirdAttack");

            // Wait for the last swing to finish, and then resetting everything
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);

            isAttacking = false;
            if (moveFasterWhileAttacking) { speed -= speedBoost; }
            attackAction = null;
        }

        /// <summary>
        /// Function used to kill the boss once it's health reaches 0.
        /// </summary>
        protected override IEnumerator KillCharacter()
        {
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
            rightClawWeapon.baseDamage = 65;
            rightClawWeapon.critPercent = 75;
            rightClawWeapon.hitBoxFrames = 10;
            anim.SetTrigger("death");

            yield return new WaitForSeconds(1.5f);

            GameManager.instance?.RemoveBossFromRadius(this);
            rightClawWeapon.gameObject.SetActive(false);
            BossInputModuleInstance.leftClawLightning.Stop();
            BossInputModuleInstance.rightClawLightning.Stop();
            BossInputModuleInstance.leftClawFire.Stop();
            BossInputModuleInstance.rightClawFire.Stop();
            BossInputModuleInstance.leftClawIce.Stop();
            BossInputModuleInstance.rightClawIce.Stop();

            BossInputModuleInstance.enabled = false;
            agent.enabled = false;
            characterController.enabled = false;
            yield return SpawnInteractableItems();
            endPortal?.gameObject.SetActive(true);
        }
        #endregion

        #region Werewolf Lunge
        /// <summary>
        /// Begins the lunge coroutine.
        /// </summary>
        public void Lunge()
        {
            if (lungeAction != null) { return; }
            else if (IsDying) { return; }

            lungeAction = LungeAnimation();
            StartCoroutine(lungeAction);
        }

        /// <summary>
        /// Usually activated within the animation itself, this continues the Lunge coroutine.
        /// </summary>
        public void LungeAnimationEvent()
        {
            if (lungeAction == null)
            {
                Debug.LogError("The Lunge Coroutine reference is null despite the animation event being called. This reference should have been set when you gave the lunge input.");
                return;
            }
            StartCoroutine(lungeAction);
        }

        /// <summary>
        /// The main logic for the bosses' lunge action. The coroutine should be paused part way through
        /// and resumed by an animation event.
        /// </summary>
        private IEnumerator LungeAnimation()
        {
            yield return new WaitUntil(WerewolfAttackingCheck);
            yield return new WaitUntil(WerewolfHowlingCheck);

            // No movement during the lunge
            isLunging = true;
            canlunge = false;
            invincible = true;

            var startPosition = EyeLineTransform.position;
            var bossForward = RotationTransform.forward;
            var lungeDirectionTarget = new Vector3();

            var hit = new RaycastHit();
            var ray = new Ray(startPosition, bossForward);

            // Raycast to determine target point for lunge destination on the X and Z axis
            if (Physics.Raycast(ray, out hit, lungeMaxDistance, lungeValidLayers))
            {
                lungeDirectionTarget = hit.point;
            }
            else
            {
                lungeDirectionTarget = ray.GetPoint(lungeMaxDistance);
            }
            Debug.DrawLine(startPosition, lungeDirectionTarget, Color.green, 5);

            // Raycast to determine target point for dodge destination on the Y axis.
            hit = new RaycastHit();
            ray = new Ray(lungeDirectionTarget, Vector3.down);

            // Setting this to the start position because if we RayCast down and dont get a hit, that means you casted off the map. If you do we cancel the dash.
            var floorPointFromLungeTarget = startPosition;
            if (Physics.Raycast(ray, out hit, lungeMaxDistance, lungeValidLayers))
            {
                floorPointFromLungeTarget = hit.point;
                Debug.DrawLine(lungeDirectionTarget, floorPointFromLungeTarget, Color.magenta, 5);
            }
            else
            {
                Debug.LogWarning("The boss tried to lunge into the void. Canceling the lunge.");

                isLunging = false;
                invincible = false;

                lungeCooldown = null;
                lungeCooldown = LungeCooldown();
                StartCoroutine(lungeCooldown);

                lungeAction = null;
                yield break;
            }

            var closestNavMeshPointToTarget = Utility.GetClosestPointOnNavMesh(floorPointFromLungeTarget, agent, transform);
            var lungeTarget = closestNavMeshPointToTarget;

            anim.SetTrigger("lungeAscend");
            Fabric.EventManager.Instance?.PostEvent("Werewolf Lunge Attack");

            yield return new WaitForSeconds(BossInputModuleInstance.lungeAscend.length);
            // PAUSE HERE FOR ANIMATION EVENT

            // COROUTINE RESUMES HERE
            var percentComplete = 0f;
            var tempPositon = transform.position;
            var elapsedTime = 0f;
            agent.enabled = false;

            // This is where the actual lerping happens
            var animationTriggerFlag = false;
            while (percentComplete < 1f)
            {
                // Yes this is broken but it's working fine so just leave it; unless you find a fix
                elapsedTime += Time.deltaTime * lungeSpeedCurve.Evaluate(lungeMaxSpeed);
                percentComplete = Mathf.Clamp01(elapsedTime / 1f);
                transform.position = Vector3.Lerp(tempPositon, lungeTarget, percentComplete);

                if (percentComplete > .9f && !animationTriggerFlag)
                {
                    anim.SetTrigger("lungeDescend");
                    animationTriggerFlag = true;
                }

                yield return null;
            }

            yield return new WaitForSeconds(BossInputModuleInstance.lungeDescend.length);

            agent.enabled = true;
            isLunging = false;
            invincible = false;

            yield return new WaitForSeconds(1f);

            lungeCooldown = null;
            lungeCooldown = LungeCooldown();
            StartCoroutine(lungeCooldown);

            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }

            lungeAction = null;
        }

        /// <summary>
        /// The Cooldown Coroutine associated with the Lunge attack.
        /// </summary>
        private IEnumerator LungeCooldown()
        {
            yield return new WaitForSeconds(lungeCooldownTime);
            canlunge = true;
            lungeCooldown = null;
        }
        #endregion

        #region Phase Related Functions
        /// <summary>
        /// Initiates the Howl Coroutine once the werewolf enters the second and third phases.
        /// </summary>
        public void Howl()
        {
            if (howlAction != null) { return; }
            if (IsDying) { return; }

            howlAction = HowlAnimation();
            StartCoroutine(howlAction);
        }

        /// <summary>
        /// Initiates the ArenaExitOrEnter Coroutine once the werewolf enters the second phase.
        /// </summary>
        public void InitiateArenaExitOrEnter()
        {
            if (arenaExitAction != null) { return; }

            arenaExitAction = ArenaExitOrEnter();
            StartCoroutine(arenaExitAction);
        }

        /// <summary>
        /// Usually activated within the animation itself, this continues the Lunge coroutine.
        /// </summary>
        public void ArenaExitOrEnterAnimationEvent()
        {
            if (arenaExitAction == null)
            {
                Debug.LogError("The ArenaChange Coroutine reference is null despite the animation event being called. This reference should have been set when you gave the lunge input.");
                return;
            }
            StartCoroutine(arenaExitAction);
        }

        /// <summary>
        /// The main logic for the bosses' howl.
        /// </summary>
        private IEnumerator HowlAnimation()
        {
            yield return new WaitUntil(WerewolfAttackingCheck);
            yield return new WaitUntil(WerewolfLungingCheck);

            isHowling = true;
            invincible = true;

            anim.SetTrigger("howl");

            yield return new WaitForSeconds(BossInputModuleInstance.howlClip.length);

            switch (phase)
            {
                case 2:
                    for (var i = 0; i < phaseTwoMobs.Count; i++)
                    {
                        if (phaseTwoMobs[i] != null) { phaseTwoMobs[i].SetActive(true); }
                    }
                    break;
                case 3:
                    for (var i = 0; i < phaseThreeMobs.Count; i++)
                    {
                        if (phaseThreeMobs[i] != null) { phaseThreeMobs[i].SetActive(true); }
                    }
                    break;
                default:
                    break;
            }

            if (canChangeElements) { ChangeWeaponElements(); }

            invincible = false;

            isHowling = false;
            howlAction = null;
        }

        /// <summary>
        /// The main logic for the bosses arena exit or entrance during a phase change.
        /// </summary>
        private IEnumerator ArenaExitOrEnter()
        {
            yield return new WaitUntil(WerewolfAttackingCheck);
            yield return new WaitUntil(WerewolfLungingCheck);
            yield return new WaitUntil(WerewolfHowlingCheck);

            BossInputModuleInstance.enabled = false;
            agent.enabled = false;
            invincible = true;


            if (!outsideOfArena)
            {
                if(silverDebuffAction != null)
                {
                    StopCoroutine(silverDebuffAction);
                    silverDebuffAction = null;
                }
                RemoveWerewolfHealing();

                anim.SetTrigger("arenaExit");

                // PAUSE HERE FOR ANIMATION EVENT
                StopCoroutine(arenaExitAction);
                yield return null;

                // COROUTINE RESUMES HERE
                var percentComplete = 0f;
                var tempPositon = transform.position;
                var elapsedTime = 0f;
                var enterLocation = new Vector3(transform.position.x, transform.position.y + 20, transform.position.z);

                // This is where the actual lerping happens
                while (percentComplete < 1f)
                {
                    elapsedTime += Time.deltaTime * lungeSpeedCurve.Evaluate(lungeMaxSpeed);
                    percentComplete = Mathf.Clamp01(elapsedTime / 1f);
                    transform.position = Vector3.Lerp(tempPositon, enterLocation, percentComplete);

                    yield return null;
                }
                RotationTransform.gameObject.SetActive(false);
                outsideOfArena = true;
            }
            else if (outsideOfArena)
            {
                RotationTransform.gameObject.SetActive(true);

                var percentComplete = 0f;
                var tempPositon = transform.position;
                var elapsedTime = 0f;
                var enterLocation = new Vector3(transform.position.x, transform.position.y - 20, transform.position.z);
                agent.enabled = false;

                // This is where the actual lerping happens
                var animationTriggerFlag = false;
                while (percentComplete < 1f)
                {
                    elapsedTime += Time.deltaTime * lungeSpeedCurve.Evaluate(lungeMaxSpeed);
                    percentComplete = Mathf.Clamp01(elapsedTime / 1f);
                    transform.position = Vector3.Lerp(tempPositon, enterLocation, percentComplete);

                    if (percentComplete > .9f && !animationTriggerFlag)
                    {
                        anim.SetTrigger("arenaEnter");
                        animationTriggerFlag = true;
                    }

                    yield return null;
                }

                // PAUSE HERE FOR ANIMATION EVENT
                StopCoroutine(arenaExitAction);
                yield return null;

                // COROUTINE RESUMES HERE
                BossInputModuleInstance.enabled = true;
                agent.enabled = true;
                invincible = false;
                outsideOfArena = false;
                AddWerewolfHealing();
            }

            arenaExitAction = null;
        }

        /// <summary>
        /// This function changes the current element of the werewolves' weapons to a new element depending on the phase.
        /// </summary>
        private void ChangeWeaponElements()
        {
            var randomElementIndex = Random.Range(0, randomElementsList.Count);
            var randomElement = randomElementsList[randomElementIndex];
            randomElementsList.RemoveAt(randomElementIndex);

            rightClawWeapon.WeaponElement = randomElement;
            leftClawWeapon.WeaponElement = randomElement;
            doubleSwipeWeapon.WeaponElement = randomElement;
            elementType = randomElement;

            var randomElementOption = Utility.ElementToElementOption(randomElement);

            #region Switch Particle Systems
            switch (randomElementOption)
            {
                case ElementOption.Fire:
                    BossInputModuleInstance.leftClawFire.gameObject.SetActive(true);
                    BossInputModuleInstance.rightClawFire.gameObject.SetActive(true);

                    BossInputModuleInstance.leftClawIce.gameObject.SetActive(false);
                    BossInputModuleInstance.rightClawIce.gameObject.SetActive(false);
                    BossInputModuleInstance.leftClawLightning.gameObject.SetActive(false);
                    BossInputModuleInstance.rightClawLightning.gameObject.SetActive(false);
                    break;
                case ElementOption.Ice:
                    BossInputModuleInstance.leftClawIce.gameObject.SetActive(true);
                    BossInputModuleInstance.rightClawIce.gameObject.SetActive(true);

                    BossInputModuleInstance.leftClawFire.gameObject.SetActive(false);
                    BossInputModuleInstance.rightClawFire.gameObject.SetActive(false);
                    BossInputModuleInstance.leftClawLightning.gameObject.SetActive(false);
                    BossInputModuleInstance.rightClawLightning.gameObject.SetActive(false);
                    break;
                case ElementOption.Electric:
                    BossInputModuleInstance.leftClawLightning.gameObject.SetActive(true);
                    BossInputModuleInstance.rightClawLightning.gameObject.SetActive(true);

                    BossInputModuleInstance.leftClawFire.gameObject.SetActive(false);
                    BossInputModuleInstance.rightClawFire.gameObject.SetActive(false);
                    BossInputModuleInstance.leftClawIce.gameObject.SetActive(false);
                    BossInputModuleInstance.rightClawIce.gameObject.SetActive(false);
                    break;
                default:
                    BossInputModuleInstance.leftClawLightning.gameObject.SetActive(false);
                    BossInputModuleInstance.rightClawLightning.gameObject.SetActive(false);
                    BossInputModuleInstance.leftClawFire.gameObject.SetActive(false);
                    BossInputModuleInstance.rightClawFire.gameObject.SetActive(false);
                    BossInputModuleInstance.leftClawIce.gameObject.SetActive(false);
                    BossInputModuleInstance.rightClawIce.gameObject.SetActive(false);
                    break;
            }
            #endregion
        }

        /// <summary>
        /// A boolean function to determine whether the werewolf is still attacking or not. 
        /// This exists so once another coroutines begin it will wait until the Attack coroutine is finished
        /// before continuing.
        /// </summary>
        private bool WerewolfAttackingCheck()
        {
            if (!isAttacking) { return true; }
            else { return false; }
        }

        /// <summary>
        /// A boolean function to determine whether the werewolf is still howling or not. 
        /// This exists so once another coroutines begin it will wait until the Howl coroutine is finished
        /// before continuing.
        /// </summary>
        private bool WerewolfHowlingCheck()
        {
            if (!isHowling) { return true; }
            else { return false; }
        }

        /// <summary>
        /// A boolean function to determine whether the werewolf is still lunging or not. 
        /// This exists so once another coroutines begin it will wait until the Lunge coroutine is finished
        /// before continuing.
        /// </summary>
        private bool WerewolfLungingCheck()
        {
            if (!isLunging) { return true; }
            else { return false; }
        }
        #endregion

        #region Unused Functions
        public void Move(Vector3 target, float finalSpeed)
        {
            // This feature will not be implemented.
        }

        public void Wander(Vector3 target)
        {
            // This feature will not be implemented.
        }

        public void Idle()
        {
            // This feature will not be implemented.
        }

        public void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            // This feature will not be implemented.
        }

        public void Dash()
        {
            // This feature will not be implemented.
        }

        public void Interact()
        {
            // This feature will not be implemented.
        }

        public void CycleWeapons(bool cycleUp)
        {
            // This feature will not be implemented.
        }

        public void CycleElements(bool cycleUp)
        {
            // This feature will not be implemented.
        }

        public void SwitchWeaponType(bool switchToMelee)
        {
            // This feature will not be implemented.
        }

        public void AttackAnimationEvent()
        {
            // This feature will not be implemented.
        }

        public void Move(Vector3 moveDirection, Vector3 lookDirection)
        {
            // This feature will not be implemented.
        }
        #endregion
    }
}
