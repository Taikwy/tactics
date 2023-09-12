using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRecipe : ScriptableObject 
{
    public string unitBase;
    public XPCurveData xpData;
    public UnitStatData statData;
    public Sprite portrait;
    public string job;
    public string attack;
    public string abilityCatalog;
    public MovementTypes movementType;
    public Alliances alliance;
}