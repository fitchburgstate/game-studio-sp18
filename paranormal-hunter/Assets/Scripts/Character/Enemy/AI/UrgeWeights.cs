using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hunter.Character;

[CreateAssetMenu(fileName = "Urge Weights", menuName = "Tools", order = 1)]
public class UrgeWeights : ScriptableObject
{
    public float canMoveAwayFromTargetValue = 100f;

    public float hasJustWanderedUrgeValue = 75f;

    public float hasJustIdledUrgeValue = 75f;

    public float noNewPositionUrgeValue = 100f;

    public float distanceToTarget = 0f;

    public float currentHealth = 0f;

    public float distanceToTargetMin = 0f;

    public float distanceToTargetMax = 100f;

    public float currentHealthMin = 0f;

    public float currentHealthMax = 100f;

    public float attackRangeMin = 2f;

    public float attackRangeMax = 100f;

    [MenuItem("Assets/Create/Urge Weights")]
    public static void CreateUrgeWeights()
    {
        var asset = CreateInstance<UrgeWeights>();

        AssetDatabase.CreateAsset(asset, "Assets/UrgeWeights.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
