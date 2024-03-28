using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSetter))]
public class LevelSetterEditor : Editor
{
    public LevelSetter levelSetter{
        get{
            return (LevelSetter)target;
        }
    }

    public override void OnInspectorGUI (){
        DrawDefaultInspector();
        GUILayout.Space(30f);
        if (GUILayout.Button("Set Level Data from Layout"))
            levelSetter.SetLevel();
        if (GUILayout.Button("Clear"))
            levelSetter.Clear();

        GUILayout.Space(15f);
            
        if (GUILayout.Button("Save LevelData"))
            levelSetter.SaveLevel();
        if (GUILayout.Button("Load LevelData"))
            levelSetter.LoadLevel();

    }
}
