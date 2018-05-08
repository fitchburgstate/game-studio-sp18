using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Hunter.Characters.AI
{
    public class BossInputModule : AIInputModule
    {
        #region Variables
        /// <summary>
        /// A possible action for the boss.
        /// </summary>
        [Space]
        public bool lunge = false;

        protected Lunge lungeAction;

        [Header("Animation Clips")]
        /// <summary>
        /// The animation clip for the first attack.
        /// </summary>
        public AnimationClip firstAttackClip;

        /// <summary>
        /// The animation clip for the second attack.
        /// </summary>
        public AnimationClip secondAttackClip;

        /// <summary>
        /// The animation clip for the third attack.
        /// </summary>
        public AnimationClip thirdAttackClip;

        [Space]
        /// <summary>
        /// The animation clip for the howl.
        /// </summary>
        public AnimationClip howlClip;

        /// <summary>
        /// The animation clip for the lunge ascend.
        /// </summary>
        public AnimationClip lungeAscend;

        /// <summary>
        /// The animation clip for the lunge descend.
        /// </summary>
        public AnimationClip lungeDescend;

        [Header("Particle Systems")]
        /// <summary>
        /// The particle system for the left claw's fire.
        /// </summary>
        public ParticleSystem leftClawFire;

        /// <summary>
        /// The particle system for the right claw's fire.
        /// </summary>
        public ParticleSystem rightClawFire;

        [Space]
        /// <summary>
        /// The particle system for the left claw's lightning.
        /// </summary>
        public ParticleSystem leftClawLightning;

        /// <summary>
        /// The particle system for the right claw's lightning.
        /// </summary>
        public ParticleSystem rightClawLightning;

        [Space]
        /// <summary>
        /// The particle system for the left claw's ice.
        /// </summary>
        public ParticleSystem leftClawIce;

        /// <summary>
        /// The particle system for the right claw's ice.
        /// </summary>
        public ParticleSystem rightClawIce;

        /// <summary>
        /// The instance of the werewolf script attached to the gameobject that this script is also attached to.
        /// </summary>
        private Werewolf werewolfInstance;
        #endregion

        #region Properties
        public Werewolf WerewolfInstance
        {
            get
            {
                if (werewolfInstance == null) { werewolfInstance = GetComponent<Werewolf>(); }
                return werewolfInstance;
            }
        }
        #endregion

        #region Unity Functions
        protected override void Start()
        {
            urgeWeights = ScriptableObject.CreateInstance<UrgeWeights>();
            enemy = GetComponent<Enemy>();

            #region Classes
            attackAction = new Attack(gameObject);
            idleAction = new Idle(gameObject);
            wanderAction = new Wander(gameObject);
            moveToAction = new MoveTo(gameObject);
            retreatAction = new Retreat(gameObject);
            turnAction = new Turn(gameObject);
            lungeAction = new Lunge(gameObject);
            #endregion
        }

        protected override void FixedUpdate()
        {
            if (!inCombat)
            {
                if (enemy.CurrentHealth < tempHealth)
                {
                    inCombat = true;
                }
            }

            var distanceToPoint = DistanceToTarget(RandomPointTarget);
            var distanceToSpawn = DistanceToTarget(spawnPosition);
            var distanceToTarget = DistanceToTarget(Target.position);

            if (AiDetection != null)
            {
                enemyInLOS = AiDetection.DetectPlayer();
                enemyInVisionCone = AiDetection.InVisionCone;

                if (enemyInLOS)
                {
                    inCombat = true;
                }
                else if ((distanceToTarget > (AiDetection.maxDetectionDistance * 2)) && (!enemyInLOS))
                {
                    inCombat = false;
                }
            }

            var currentState = FindNextState(distanceToTarget, distanceToPoint, distanceToSpawn);

            #region Debug Logs
#if UNITY_EDITOR
            //Debug.Log(currentState);
            //Debug.Log("enemyInVisionCone: " + enemyInVisionCone);
#endif
            #endregion

            currentState.Act();

            #region State Switchers
            if (currentState is Attack)
            {
                inCombat = true;
            }
            else if (currentState is MoveTo)
            {
                inCombat = true;
            }
            else if (currentState is Turn)
            {
                inCombat = true;
            }
            else if (currentState is Lunge)
            {
                inCombat = true;
            }
            else if (currentState is Retreat)
            {
                inCombat = true;
            }
            else if (currentState is Idle)
            {
                inCombat = false;
            }
            else if (currentState is Wander)
            {
                inCombat = false;
            }
            #endregion

            tempHealth = enemy.CurrentHealth;
        }
        #endregion

        #region FindNextState Function
        /// <summary>
        /// Finds the next most desired state by determining multiple factors present within the environment.
        /// </summary>
        public override UtilityBasedAI FindNextState(float distanceToTarget, float distanceToPoint, float distanceToSpawn)
        {
            var attackValue = 0f;
            var idleValue = 0f;
            var wanderValue = 0f;
            var turnValue = 0f;
            var moveToValue = 0f;
            var lungeValue = 0f;
            var retreatValue = 0f;

            if (idle) { idleValue = idleAction.CalculateIdle(distanceToPoint, distanceToSpawn, urgeWeights.distanceToPointMax, inCombat); }
            if (attack) { attackValue = attackAction.CalculateAttack(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
            if (wander) { wanderValue = wanderAction.CalculateWander(distanceToPoint, urgeWeights.distanceToPointMax, inCombat); }
            if (turn) { turnValue = turnAction.CalculateTurn(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
            if (moveTo) { moveToValue = moveToAction.CalculateMoveTo(distanceToTarget, urgeWeights.distanceToTargetMin, urgeWeights.distanceToTargetMax, inCombat); }
            if (retreat) { retreatValue = retreatAction.CalculateRetreat(distanceToSpawn, urgeWeights.distanceToTargetMin, urgeWeights.distanceToTargetMax, inCombat); }
            if (lunge) { lungeValue = lungeAction.CalculateLunge(WerewolfInstance.lungeMaxDistance, distanceToTarget, WerewolfInstance.canlunge, WerewolfInstance.phase, inCombat); }

            #region Debug Logs
#if UNITY_EDITOR
            if (Selection.Contains(gameObject))
            {
                //Debug.Log("Attack Value: " + attackValue);
                //Debug.Log("Idle Value: " + idleValue);
                //Debug.Log("Wander Value: " + wanderValue);
                //Debug.Log("Turn Value: " + turnValue);
                //Debug.Log("MoveTo Value: " + moveToValue);
                //Debug.Log("Retreat Value: " + retreatValue);
                //Debug.Log("Lunge Value: " + lungeValue);
            }
#endif
            #endregion

            var largestValue = new Dictionary<UtilityBasedAI, float>
            {
                { attackAction, attackValue },
                { idleAction, idleValue },
                { wanderAction, wanderValue },
                { turnAction, turnValue },
                { moveToAction, moveToValue },
                { lungeAction, lungeValue },
                { retreatAction, retreatValue }
            };
            var max = largestValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return max;
        }
        #endregion
    }
}
