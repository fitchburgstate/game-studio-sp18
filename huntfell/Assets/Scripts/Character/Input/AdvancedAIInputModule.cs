using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hunter;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AdvancedAIInputModule : MonoBehaviour
{
    #region Variables and Enums
    public enum AIActions
    {
        None,
        Attack,
        Turn,
        MoveTo,
        Retreat,
        Idle,
        Wander,
        Special1,
        Special2,
        Special3
    }

    // Public Variables
    public bool attack;
    public bool turn;
    public bool moveTo;
    public bool retreat;
    public bool idle = true;
    public bool wander;
    public bool special1;
    public bool special2;
    public bool special3;

    // Private Variables
    private Transform target;
    private Transform randomPointTarget;
    private Vector3 spawnPosition;

    protected float tempHealth = 0f;
    private AIActions currentAction;

    private bool inCombat = false;
    private bool enemyInLOS = false;
    #endregion

    #region Unity Functions
    private void Start()
    {
        currentAction = AIActions.Idle;
    }

    private void Update()
    {
        currentAction = DetermineNextAction();
        PerformAction(currentAction);
    }
    #endregion

    #region Decisions
    public void PerformAction(AIActions action)
    {
        switch (action)
        {
            case AIActions.None:
                NoneAction();
                break;
            case AIActions.Attack:
                AttackAction();
                break;
            case AIActions.Idle:
                IdleAction();
                break;
            case AIActions.MoveTo:
                MoveToAction();
                break;
            case AIActions.Retreat:
                RetreatAction();
                break;
            case AIActions.Turn:
                TurnAction();
                break;
            case AIActions.Wander:
                WanderAction();
                break;
            case AIActions.Special1:
                Special1Action();
                break;
            case AIActions.Special2:
                Special2Action();
                break;
            case AIActions.Special3:
                Special3Action();
                break;
            default:
                break;
        }
    }

    public AIActions DetermineNextAction()
    {
        var nextActionToPerform = new AIActions();
        float idleValue, attackValue, moveToValue, retreatValue, turnValue, wanderValue, special1Value, special2Value, special3Value;

        if (inCombat)
        {
            if (attack) { }
            if (moveTo) { }
            if (retreat) { }
            if (special1) { }
            if (special2) { }
            if (special3) { }
        }
        if (idle) { }
        if (turn) { }
        if (wander) { }

        return nextActionToPerform;
    }
    #endregion

    #region Actions
    public void NoneAction()
    {
        return;
    }

    public void AttackAction()
    {
    }

    public void IdleAction()
    {
    }

    public void MoveToAction()
    {
    }

    public void RetreatAction()
    {
    }

    public void TurnAction()
    {
    }

    public void WanderAction()
    {
    }

    public void Special1Action()
    {
    }

    public void Special2Action()
    {
    }

    public void Special3Action()
    {
    }
    #endregion

    #region Helper Functions
    protected float DistanceToTarget(Vector3 targetPosition)
    {
        var distance = Vector3.Distance(targetPosition, gameObject.transform.position);

        return distance;
    }

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

    //public Vector3 FindPointOnNavmesh()
    //{
    //    var targetPosition = new Vector3();

    //    if (Utility.RandomNavMeshPoint(transform.position, maxDistance, out targetPosition))
    //    {
    //        randomPointTarget = targetPosition;
    //    }

    //    return randomPointTarget;
    //}

    //public Vector3 FindNewTargetPoint()
    //{
    //    RandomPointTarget = FindPointOnNavmesh();
    //    return RandomPointTarget;
    //}
    #endregion
}
