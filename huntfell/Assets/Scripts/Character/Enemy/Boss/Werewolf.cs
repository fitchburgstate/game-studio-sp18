using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters.AI;

namespace Hunter.Characters
{
    public class Werewolf : Boss, IMoveable, IAttack, IUtilityBasedAI
    {
        #region Variables
        /// <summary>
        /// Determines whether debug logs relating to the boss will appear in the console window.
        /// </summary>
        [Header("Debug"), Tooltip("Determines whether debug logs relating to the boss will appear in the console window.")]
        public bool showDebugLogs = false;

        /// <summary>
        /// Determines the movement speed of the boss.
        /// </summary>
        [Header("Movement Options")]
        [Range(0, 20), Tooltip("The running speed of the boss when it is in combat.")]
        public float speed = 5f;

        /// <summary>
        /// Determines the speed at which the boss turns.
        /// </summary>
        [Range(1, 250), Tooltip("The speed at which the boss will turn.")]
        public float turnSpeed = 200f;

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
        /// The mobs that will spawn in phase 4. This should be set in the inspector.
        /// </summary>
        [Tooltip("The mobs that will spawn when the boss enters phase 4.")]
        public List<GameObject> phaseFourMobs = new List<GameObject>();

        /// <summary>
        /// A boolean to determine whether the boss can apply elements to himself during every phase change.
        /// </summary>
        [Tooltip("Determines whether the wereweolf can change elements throughout the fight.")]
        public bool canChangeElements = true;

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
        public float lungeCoolDown = 5f;

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
        /// Determines the phase that the boss is in, currently there are a maximum of 3 phases.
        /// </summary>
        [Range(1, 3), HideInInspector]
        public int phase = 1;

        private List<Element> randomElementsList = new List<Element>();

        /// <summary>
        /// Determines whether the Attack Coroutine can be played or not.
        /// </summary>
        private IEnumerator attackCR;

        /// <summary>
        /// Determines whether the Howl Coroutine can be played or not.
        /// </summary>
        private IEnumerator howlCR;

        /// <summary>
        /// Determines whether the Lunge Coroutine can be played or not.
        /// </summary>
        private IEnumerator lungeCR;

        /// <summary>
        /// Used to reference the BossInputModule attached to the boss.
        /// </summary>
        private BossInputModule bossInputModuleInstance;

        /// <summary>
        /// Used to reference the Player script attached to the player in the scene.
        /// </summary>
        private Player playerInstance;
        #endregion

        #endregion

        #region Properties
        public override float CurrentHealth
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                if (health <= 0 && !isDying)
                {
                    StartCoroutine(KillWerewolf());
                    isDying = true;
                }

                // This checks to see if the werewolf has entered phase 2 yet.
                else if ((health / totalHealth < .66f) && phase < 2)
                {
                    phase = 2;
                    invincible = true;

                    InitiateHowl();
                }
                // This checks to see if the werewolf has entered phase 3 yet.
                else if ((health / totalHealth < .33f) && phase < 3)
                {
                    phase = 3;
                    invincible = true;

                    InitiateHowl();
                }
            }
        }

        public BossInputModule BossInputModuleInstance
        {
            get
            {
                if (bossInputModuleInstance == null) { GetComponent<BossInputModule>(); }
                return bossInputModuleInstance;
            }
        }

        public Player PlayerInstance
        {
            get
            {
                if (playerInstance == null) { playerInstance = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); }
                return playerInstance;
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

            randomElementsList.Add(Utility.ElementOptionToElement(ElementOption.Fire));
            randomElementsList.Add(Utility.ElementOptionToElement(ElementOption.Ice));
            randomElementsList.Add(Utility.ElementOptionToElement(ElementOption.Lightning));

            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
            if (canChangeElements) { ChangeWeaponElements(phase); }
            bossInputModuleInstance = GetComponent<BossInputModule>();
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

            #region Debug Logs
            if (showDebugLogs)
            {
                if (attackCR != null)
                {
                    Debug.Log("attackCR is not null, performing attack combo.");
                }
                else if (howlCR != null)
                {
                    Debug.Log("howlCR is not null, performing howl.");
                }
                else if (lungeCR != null)
                {
                    Debug.Log("lungeCR is not null, performing lunge.");
                }
            }
            #endregion
        }
        #endregion

        #region Werewolf Movement
        public void Move(Transform target)
        {
            if (isDying) { return; }
            isAttacking = false;
            Move(target, speed);
        }

        public void Move(Transform target, float finalSpeed)
        {
            if (isDying) { return; }
            var finalTarget = new Vector3(target.position.x, RotationTransform.localPosition.y, target.position.z);

            if (target != null)
            {
                MoveToCalculations(turnSpeed, finalSpeed, finalTarget);
            }
            else
            {
                Debug.LogError("The target is null.");
            }
        }

        public void Move(Vector3 target, float finalSpeed)
        {
            if (isDying) { return; }
        }

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }
        }

        public void Turn(Transform target)
        {
            if (isDying) { return; }
            isAttacking = false;
            if (target != null)
            {
                RotateTowardsTarget(target.position, turnSpeed);
            }
        }
        #endregion

        #region Werewolf Combat
        /// <summary>
        /// Begins the attack coroutine.
        /// </summary>
        public void Attack()
        {
            if (attackCR != null) { return; }
            else if (isDying) { return; }

            attackCR = AttackAnimation();
            StartCoroutine(attackCR);
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
            yield return new WaitUntil(WerewolfAttackingCheck);
            yield return new WaitUntil(WerewolfLungingCheck);

            isAttacking = true;

            // First attack in the combo swing
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("firstAttack");

            yield return new WaitForSeconds(BossInputModuleInstance.firstAttackClip.length);

            // Second attack in the combo swing
            if (leftClawWeapon != null) { EquipWeaponToCharacter(leftClawWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("secondAttack");

            yield return new WaitForSeconds(BossInputModuleInstance.secondAttackClip.length);

            // Third attack in the combo swing
            if (doubleSwipeWeapon != null) { EquipWeaponToCharacter(doubleSwipeWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            anim.SetTrigger("thirdAttack");

            yield return new WaitForSeconds(BossInputModuleInstance.thirdAttackClip.length);

            // Wait for the last swing to finish, and then resetting everything
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }

            anim.SetFloat("attackSpeed", CurrentWeapon.attackSpeed);
            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);

            isAttacking = false;
            attackCR = null;
        }

        /// <summary>
        /// Function used to kill the boss once it's health reaches 0.
        /// </summary>
        private IEnumerator KillWerewolf()
        {
            agent.speed = 0;
            agent.destination = transform.position;
            agent.enabled = false;
            characterController.enabled = false;

            if (attackCR != null)
            {
                StopCoroutine(attackCR);
                attackCR = null;
            }

            #region Not Important
            if (rightClawWeapon != null) { EquipWeaponToCharacter(rightClawWeapon); }
            rightClawWeapon.baseDamage = 65;
            rightClawWeapon.critPercent = 75;
            rightClawWeapon.hitBoxFrames = 80;
            anim.SetTrigger("death");
            #endregion

            yield return null;
        }
        #endregion

        #region Werewolf lunge
        /// <summary>
        /// Begins the lunge coroutine.
        /// </summary>
        public void Lunge()
        {
            if (lungeCR != null) { return; }
            else if (isDying) { return; }

            lungeCR = LungeAnimation();
            StartCoroutine(lungeCR);
        }

        /// <summary>
        /// Usually activated within the animation itself, this continues the Lunge coroutine.
        /// </summary>
        public void LungeAnimationEvent()
        {
            if (lungeCR == null)
            {
                Debug.LogError("The Lunge Coroutine reference is null despite the animation event being called. This reference should have been set when you gave the lunge input.");
                return;
            }
            StartCoroutine(lungeCR);
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
            isDying = true;
            canlunge = false;
            invincible = true;

            var startPosition = eyeLine.position;
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
                Debug.LogWarning("You tried to lunge into the void. Canceling the lunge.");
                isLunging = false;
                isDying = false;
                lungeCR = null;
                invincible = false;
                yield break;
            }

            var closestNavMeshPointToTarget = Utility.GetClosestPointOnNavMesh(floorPointFromLungeTarget, agent, transform);
            var lungeTarget = closestNavMeshPointToTarget;

            anim.SetTrigger("lungeAscend");

            // PAUSE HERE FOR ANIMATION EVENT
            StopCoroutine(lungeCR);
            yield return null;

            // COROUTINE RESUMES HERE
            var lungeDistanceCheckMargin = 0.09f;
            //var lungeTime = 0f;
            var startTime = Time.time;
            var percentComplete = 0f;
            var lungeAmount = 0f;
            agent.enabled = false;

            //This is where the actual movement happens
            while (Vector3.Distance(transform.position, lungeTarget) > lungeDistanceCheckMargin)
            {
                var elapsedTime = Time.time - startTime;
                percentComplete = Mathf.Clamp01(elapsedTime / lungeCoolDown);
                lungeAmount = lungeSpeedCurve.Evaluate(percentComplete);
                transform.position = Vector3.Lerp(transform.position, lungeTarget, lungeAmount);
                yield return null;
            }

            anim.SetTrigger("lungeDescend");
            yield return new WaitForSeconds(BossInputModuleInstance.lungeDescend.length);

            Debug.Log("Lunge Amount: " + lungeAmount);
            Debug.Log("Percent Complete: " + percentComplete);

            //while (Vector3.Distance(transform.position, lungeTarget) > lungeDistanceCheckMargin)
            //{
            //    lungeTime += lungeMaxSpeed * Time.deltaTime;
            //    var lungeAmount = lungeSpeedCurve.Evaluate(lungeTime);
            //    transform.position = Vector3.Lerp(transform.position, lungeTarget, lungeAmount);
            //    yield return null;
            //}

            agent.enabled = true;
            yield return null;

            isLunging = false;
            isDying = false;
            invincible = false;

            yield return new WaitForSeconds(lungeCoolDown);
            lungeCR = null;
        }
        #endregion

        #region Phase Related Functions
        /// <summary>
        /// Initiates the Howl coroutine once the werewolf enters the second and third phases.
        /// </summary>
        public void InitiateHowl()
        {
            if (howlCR != null) { return; }
            if (isDying) { return; }

            howlCR = HowlAnimation();
            StartCoroutine(howlCR);
        }

        /// <summary>
        /// The main logic for the bosses' howl.
        /// </summary>
        private IEnumerator HowlAnimation()
        {
            yield return new WaitUntil(WerewolfAttackingCheck);
            yield return new WaitUntil(WerewolfLungingCheck);

            isHowling = true;

            isDying = true;
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
                case 4:
                    for (var i = 0; i < phaseFourMobs.Count; i++)
                    {
                        if (phaseFourMobs[i] != null) { phaseFourMobs[i].SetActive(true); }
                    }
                    break;
                default:
                    break;
            }

            if (canChangeElements) { ChangeWeaponElements(phase); }

            invincible = false;
            isDying = false;

            isHowling = false;
            howlCR = null;
        }

        /// <summary>
        /// This function changes the current element of the werewolves' weapons to a new element depending on the phase.
        /// </summary>
        private void ChangeWeaponElements(int phase)
        {
            var randomElementIndex = Random.Range(0, randomElementsList.Count - 1);
            var randomElement = randomElementsList[randomElementIndex];
            randomElementsList.RemoveAt(randomElementIndex);

            switch (phase)
            {
                case 1:
                    rightClawWeapon.weaponElement = randomElement;
                    leftClawWeapon.weaponElement = randomElement;
                    doubleSwipeWeapon.weaponElement = randomElement;
                    break;
                case 2:
                    rightClawWeapon.weaponElement = randomElement;
                    leftClawWeapon.weaponElement = randomElement;
                    doubleSwipeWeapon.weaponElement = randomElement;
                    break;
                case 3:
                    rightClawWeapon.weaponElement = randomElement;
                    leftClawWeapon.weaponElement = randomElement;
                    doubleSwipeWeapon.weaponElement = randomElement;
                    break;
                default:
                    break;
            }
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
        public void Idle()
        {
            if (isDying) { return; }
            // This feature has not yet been implemented.
        }

        public void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void Dash()
        {
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void Interact()
        {
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void CycleWeapons(bool cycleUp)
        {
            if (isDying) { return; }
        }

        public void CycleElements(bool cycleUp)
        {
            if (isDying) { return; }
        }

        public void SwitchWeaponType(bool switchToMelee)
        {
            if (isDying) { return; }
        }
        #endregion
    }
}
