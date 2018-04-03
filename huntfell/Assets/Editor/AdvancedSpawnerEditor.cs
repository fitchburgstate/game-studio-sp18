using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Hunter.Character;

[CustomEditor(typeof(CharacterSpawner))]
[CanEditMultipleObjects]
public class AdvancedSpawnerEditor : Editor
{
    SerializedProperty m_Monster;
    SerializedProperty m_MonsterHealth;
    SerializedProperty m_MonsterName;
    SerializedObject so_Monster;

    private Character oldCharacter;

    // Setting up the serialized properties
    private void OnEnable()
    {
        m_Monster = serializedObject.FindProperty("characterToSpawn");
        m_MonsterHealth = serializedObject.FindProperty("monsterHealth");
        m_MonsterName = serializedObject.FindProperty("monsterName");
        so_Monster = new SerializedObject(m_Monster.objectReferenceValue);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var characterToSpawn = m_Monster.objectReferenceValue as Character;

        if (oldCharacter != characterToSpawn)
        {
            UpdateInspectorInformation(characterToSpawn);
        }

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.PropertyField(m_Monster, new GUIContent("Character to Spawn"));
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Generic Variables", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.PropertyField(m_MonsterName);
        EditorGUILayout.PropertyField(m_MonsterHealth);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        var newCTS = so_Monster.GetIterator();
        EditorGUILayout.BeginVertical();
        while (newCTS.NextVisible(true))
        {
            EditorGUILayout.PropertyField(newCTS);
        }
        EditorGUILayout.EndVertical();

        oldCharacter = characterToSpawn;

        EditorGUILayout.Space();

        if (GUILayout.Button("Create"))
        {
            InstantiateCharacter(characterToSpawn);
        }

        if (GUILayout.Button("Reset Information"))
        {
            UpdateInspectorInformation(characterToSpawn);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void InstantiateCharacter(Character characterToSpawn)
    {
        if (characterToSpawn != null)
        {
            Instantiate(characterToSpawn.gameObject);
        }
    }

    private void UpdateInspectorInformation(Character characterToSpawn)
    {
        if (characterToSpawn != null)
        {
            m_MonsterHealth.floatValue = characterToSpawn.CurrentHealth;
            m_MonsterName.stringValue = characterToSpawn.name;
        }
    }
}
