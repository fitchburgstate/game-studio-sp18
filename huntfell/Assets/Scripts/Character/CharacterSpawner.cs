using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hunter.Characters
{
    [Serializable]
    public class CharacterSpawner : MonoBehaviour
    {
        public Character characterToSpawn;

        public string monsterName;

        [Range(1, 200)]
        public float monsterHealth;

        [Range(1, 20)]
        public float monsterWalkSpeed;

        [Range(1, 20)]
        public float monsterRunSpeed;

        [Range(1, 200)]
        public float monsterDamage;

        public void OnDrawGizmos ()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }
}
