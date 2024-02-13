using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRecipe : ScriptableObject 
{
    public string unitBase;
     [Header("Sprites")]
    public Sprite sprite;
    public Sprite portrait;
    
     [Header("stat curves")]
    public XPCurveData xpData;
    public UnitStatData statData;
    public string job;
    
     [Header("attacks and stuff")]
    public string attack;
    public string abilityCatalog;
    public MovementTypes movementType;
    public Alliances alliance;
    public List<GameObject> equipment = new List<GameObject>();
    // public Color portraitColor;
}