using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSetter))]
public class LevelSetterEditor : Editor
{
    public LevelSetter setter{
        get{
            return (LevelSetter)target;
        }
    }

    public override void OnInspectorGUI (){
        DrawDefaultInspector();
        GUILayout.Space(30f);
        if (GUILayout.Button("Set Level Data from Layout"))
            setter.SetLevel();
        if (GUILayout.Button("Clear"))
            setter.Clear();

        GUILayout.Space(15f);
            
        if (GUILayout.Button("Save LevelData"))
            setter.SaveLevel();
        if (GUILayout.Button("Load LevelData"))
            setter.LoadLevel();

    }
}
