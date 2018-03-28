using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Hunter.Interactable;
using Hunter.Character;

namespace Hunter.AI
{
    public class AIInputModule : MonoBehaviour
    {
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
                return enemyModel;
            }

            set
            {
                enemyModel = value;
            }
        }

        public NavMeshAgent Agent
        {
            get
            {
                return agent;
            }

            set
            {
                agent = value;
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

        #region Classes
        private Attack attack;
        private Idle idle;
        private Wander wander;
        private MoveTo moveTo;
        private Retreat retreat;
        private NavPosition navPosition;
        private Character.Character character;
        private AIDetection aiDetection;
        #endregion

        public UrgeWeights urgeWeights;


        private void Start()
        {
            #region Classes
            attack = new Attack(gameObject);
            idle = new Idle(gameObject);
            wander = new Wander(gameObject);
            moveTo = new MoveTo(gameObject);
            retreat = new Retreat(gameObject);

            urgeWeights = new UrgeWeights();
            navPosition = new NavPosition();
            character = GetComponent<Character.Character>();
            aiDetection = GetComponent<AIDetection>();
            #endregion

            EnemyModel = gameObject.transform.GetChild(0).gameObject;
            Agent = GetComponent<NavMeshAgent>();

            target = FindNearestTargetWithString("Player");
            pointTarget = FindPointOnNavmesh();
        }

        private void FixedUpdate()
        {
            var distanceToTarget = DistanceToTarget();
            var distanceToPoint = DistanceToPoint();

            if (aiDetection != null)
            {
                enemyInLOS = aiDetection.DetectPlayer();
                if (enemyInLOS)
                {
                    inCombat = true;
                }
                else if ((distanceToTarget > aiDetection.maxDetectionDistance) && (!enemyInLOS))
                {
                    inCombat = false;
                }
            }

            var currentState = FindNextState(distanceToTarget, distanceToPoint);

            Debug.Log("Current State: " + currentState);

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

        /// <summary>
        /// This function performs various operations to determine what action has the highest urge value and then returns it.
        /// </summary>
        /// <returns></returns>
        public UtilityBasedAI FindNextState(float distanceToTarget, float distanceToPoint)
        {
            var attackValue = attack.CalculateAttack(urgeWeights.attackRangeMin, distanceToTarget, inCombat);
            var idleValue = idle.CalculateIdle(distanceToPoint, urgeWeights.distanceToPointMax, inCombat);
            var wanderValue = wander.CalculateWander(distanceToPoint, urgeWeights.distanceToPointMax, inCombat);
            var moveToValue = moveTo.CalculateMoveTo(distanceToTarget, urgeWeights.distanceToTargetMin, urgeWeights.distanceToTargetMax, inCombat);
            var retreatValue = retreat.CalculateRetreat(character.CurrentHealth, inCombat);

            #region Debug Logs
            //Debug.Log("Attack Value: " + attackValue);
            //Debug.Log("Idle Value: " + idleValue);
            //Debug.Log("Wander Value: " + wanderValue);
            //Debug.Log("MoveTo Value: " + moveToValue);
            //Debug.Log("Retreat Value: " + retreatValue);
            #endregion

            var largestValue = new Dictionary<UtilityBasedAI, float>
        {
            { attack, attackValue },
            { idle, idleValue },
            { wander, wanderValue },
            { moveTo, moveToValue },
            { retreat, retreatValue }
        };
            var max = largestValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return max;
        }

        private float DistanceToTarget()
        {
            var distance = Vector3.Distance(target.position, gameObject.transform.position);

            return distance;
        }

        private float DistanceToPoint()
        {
            var distance = Vector3.Distance(pointTarget, gameObject.transform.position);

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

            if (navPosition.RandomPoint(transform.position, maxDistance, out targetPosition))
            {
                pointTarget = targetPosition;
            }

            return pointTarget;
        }
    }
}
