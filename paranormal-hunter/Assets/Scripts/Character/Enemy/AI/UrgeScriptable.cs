using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

[CreateAssetMenu(fileName = "UrgeValues", menuName = "Utility Based AI/Urges", order = 0)]
public class UrgeScriptable : ScriptableObject
{
    /// <summary>
    /// This value is used to determine the next action that the AI will perform.
    /// </summary>
    [Range(0, 100)]
    public float enemyInLOSUrgeValue = 75f;

    /// <summary>
    /// This value is used to determine the next action that the AI will perform.
    /// </summary>
    [Range(0, 100)]
    public float hasJustWanderedUrgeValue = 25f;

    /// <summary>
    /// This value is used to determine the next action that the AI will perform.
    /// </summary>
    [Range(0, 100)]
    public float hasJustIdledUrgeValue = 25;

    /// <summary>
    /// This value is used to determine the next action that the AI will perform.
    /// </summary>
    [Range(0, 100)]
    public float noNewPositionUrgeValue = 100f;

    /// <summary>
    /// This value is used to determine the next action that the AI will perform.
    /// </summary>
    [Range(0, 100)]
    public float hasAttackedUrgeValue = 25f;

    /// <summary>
    /// This value is used to determine the next action that the AI will perform.
    /// </summary>
    public float distanceToTarget = 0f;

    /// <summary>
    /// This value is used to determine the next action that the AI will perform.
    /// </summary>
    public float currentHealth = 0f;

    /// <summary>
    /// This value is used to determine the next action that the AI will perform.
    /// </summary>
    [Range(0, 100)]
    public float inCombatValue = 100f;
}
