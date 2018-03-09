using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Hunter;
using Hunter.Character;
using System.Linq;

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

    private bool hasJustAttacked = false;
    private bool enemyInLOS = false;
    private bool hasJustIdled = false;
    private bool inCombat = false;
    private bool hasJustWandered = false;
    private bool canMoveAwayFromTarget = false;

    private Character character;
    private AIDetection aiDetection;
    private Attack attack;
    private Idle idle;
    private Wander wander;
    private MoveTo moveTo;
    private Retreat retreat;
    private UrgeWeights urgeWeights;

    private void Start()
    {
        attack = new Attack(gameObject);
        idle = new Idle(gameObject);
        wander = new Wander(gameObject);
        moveTo = new MoveTo(gameObject);
        retreat = new Retreat(gameObject);

        urgeWeights = new UrgeWeights();
        character = GetComponent<Character>();
        aiDetection = GetComponent<AIDetection>();

        EnemyModel = gameObject.transform.GetChild(0).gameObject;
        Agent = GetComponent<NavMeshAgent>();

        target = FindNearestTargetWithString("Player");
    }

    private void Update()
    {
        enemyInLOS = aiDetection.DetectPlayer();
        var distanceToTarget = DistanceToTarget();

        if (enemyInLOS)
        {
            inCombat = true;
        }
        else if ((distanceToTarget > aiDetection.maxDetectionDistance) && (!enemyInLOS))
        {
            inCombat = false;
        } 

        var currentState = FindNextState(distanceToTarget);

        Debug.Log("Current State: " + currentState);
        Debug.Log("inCombat: " + inCombat);
        Debug.Log("enemyInLOS: " + enemyInLOS);

        currentState.Act();

        #region State Switchers
        if (currentState is Attack)
        {
            hasJustAttacked = true;
            hasJustIdled = false;
            hasJustWandered = false;
            inCombat = true;
        }
        else if (currentState is Idle)
        {
            hasJustAttacked = false;
            hasJustIdled = true;
            hasJustWandered = false;
            inCombat = false;
        }
        else if (currentState is MoveTo)
        {
            hasJustAttacked = false;
            hasJustIdled = false;
            hasJustWandered = false;
            inCombat = true;
        }
        else if (currentState is Retreat)
        {
            inCombat = true;
            hasJustAttacked = false;
            hasJustIdled = false;
            hasJustWandered = false;
        }
        else if (currentState is Wander)
        {
            inCombat = false;
            hasJustAttacked = false;
            hasJustIdled = false;
            hasJustWandered = true;
        }
        #endregion
    }

    /// <summary>
    /// This function performs various operations to determine what action has the highest urge value and then returns it.
    /// </summary>
    /// <returns></returns>
    public UtilityBasedAI FindNextState(float distanceToTarget)
    {
        var attackValue = attack.CalculateAttack(distanceToTarget, character.health, inCombat);
        var idleValue = idle.CalculateIdle(enemyInLOS, urgeWeights.targetInLOSUrgeValue, hasJustIdled, urgeWeights.hasJustIdledUrgeValue, inCombat);
        var wanderValue = wander.CalculateWander(enemyInLOS, urgeWeights.targetInLOSUrgeValue, hasJustWandered, urgeWeights.hasJustWanderedUrgeValue, inCombat);
        var moveToValue = moveTo.CalculateMoveTo(distanceToTarget, character.health, inCombat);
        var retreatValue = retreat.CalculateRetreat(canMoveAwayFromTarget, urgeWeights.canMoveAwayFromTargetValue, character.health, inCombat);

        #region Debug Logs
        //Debug.Log("Attack Value: " + attackValue);
        ////Debug.Log("Idle Value: " + idleValue);
        ////Debug.Log("Wander Value: " + wanderValue);
        //Debug.Log("MoveTo Value: " + moveToValue);
        ////Debug.Log("Retreat Value: " + retreatValue);
        #endregion

        var largestValue = new Dictionary<UtilityBasedAI, float>
        {
            { attack, attackValue },
            { idle, idleValue },
            { wander, wanderValue },
            { moveTo, moveToValue },
            { retreat, retreatValue }
        };

        //var sortedDict = from entry in largestValue orderby entry.Value ascending select entry;
        var max = largestValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

        return max;
    }

    private float DistanceToTarget()
    {
        var distance = Vector3.Distance(target.position, gameObject.transform.position);

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
}
