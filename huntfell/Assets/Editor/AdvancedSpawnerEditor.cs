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
    //SerializedProperty m_MonsterWalkSpeed;
    SerializedProperty m_MonsterRunSpeed;
    //SerializedProperty m_MonsterDamage;
    SerializedObject so_Monster;

    private Character oldCharacter;

    // Setting up the serialized properties
    private void OnEnable()
    {
        m_Monster = serializedObject.FindProperty("characterToSpawn");
        m_MonsterHealth = serializedObject.FindProperty("monsterHealth");
        m_MonsterName = serializedObject.FindProperty("monsterName");
        //m_MonsterWalkSpeed = serializedObject.FindProperty("monsterWalkSpeed");
        m_MonsterRunSpeed = serializedObject.FindProperty("monsterRunSpeed");
        //m_MonsterDamage = serializedObject.FindProperty("monsterDamage");
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

        #region Hard-Coded Wolf
        //if (characterToSpawn is Wolf)
        //{
        //    EditorGUILayout.LabelField(m_MonsterName.stringValue + " Specific Variables", EditorStyles.boldLabel);
        //    EditorGUILayout.BeginVertical("Box");
        //    EditorGUILayout.PropertyField(m_MonsterWalkSpeed);
        //    EditorGUILayout.PropertyField(m_MonsterRunSpeed);
        //    EditorGUILayout.PropertyField(m_MonsterDamage);
        //    EditorGUILayout.EndVertical();
        //}
        #endregion

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
            if (characterToSpawn is Wolf)
            {
                //m_MonsterWalkSpeed.floatValue = characterToSpawn.GetComponent<Wolf>().walkSpeed;
                m_MonsterRunSpeed.floatValue = characterToSpawn.GetComponent<Wolf>().runSpeed;
            }
        }
    }
}
