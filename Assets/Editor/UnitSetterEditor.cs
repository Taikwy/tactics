using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitSetter))]
public class UnitSetterEditor : Editor
{
    public UnitSetter unitSetter{
        get{
            return (UnitSetter)target;
        }
    }

    public override void OnInspectorGUI (){
        DrawDefaultInspector();
        GUILayout.Space(30f);
        if (GUILayout.Button("Set (unit layout) Level Recipe from Layout"))
            unitSetter.SetUnits();
        // if (GUILayout.Button("Clear"))
        //     unitSetter.Clear();

        GUILayout.Space(15f);
            
        if (GUILayout.Button("Save LevelRecipe"))
            unitSetter.SaveRecipe();
        // if (GUILayout.Button("Load LevelData"))
        //     unitSetter.LoadLevel();

    }
}
