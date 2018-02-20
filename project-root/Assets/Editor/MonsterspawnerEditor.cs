using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Monsterspawner))]
public class MonsterspawnerEditor : Editor
{
    
   
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("click the check box of the monster you would like to spawn and type the amount you would like to spawn then when you want it to respawn under time to spawn", MessageType.Info);
        Monsterspawner mySpawner = (Monsterspawner)target;
        if (GUILayout.Button("Spawn Wolf"))
        {
            mySpawner.MakeWolf();
        }
    }
}
