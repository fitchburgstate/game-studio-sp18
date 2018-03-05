using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Hunter;
using Hunter.Character;

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

    private Attack attack;
    private Idle idle;
    private Wander wander;
    private MoveTo moveTo;
    private Retreat retreat;

    private void Start()
    {
        EnemyModel = gameObject.transform.GetChild(0).gameObject;
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var moveCharacter = GetComponent<IMoveable>();
        var findTarget = FindNearestTargetWithString("Player");

        //moveCharacter.Move();
    }

    private void GetCurrentState()
    {

    }

    private void NextState()
    {

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
