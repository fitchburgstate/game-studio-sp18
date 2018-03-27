using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavMeshSpawner))]
public class NavSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Drag your monster prefab onto the moster list to make it spawn. If 'Amount To Spawn' is 1 then it will spawn at origin point. If not it will spawn randomly on the Nav Mesh", MessageType.Info);
        EditorGUILayout.HelpBox("Spawn Monsters button spawns a monster at the origin of the spawn point when you drag the monster prefab into Monster To Spawn", MessageType.Info);
        NavMeshSpawner mySpawner = (NavMeshSpawner)target;
        if (GUILayout.Button("Spawn Monster"))
        {
            mySpawner.MakeWolf();
        }
    }
}
