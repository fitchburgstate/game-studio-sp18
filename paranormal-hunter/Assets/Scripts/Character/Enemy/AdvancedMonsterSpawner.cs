using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

public class AdvancedMonsterSpawner : MonoBehaviour
{
    public GameObject monster;

    public float monsterHealth;

    public string monsterName;

    public float monsterWalkSpeed;

    public float monsterRunSpeed;

    public float monsterDamage;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
