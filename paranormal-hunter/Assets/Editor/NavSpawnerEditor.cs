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
        
        NavMeshSpawner mySpawner = (NavMeshSpawner)target;
        if (GUILayout.Button("Spawn Monster"))
        {
            mySpawner.MakeWolf();
        }
    }


}
