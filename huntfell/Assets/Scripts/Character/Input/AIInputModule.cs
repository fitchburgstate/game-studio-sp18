using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
        public bool idle = true;
        public bool wander = true;
        public bool retreat = true;

        [Header("Other Variables")]
        /// <summary>
        /// The max distance that the character will move to during a Wander action.
        /// </summary>
        [Range(0f, 25f), Tooltip("The max distance that the character will move to during a Wander action.")]
        public float maxDistance = 10f;

        public UrgeWeights urgeWeights;

        /// <summary>
        /// Represents which direction the character should move in.
        /// </summary>
        private Vector3 moveDirection = Vector3.zero;

        /// <summary>
        /// Represents which direction the character should look in.
        /// </summary>
        private Vector3 lookDirection = Vector3.zero;

        /// <summary>
        /// The model's gameobject. This exists so the model can be turned independently of the parent.
        /// </summary>
        private GameObject enemyModel;

        /// <summary>
        /// This is the navmesh agent attached to the parent. The navmesh is used to find walkable area.
        /// </summary>
        private NavMeshAgent agent;

        /// <summary>
        /// The character controller that controls the character's movement.
        /// </summary>
        private CharacterController controller;

        /// <summary>
        /// The final direction that the character will face that's calculated.
        /// </summary>
        private Vector3 finalDirection;

        /// <summary>
        /// The target that the AI has acquired.
        /// </summary>
        [SerializeField]
        private Transform target;

        /// <summary>
        /// The target point that the AI has acquired.
        /// </summary>
        [SerializeField]
        private Vector3 randomPointTarget;

        private float tempHealth = 0f;

        #region AI Actions
        private AIDetection aiDetection;
        private Attack attackAction;
        private Turn turnAction;
        private MoveTo moveToAction;
        private Idle idleAction;
        private Wander wanderAction;
        private Retreat retreatAction;
        private Enemy enemy;
        #endregion

        private bool enemyInLOS = false;
        private bool inCombat = false;
        private bool enemyInVisionCone = false;
        #endregion

        #region Properties
        public Vector3 MoveDirection
        {
            get
            {
                return moveDirection;
            }

            set
            {
                moveDirection = value;
            }
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

        public GameObject EnemyModel
        {
            get
            {
                if (enemyModel == null)
                {
                    enemyModel = gameObject.transform.GetChild(0).gameObject;
                }
                return enemyModel;
            }
        }

        public NavMeshAgent Agent
        {
            get
            {
                if (agent == null)
                {
                    agent = GetComponent<NavMeshAgent>();
                }
                return agent;
            }
        }

        public CharacterController Controller
        {
            get
            {
                return controller;
            }

            set
            {
                controller = value;
            }
        }

        public Vector3 FinalDirection
        {
            get
            {
                return finalDirection;
            }

            set
            {
                finalDirection = value;
            }
        }

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

        private void Start()
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
            #endregion
        }

        private void FixedUpdate()
        {
            if (!inCombat)
            {
                if (enemy.CurrentHealth < tempHealth)
                {
                    inCombat = true;
                }
            }

            var distanceToPoint = DistanceToTarget(RandomPointTarget);
            var distanceToTarget = DistanceToTarget(Target.position);

            if (AiDetection != null)
            {
                enemyInLOS = AiDetection.DetectPlayer();
                enemyInVisionCone = AiDetection.InVisionCone;

                if (enemyInLOS)
                {
                    inCombat = true;
                }
                else if ((distanceToTarget > (AiDetection.maxDetectionDistance*2)) && (!enemyInLOS))
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

            var currentState = FindNextState(distanceToTarget, distanceToPoint);

#if UNITY_EDITOR
            //Debug.Log(currentState);
            //Debug.Log("enemyInVisionCone: " + enemyInVisionCone);
#endif

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

        #region FindNextState Function
        /// <summary>
        /// This function performs various operations to determine what action has the highest urge value and then returns it.
        /// </summary>
        /// <returns></returns>
        public UtilityBasedAI FindNextState(float distanceToTarget, float distanceToPoint)
        {
            var attackValue = 0f;
            var idleValue = 0f;
            var wanderValue = 0f;
            var turnValue = 0f;
            var moveToValue = 0f;
            var retreatValue = 0f;

            if (attack) { attackValue = attackAction.CalculateAttack(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
            if (idle) { idleValue = idleAction.CalculateIdle(distanceToPoint, urgeWeights.distanceToPointMax, inCombat); }
            if (wander) { wanderValue = wanderAction.CalculateWander(distanceToPoint, urgeWeights.distanceToPointMax, inCombat); }
            if (turn) { turnValue = turnAction.CalculateTurn(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
            if (moveTo) { moveToValue = moveToAction.CalculateMoveTo(distanceToTarget, urgeWeights.distanceToTargetMin, urgeWeights.distanceToTargetMax, inCombat); }
            if (retreat) { retreatValue = retreatAction.CalculateRetreat(enemy.CurrentHealth, inCombat); }

            #region Debug Logs
#if UNITY_EDITOR
            if (Selection.Contains(gameObject))
            {
                //Debug.Log("Attack Value: " + attackValue);
                // Debug.Log("Idle Value: " + idleValue);
                // Debug.Log("Wander Value: " + wanderValue);
                //Debug.Log("Turn Value: " + turnValue);
                // Debug.Log("MoveTo Value: " + moveToValue);
                // Debug.Log("Retreat Value: " + retreatValue);
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

        #region DistanceToTarget Function
        private float DistanceToTarget(Vector3 targetPosition)
        {
            var distance = Vector3.Distance(targetPosition, gameObject.transform.position);

            return distance;
        }
        #endregion

        #region FindNearestTargetWithString Function
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
        #endregion

        #region FindPointOnNavmesh Function
        public Vector3 FindPointOnNavmesh()
        {
            var targetPosition = new Vector3();

            if (Utility.RandomNavMeshPoint(transform.position, maxDistance, out targetPosition))
            {
                randomPointTarget = targetPosition;
            }

            return randomPointTarget;
        }
        #endregion
    }
}
