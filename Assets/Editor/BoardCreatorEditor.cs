using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorEditor : Editor
{
    public BoardCreator current{
        get{
            return (BoardCreator)target;
        }
    }

    public override void OnInspectorGUI (){
        DrawDefaultInspector();
        GUILayout.Space(5f);

        if (GUILayout.Button("Generate"))
            current.Generate();
        if (GUILayout.Button("Generate Wall"))
            current.GenerateWall();
        GUILayout.Space(10f);

        if (GUILayout.Button("Generate Area"))
            current.GenerateArea();
        if (GUILayout.Button("Clear"))
            current.Clear();
        GUILayout.Space(10f);

        if (GUILayout.Button("Save"))
            current.Save();
        if (GUILayout.Button("Load"))
            current.Load();
        if (GUI.changed)
            current.UpdateSelectionMarker ();


        // if (GUILayout.Button("Grow"))
        //     current.Grow();
        // if (GUILayout.Button("Shrink"))
        //     current.Shrink();
        // if (GUILayout.Button("Grow Area"))
        //     current.GrowArea();
        // if (GUILayout.Button("Shrink Area"))
        //    current.ShrinkArea();

    }
}
