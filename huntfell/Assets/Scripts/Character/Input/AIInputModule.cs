using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hunter.Characters.AI
{
    public class AIInputModule : MonoBehaviour
    {
        #region Variables
        [Header("Possible Actions")]
        public bool attack = true;
        public bool turn = true;
        public bool moveTo = true;
        public bool retreat = true;
        public bool idle = true;
        public bool wander = true;

        [Header("Other Variables")]
        /// <summary>
        /// The max distance that the character will move to during a Wander action.
        /// </summary>
        [Range(0f, 25f), Tooltip("The max distance that the character will move to during a Wander action.")]
        public float maxDistance = 10f;

        /// <summary>
        /// The Scriptable Object which contains all of the values for the urge weights.
        /// </summary>
        public UrgeWeights urgeWeights;

        /// <summary>
        /// The target that the AI has acquired.
        /// </summary>
        [SerializeField]
        protected Transform target;

        /// <summary>
        /// The target point that the AI has acquired.
        /// </summary>
        [SerializeField]
        protected Vector3 randomPointTarget;

        public Vector3 spawnPosition;

        protected float tempHealth = 0f;

        #region AI Actions
        protected AIDetection aiDetection;
        protected Attack attackAction;
        protected Turn turnAction;
        protected MoveTo moveToAction;
        protected Idle idleAction;
        protected Wander wanderAction;
        protected Retreat retreatAction;
        protected Enemy enemy;
        #endregion

        public bool inCombat = false;
        protected bool enemyInLOS = false;
        protected bool enemyInVisionCone = false;
        #endregion

        #region Properties
        public Transform Target
        {
            get
            {
                if (target == null) { target = FindNearestTargetWithString("Player"); }
                return target;
            }
        }

        public Vector3 RandomPointTarget
        {
            get
            {
                if (randomPointTarget == Vector3.zero) { randomPointTarget = FindPointOnNavmesh(); }
                return randomPointTarget;
            }

            set
            {
                randomPointTarget = value;
            }
        }

        public AIDetection AiDetection
        {
            get
            {
                if (aiDetection == null)
                {
                    aiDetection = GetComponent<AIDetection>();
                }
                return aiDetection;
            }
        }
        #endregion

        #region Unity Functions
        protected virtual void Start()
        {
            spawnPosition = transform.position;
            urgeWeights = ScriptableObject.CreateInstance<UrgeWeights>();
            enemy = GetComponent<Enemy>();

            #region Classes
            attackAction = new Attack(gameObject);
            idleAction = new Idle(gameObject);
            wanderAction = new Wander(gameObject);
            moveToAction = new MoveTo(gameObject);
            retreatAction = new Retreat(gameObject);
            turnAction = new Turn(gameObject);
            #endregion
        }

        protected virtual void FixedUpdate()
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

                    var wolfComponent = GetComponent<Wolf>();

                    if (wolfComponent != null)
                    {
                        wolfComponent.justFound = false;
                    }
                    else { return; }
                }
            }

            var currentState = FindNextState(distanceToTarget, distanceToPoint, distanceToSpawn);

            #region Debug Logs
#if UNITY_EDITOR
            if (Selection.Contains(this.gameObject))
            {
                Debug.Log(currentState);
                //Debug.Log("enemyInVisionCone: " + enemyInVisionCone);
            }
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
            else if (currentState is Retreat)
            {
                inCombat = false;
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
        /// This function performs various operations to determine what action has the highest urge value and then returns it.
        /// </summary>
        /// <returns></returns>
        public virtual UtilityBasedAI FindNextState(float distanceToTarget, float distanceToPoint, float distanceToSpawn)
        {
            var attackValue = 0f;
            var idleValue = 0f;
            var wanderValue = 0f;
            var turnValue = 0f;
            var moveToValue = 0f;
            var retreatValue = 0f;

            if (attack) { attackValue = attackAction.CalculateAttack(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
            if (idle) { idleValue = idleAction.CalculateIdle(distanceToPoint, distanceToSpawn, urgeWeights.distanceToPointMax, inCombat); }
            if (wander) { wanderValue = wanderAction.CalculateWander(distanceToPoint, urgeWeights.distanceToPointMax, inCombat); }
            if (turn) { turnValue = turnAction.CalculateTurn(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
            if (moveTo) { moveToValue = moveToAction.CalculateMoveTo(distanceToTarget, urgeWeights.distanceToTargetMin, urgeWeights.distanceToTargetMax, inCombat); }
            if (retreat) { retreatValue = retreatAction.CalculateRetreat(distanceToSpawn, urgeWeights.distanceToTargetMin, urgeWeights.distanceToTargetMax, inCombat); }

            #region Debug Logs
#if UNITY_EDITOR
            if (Selection.Contains(this.gameObject))
            {
                //Debug.Log("Attack Value: " + attackValue);
                Debug.Log("Idle Value: " + idleValue);
                //Debug.Log("Wander Value: " + wanderValue);
                //Debug.Log("Turn Value: " + turnValue);
                //Debug.Log("MoveTo Value: " + moveToValue);
                Debug.Log("Retreat Value: " + retreatValue);
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
                { retreatAction, retreatValue }
            };
            var max = largestValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return max;
        }
        #endregion

        #region Helper Functions
        protected float DistanceToTarget(Vector3 targetPosition)
        {
            var distance = Vector3.Distance(targetPosition, gameObject.transform.position);

            return distance;
        }

        /// <summary>
        /// This function returns the nearest transform with the correct tag.
        /// </summary>
        /// <param name="targetString">The name of the tag that is being searched for.</param>
        /// <returns></returns>
        public Transform FindNearestTargetWithString(string targetString)
        {
            var targets = GameObject.FindGameObjectsWithTag(targetString);
            Transform bestTarget = null;
            var closestDistanceSqr = Mathf.Infinity;
            var currentPosition = transform.position;
            foreach (var potentialTarget in targets)
            {
                var directionToTarget = potentialTarget.transform.position - currentPosition;
                var dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }
            return bestTarget;
        }

        public Vector3 FindPointOnNavmesh()
        {
            var targetPosition = new Vector3();

            if (Utility.RandomNavMeshPoint(transform.position, maxDistance, out targetPosition))
            {
                randomPointTarget = targetPosition;
            }

            return randomPointTarget;
        }

        public Vector3 FindNewTargetPoint()
        {
            RandomPointTarget = FindPointOnNavmesh();
            return RandomPointTarget;
        }
        #endregion
    }
}
