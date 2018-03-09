using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hunter.Character;

[System.Serializable]
public class UrgeWeights : EditorWindow
{
    // These values are used to determine the next action the AI will perform.
    public float canMoveAwayFromTargetValue = 100f;
    public float hasJustWanderedUrgeValue = 25f;
    public float hasJustIdledUrgeValue = 25;
    public float noNewPositionUrgeValue = 100f;

    public float distanceToTarget = 0f;
    public float currentHealth = 0f;

    [SerializeField]
    private bool distanceToTargetFoldout = false;
    public float distanceToTargetMin = 0f;
    public float distanceToTargetMax = 100f;

    [SerializeField]
    private bool currentHealthFoldout = false;
    public float currentHealthMin = 0f;
    public float currentHealthMax = 100f;

    [SerializeField]
    private bool attackRangeFoldout = false;
    public float attackRangeMin = 0f;
    public float attackRangeMax = 10f;

    [SerializeField]
    private bool showRawValues = false;

    [MenuItem("Tools/Urge Weights")]
    [SerializeField]
    public static void ShowWindow()
    {
        GetWindow(typeof(UrgeWeights), true, "Urge Weights");
    }

    [SerializeField]
    private void OnGUI()
    {
        EditorGUILayout.HelpBox("This window is used to set the weights of the urges for the AI system. Refer to the spreadsheet in Google Docs relating to this system for more information or clarity.", MessageType.Info);

        showRawValues = EditorGUILayout.Toggle("Show Raw values", showRawValues);

        EditorGUIUtility.labelWidth = 300;
        EditorGUILayout.Space();

        if (!showRawValues)
        {
            #region Value Slider
            EditorGUILayout.BeginVertical("Box");
            canMoveAwayFromTargetValue = EditorGUILayout.Slider("Can Move Away", canMoveAwayFromTargetValue, 0f, 100f);
            hasJustWanderedUrgeValue = EditorGUILayout.Slider("Has Just Wandered", hasJustWanderedUrgeValue, 0f, 100f);
            hasJustIdledUrgeValue = EditorGUILayout.Slider("Has Just Idled", hasJustIdledUrgeValue, 0f, 100f);
            noNewPositionUrgeValue = EditorGUILayout.Slider("No New Position", noNewPositionUrgeValue, 0f, 100f);
            EditorGUILayout.EndVertical();
            #endregion
            EditorGUILayout.Space();

            #region Foldout Slider
            EditorGUILayout.BeginVertical("Box");

            distanceToTargetFoldout = EditorGUILayout.Foldout(distanceToTargetFoldout, "Distance To Target Slider");
            if (distanceToTargetFoldout)
            {
                distanceToTargetMin = EditorGUILayout.FloatField("Distance To Target Minimum", distanceToTargetMin);
                distanceToTargetMax = EditorGUILayout.FloatField("Distance To Target Maximum", distanceToTargetMax);
                EditorGUILayout.MinMaxSlider(ref distanceToTargetMin, ref distanceToTargetMax, 0f, 100f);
            }
            EditorGUILayout.Space();
            currentHealthFoldout = EditorGUILayout.Foldout(currentHealthFoldout, "Health Range Slider");
            if (currentHealthFoldout)
            {
                currentHealthMin = EditorGUILayout.FloatField("Current Health Minimum", currentHealthMin);
                currentHealthMax = EditorGUILayout.FloatField("Current Health Maximum", currentHealthMax);
                EditorGUILayout.MinMaxSlider(ref currentHealthMin, ref currentHealthMax, 0f, 100f);
            }
            EditorGUILayout.Space();
            attackRangeFoldout = EditorGUILayout.Foldout(attackRangeFoldout, "Attack Range Slider");
            if (attackRangeFoldout)
            {
                attackRangeMin = EditorGUILayout.FloatField("Attack Range Minimum", attackRangeMin);
                attackRangeMax = EditorGUILayout.FloatField("Attack Range Maximum", attackRangeMax);
                EditorGUILayout.MinMaxSlider(ref attackRangeMin, ref attackRangeMax, 0f, 10f);
            }
            EditorGUILayout.EndVertical();
            #endregion
        }
        else
        {
            #region Value Raw
            EditorGUILayout.BeginVertical("Box");
            canMoveAwayFromTargetValue = EditorGUILayout.FloatField("Can Move Away", canMoveAwayFromTargetValue);
            hasJustWanderedUrgeValue = EditorGUILayout.FloatField("Has Just Wandered", hasJustWanderedUrgeValue);
            hasJustIdledUrgeValue = EditorGUILayout.FloatField("Has Just Idled", hasJustIdledUrgeValue);
            noNewPositionUrgeValue = EditorGUILayout.FloatField("No New Position", noNewPositionUrgeValue);
            EditorGUILayout.EndVertical();

            #endregion
            EditorGUILayout.Space();
            #region Foldout Raw
            EditorGUILayout.BeginVertical("Box");
            currentHealthMin = EditorGUILayout.FloatField("Current Health Minimum", currentHealthMin);
            currentHealthMax = EditorGUILayout.FloatField("Current Health Maximum", currentHealthMax);

            distanceToTargetMin = EditorGUILayout.FloatField("Distance To Target Minimum", distanceToTargetMin);
            distanceToTargetMax = EditorGUILayout.FloatField("Distance To Target Maximum", distanceToTargetMax);

            attackRangeMin = EditorGUILayout.FloatField("Attack Range Minimum", attackRangeMin);
            attackRangeMax = EditorGUILayout.FloatField("Attack Range Maximum", attackRangeMax);
            EditorGUILayout.EndVertical();
            #endregion
        }
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Do not edit these values unless you know what you are doing!", MessageType.Warning);


    }
}
