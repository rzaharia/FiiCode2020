using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreatePacManMaze))]
public class UpdateButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreatePacManMaze myScript = (CreatePacManMaze)target;
        if(GUILayout.Button("Update script"))
        {
            myScript.ReStart();
        }
        if (GUILayout.Button("Save script"))
        {
            myScript.SaveScript();
        }
        if (GUILayout.Button("Load script"))
        {
            myScript.LoadScript();
        }
    }
}
