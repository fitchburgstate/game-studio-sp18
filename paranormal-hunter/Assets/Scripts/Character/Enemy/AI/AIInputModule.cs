using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Hunter;

public class AIInputModule : MonoBehaviour
{
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


    private void Start()
    {
        enemyModel = gameObject.transform.GetChild(0).gameObject;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var moveEnemy = GetComponent<IMoveable>();
        var characterController = GetComponent<CharacterController>();
        var finalDirection = Vector3.zero;
    }
}
