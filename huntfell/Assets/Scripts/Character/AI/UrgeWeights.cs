﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hunter.Character.AI
{
    public class UrgeWeights : ScriptableObject
    {
        public float distanceToTarget = 0f;

        public float currentHealth = 0f;

        public float distanceToTargetMin = 0f;

        public float distanceToTargetMax = 100f;

        public float distanceToPointMax = 2f;

        public float attackRangeMin = 2f;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Urge Weights")]
        public static void CreateUrgeWeights()
        {
            var asset = CreateInstance<UrgeWeights>();

            AssetDatabase.CreateAsset(asset, "Assets/UrgeWeights.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
#endif
    }
}
