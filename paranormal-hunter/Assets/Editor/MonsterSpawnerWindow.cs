using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class MonsterSpawnerWindow : EditorWindow
{
    public MonsterSpawner spawner;
    public List<MonsterSpawner> monsterSpawners;
    static Vector2 windowMinSize = Vector2.one * 300.0f;
    SerializedObject monsterSO = null;
    ReorderableList reorderlist = null;
    static Rect listRect = new Rect(Vector2.zero, windowMinSize);
    static Rect newRect = new Rect(310, 0, 200, 20);
    [MenuItem("Window/MonsterSpawner Settings")]

    public static void ShowWindow()
    {
        GetWindow(typeof(MonsterSpawnerWindow));
    }

    void OnEnable()
    {     
        monsterSpawners = new List<MonsterSpawner>(FindObjectsOfType<MonsterSpawner>());
        spawner = FindObjectOfType<MonsterSpawner>();

        foreach (var monsterSpawn in monsterSpawners)
        {
            monsterSO = new SerializedObject(monsterSpawn);
            reorderlist = new ReorderableList(monsterSO, monsterSO.FindProperty("monsters"), true, true, true, true);
            reorderlist.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Monster");
            reorderlist.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
               EditorGUI.PropertyField(rect, reorderlist.serializedProperty.GetArrayElementAtIndex(index));
        }
    }

    public void OnInspectorGUI()
    {
        monsterSO.Update();
        reorderlist.DoLayoutList();
        monsterSO.ApplyModifiedProperties();
    }

    void OnGUI()
    {
        EditorGUI.HelpBox(new Rect (310, 100, 400, 50), "When you want the list of monsters to effect all the spawners click 'apply' at the top of the inpector of the monster spawner its effecting to apply it to the prefab", MessageType.Info);
        spawner.amountToSpawn = EditorGUI.IntField(new Rect(newRect), "Amount To Spawn", spawner.amountToSpawn);
        spawner.range = EditorGUI.FloatField(new Rect(310, 20, 200, 20), "Range", spawner.range);
        spawner.respawnTime = EditorGUI.IntField(new Rect(310, 40, 200, 20), "Respawn Time", spawner.respawnTime);
        spawner.monsterToSpawn = (GameObject)EditorGUI.ObjectField(new Rect(310, 60, 400, 20), "Monster To Spawn", spawner.monsterToSpawn, typeof(GameObject));

        foreach (var monsterSpawn in monsterSpawners)
        {
            monsterSpawn.amountToSpawn = spawner.amountToSpawn;
            monsterSpawn.range = spawner.range;
            monsterSpawn.respawnTime = spawner.respawnTime;
            monsterSpawn.monsterToSpawn = spawner.monsterToSpawn;
            EditorUtility.SetDirty(monsterSpawn);
        }

        if (GUI.Button(new Rect(310, 80, 400, 20), "Spawn Monsters"))
        {
            //uses function in monsterspawner script to spawn a monster on all monster spawners
            foreach (var monsterSpawn in monsterSpawners)
            {
                monsterSpawn.MakeWolf();
            }
        }

        if (monsterSO != null)
        {
            monsterSO.Update();
            reorderlist.DoList(listRect);
            monsterSO.ApplyModifiedProperties();
        }
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}
