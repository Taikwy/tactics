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
    [Header("general info")]
    public MovementTypes movementType;
    public Alliances alliance;
    public List<GameObject> equipment = new List<GameObject>();
    
     [Header("abilities and stuff")]
    public string attack;
    public string abilityCatalog;
    [Header("ai stuff")]
    public bool cpuDriver = false;
    public string attackPattern;

}