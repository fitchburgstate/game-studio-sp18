using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEditor;

namespace Hunter.Characters.AI
{
    public class AIInputModule : MonoBehaviour
    {
        #region Variables
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
        private Transform target;

        /// <summary>
        /// The target point that the AI has acquired.
        /// </summary>
        private Vector3 pointTarget;

        /// <summary>
        /// The max distance that the wolf will move to during a Wander action.
        /// </summary>
        [Tooltip("The max distance that the wolf will move to during a Wander action.")]
        [Range(0f, 25f)]
        public float maxDistance = 5f;

        private bool enemyInLOS = false;
        private bool inCombat = false;
        private bool enemyInVisionCone = false;

        private AIDetection aiDetection;

        private Attack attack;
        private Idle idle;
        private Wander wander;
        private MoveTo moveTo;
        private Retreat retreat;
        private Character character;
        private Turn turn;

        public UrgeWeights urgeWeights;
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
                return target;
            }

            set
            {
                target = value;
            }
        }

        public Vector3 PointTarget
        {
            get
            {
                return pointTarget;
            }

            set
            {
                pointTarget = value;
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
            target = FindNearestTargetWithString("Player");
            urgeWeights = ScriptableObject.CreateInstance<UrgeWeights>();
            character = GetComponent<Character>();

            #region Classes
            attack = new Attack(gameObject);
            idle = new Idle(gameObject);
            wander = new Wander(gameObject);
            moveTo = new MoveTo(gameObject);
            retreat = new Retreat(gameObject);
            turn = new Turn(gameObject, target);
            #endregion

            pointTarget = FindPointOnNavmesh();
        }

        private void FixedUpdate()
        {
            var distanceToTarget = DistanceToTarget();
            var distanceToPoint = DistanceToPoint();

            if (AiDetection != null)
            {
                enemyInLOS = AiDetection.DetectPlayer();
                enemyInVisionCone = AiDetection.InVisionCone;

                if (enemyInLOS)
                {
                    inCombat = true;
                }
                else if ((distanceToTarget > AiDetection.maxDetectionDistance) && (!enemyInLOS))
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
            //Debug.Log(currentState);
            //Debug.Log("enemyInVisionCone: " + enemyInVisionCone);
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
        }

        #region FindNextState Function
        /// <summary>
        /// This function performs various operations to determine what action has the highest urge value and then returns it.
        /// </summary>
        /// <returns></returns>
        public UtilityBasedAI FindNextState(float distanceToTarget, float distanceToPoint)
        {
            var attackValue = attack.CalculateAttack(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat);
            var idleValue = idle.CalculateIdle(distanceToPoint, urgeWeights.distanceToPointMax, inCombat);
            var wanderValue = wander.CalculateWander(distanceToPoint, urgeWeights.distanceToPointMax, inCombat);
            var turnValue = turn.CalculateTurn(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat);
            var moveToValue = moveTo.CalculateMoveTo(distanceToTarget, urgeWeights.distanceToTargetMin, urgeWeights.distanceToTargetMax, inCombat);
            var retreatValue = retreat.CalculateRetreat(character.CurrentHealth, inCombat);

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
                { attack, attackValue },
                { idle, idleValue },
                { wander, wanderValue },
                { turn, turnValue },
                { moveTo, moveToValue },
                { retreat, retreatValue }
            };
            var max = largestValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return max;
        }
        #endregion

        #region DistanceToTarget Function
        private float DistanceToTarget()
        {
            var distance = Vector3.Distance(target.position, gameObject.transform.position);

            return distance;
        }
        #endregion

        #region DistanceToPoint Function
        private float DistanceToPoint()
        {
            var distance = Vector3.Distance(pointTarget, gameObject.transform.position);

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
                pointTarget = targetPosition;
            }

            return pointTarget;
        }
        #endregion
    }
}
