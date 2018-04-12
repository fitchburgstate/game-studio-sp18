using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif

namespace Hunter.Characters.AI
{
    [CreateAssetMenu(fileName = "NewUrgeWeights", menuName = "Urge Weights", order = -1)]
    public class UrgeWeights : ScriptableObject
    {
        public float distanceToTarget = 0f;

        public float currentHealth = 0f;

        public float distanceToTargetMin = 0f;

        public float distanceToTargetMax = 100f;

        public float distanceToPointMax = 2f;

        public float attackRangeMin = 2.5f;
    }
}
