using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hunter.Character;

public class UrgeWeights : EditorWindow
{
    // These values are used to determine the next action the AI will perform.
    public float targetInLOSUrgeValue = 75f;
    public float canMoveAwayFromTargetValue = 100f;
    public float hasJustWanderedUrgeValue = 25f;
    public float hasJustIdledUrgeValue = 25;
    public float noNewPositionUrgeValue = 100f;
    public float hasAttackedUrgeValue = 25f;
    public float inCombatValue = 100f;
    public float distanceToTarget = 0f;
    public float currentHealth = 0f;

    [Tooltip("Instead of showing raw sliders with a range, show the raw values instead.")]
    bool showRawValues = false;

    [MenuItem("Tools/Urge Weights")]
    public static void ShowWindow() // Opens the window when clicked
    {
        EditorWindow.GetWindow(typeof(UrgeWeights), true, "Urge Weights");
    }

    private void OnGUI()
    {
        showRawValues = EditorGUILayout.Toggle("Show Raw values", showRawValues);

        if (!showRawValues)
        {
            targetInLOSUrgeValue = EditorGUILayout.Slider("Target in LOS", targetInLOSUrgeValue, 0, 100);

            canMoveAwayFromTargetValue = EditorGUILayout.Slider("Can move away", canMoveAwayFromTargetValue, 0, 100);

            hasJustWanderedUrgeValue = EditorGUILayout.Slider("Has just wandered", hasJustWanderedUrgeValue, 0, 100);

            hasJustIdledUrgeValue = EditorGUILayout.Slider("Has just idled", hasJustIdledUrgeValue, 0, 100);

            noNewPositionUrgeValue = EditorGUILayout.Slider("No new position", noNewPositionUrgeValue, 0, 100);

            hasAttackedUrgeValue = EditorGUILayout.Slider("Has attacked", hasAttackedUrgeValue, 0, 100);

            inCombatValue = EditorGUILayout.Slider("In combat", inCombatValue, 0, 100);
        }
        else
        {
            targetInLOSUrgeValue = EditorGUILayout.FloatField("Target in LOS", targetInLOSUrgeValue);

            canMoveAwayFromTargetValue = EditorGUILayout.FloatField("Can move away", canMoveAwayFromTargetValue);

            hasJustWanderedUrgeValue = EditorGUILayout.FloatField("Has just wandered", hasJustWanderedUrgeValue);

            hasJustIdledUrgeValue = EditorGUILayout.FloatField("Has just idled", hasJustIdledUrgeValue);

            noNewPositionUrgeValue = EditorGUILayout.FloatField("No new position", noNewPositionUrgeValue);

            hasAttackedUrgeValue = EditorGUILayout.FloatField("Has attacked", hasAttackedUrgeValue);

            inCombatValue = EditorGUILayout.FloatField("In combat", inCombatValue);
        }

        EditorGUILayout.HelpBox("Do not edit these values unless you know what you are doing!", MessageType.Warning);
        EditorGUILayout.HelpBox("This window is used to set the weights of the urges for the AI system. Refer to the spreadsheet in Google Docs relating to this system for more information or clarity.", MessageType.Info);
    }
}
