using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Hunter;

public class AIInputModule : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;

    private GameObject enemyRoot;
    private NavMeshAgent agent;

    private void Start()
    {
        enemyRoot = gameObject.transform.GetChild(0).gameObject;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var moveEnemy = GetComponent<IMoveable>();
        var characterController = GetComponent<CharacterController>();
        var finalDirection = Vector3.zero;
    }
}
