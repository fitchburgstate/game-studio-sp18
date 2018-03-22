using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Hunter.Character;

[CustomEditor(typeof(AdvancedMonsterSpawner))]
[CanEditMultipleObjects]
public class AdvancedSpawnerEditor : Editor
{
    SerializedProperty m_Monster;
    SerializedProperty m_MonsterHealth;
    SerializedProperty m_MonsterName;
    SerializedProperty m_MonsterWalkSpeed;
    SerializedProperty m_MonsterRunSpeed;
    SerializedProperty m_MonsterDamage;
    SerializedObject m_Monster_SO;

    Character oldCharacter;

    // Setting up the serialized properties
    private void OnEnable()
    {
        m_Monster = serializedObject.FindProperty("characterToSpawn");
        m_MonsterHealth = serializedObject.FindProperty("monsterHealth");
        m_MonsterName = serializedObject.FindProperty("monsterName");
        m_MonsterWalkSpeed = serializedObject.FindProperty("monsterWalkSpeed");
        m_MonsterRunSpeed = serializedObject.FindProperty("monsterRunSpeed");
        m_MonsterDamage = serializedObject.FindProperty("monsterDamage");
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

        if (characterToSpawn is Wolf)
        {
            EditorGUILayout.LabelField(m_MonsterName.stringValue + " Specific Variables", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(m_MonsterWalkSpeed);
            EditorGUILayout.PropertyField(m_MonsterRunSpeed);
            EditorGUILayout.PropertyField(m_MonsterDamage);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Create"))
        {
            InstantiateCharacter();
        }

        if (GUILayout.Button("Reset Information"))
        {
            UpdateInspectorInformation(characterToSpawn);
        }

        serializedObject.ApplyModifiedProperties();

        oldCharacter = characterToSpawn;
    }

    private void InstantiateCharacter()
    {

    }

    private void UpdateInspectorInformation(Character characterToSpawn)
    {
        if (characterToSpawn != null)
        {
            m_MonsterHealth.floatValue = characterToSpawn.health;
            m_MonsterName.stringValue = characterToSpawn.name;
            if (characterToSpawn is Wolf)
            {
                m_MonsterWalkSpeed.floatValue = characterToSpawn.GetComponent<Enemy>().walkSpeed;
                m_MonsterRunSpeed.floatValue = characterToSpawn.GetComponent<Enemy>().runSpeed;
            }
        }
    }
}


//            //Property Drawer here
//            foreach (var name in characterToSpawn.SerialzedPropertyNamesList)
//            {
//                var relProp = m_Monster.FindPropertyRelative(name);
//                EditorGUILayout.PropertyField(relProp);
//            }
