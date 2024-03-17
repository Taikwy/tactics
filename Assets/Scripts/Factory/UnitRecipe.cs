using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRecipe : ScriptableObject 
{
    public string unitBase;
     [Header("Sprites")]
    public Sprite sprite;
    public Sprite portrait;
    [Header("Animations")]
    public RuntimeAnimatorController idleAnim;
    public RuntimeAnimatorController outlineIdleAnim;
    public RuntimeAnimatorController iconAnim;
    // public RuntimeAnimatorController targetAnim;

    [Header("ui stuff")]
    public GameObject canvasPrefab;
    
     [Header("stat curves")]
    public XPCurveData xpData;
    public UnitStatData statData;
    [Header("general info")]
    public MovementTypes movementType;
    [Tooltip("for the movement coroutine moving speed")]public float moveSpeed = .035f;
    [Tooltip("for the movement coroutine delay after reaching a tile")]public float moveDelay = .05f;
    public Alliances alliance;
    public List<GameObject> equipment = new List<GameObject>();
    
     [Header("abilities and stuff")]
    public string attack;
    public string abilityCatalog;
    [Header("audio stuff")]
    public string hpIncreaseSound = "Health Increase";
    public string hpDecreaseSound = "Health Decrease";
    public string deathSound =  "Unit Death";
    public string moveSound;
    [Header("ai stuff")]
    public bool cpuDriver = false;
    public string attackPattern;

}