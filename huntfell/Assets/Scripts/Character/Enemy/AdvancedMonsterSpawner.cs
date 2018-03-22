using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;
using System;

[Serializable]
public class AdvancedMonsterSpawner : MonoBehaviour
{
    public Character characterToSpawn;

    public GameObject monster;
    
    public string monsterName;

    [Range(1, 200)]
    public float monsterHealth;
    
    [Range(1, 20)]
    public float monsterWalkSpeed;
    
    [Range(1, 20)]
    public float monsterRunSpeed;
    
    [Range(1, 200)]
    public float monsterDamage;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
