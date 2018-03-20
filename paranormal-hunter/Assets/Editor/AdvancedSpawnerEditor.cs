using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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

    // Setting up the serialized properties
    private void OnEnable()
    {
        m_Monster = serializedObject.FindProperty("monster");
        m_MonsterHealth = serializedObject.FindProperty("monsterHealth");
        m_MonsterName = serializedObject.FindProperty("monsterName");
        m_MonsterWalkSpeed = serializedObject.FindProperty("monsterWalkSpeed");
        m_MonsterRunSpeed = serializedObject.FindProperty("monsterRunSpeed");
        m_MonsterDamage = serializedObject.FindProperty("monsterDamage");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_Monster, new GUIContent("Monster to Spawn"));

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(m_MonsterName);
        EditorGUILayout.PropertyField(m_MonsterHealth);
        EditorGUILayout.PropertyField(m_MonsterWalkSpeed);
        EditorGUILayout.PropertyField(m_MonsterRunSpeed);
        EditorGUILayout.PropertyField(m_MonsterDamage);

        EditorGUILayout.Space();

        if (GUILayout.Button("Update Information"))
        {
            var monsterGO = m_Monster.objectReferenceValue as Enemy;
            m_MonsterName.stringValue = m_Monster.objectReferenceValue.name;
            m_MonsterHealth.floatValue = monsterGO.health;
            m_MonsterWalkSpeed.floatValue = monsterGO.walkSpeed;
            m_MonsterRunSpeed.floatValue = monsterGO.runSpeed;
        }

        if (GUILayout.Button("Create"))
        {
            InstantiateCharacter();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void InstantiateCharacter()
    {

    }
}
