using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Unit : MonoBehaviour 
{
    public Tile tile { get; protected set; }
    public Directions dir;
    public string unitName;
    // public UnitStats stats;
    public UnitStatData statData;

    public const int minLevel = 1;
    public const int maxLevel = 20;
    public XPCurveData xpData;
    

    public int LV{
        get { return statsScript[StatTypes.LV]; }
    }
    public int XP{
        get { return statsScript[StatTypes.XP]; }
        set { statsScript[StatTypes.XP] = value; }
    }

    // public int[] stats = new int[UnitStatData.statOrder.Length ];
    // public float[] growStats;
    [HideInInspector] public Stats statsScript;

    public void Init(Tile t){
        Place(t);
        Match();
        // statData.SetStats(s);
        // statData.LoadDefaultStats();
        statsScript = gameObject.AddComponent<Stats>();
        
        LoadBaseStats();
        // unitName = statData.unitName;


        if(statData.canFly){
            gameObject.AddComponent<FlyMovement>();
        }
        else{
            gameObject.AddComponent<WalkMovement>();
        }
    }
    public void Place (Tile target){
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
        tile.content = null;
        
        // Link unit and tile references
        tile = target;
        
        if (target != null)
        target.content = gameObject;
    }
    public void Match (){
        transform.localPosition = tile.center;
        // transform.localEulerAngles = dir.ToEuler();
    }

    public void LoadBaseStats (){
        statsScript[StatTypes.LV] = 1;
        for (int i = 0; i < UnitStatData.statOrder.Length; ++i)
        {
            StatTypes type = UnitStatData.statOrder[i];
            statsScript.SetValue(type, statData.baseStats[i], false);
        }
        statsScript.SetValue(StatTypes.HP, statsScript[StatTypes.MHP], false);
    }

    public void LevelUp(){
        Debug.Log("levelling up! currently lv " + LV + " out of " + xpData.maxLevel);
        // statsScript.SetValue(StatTypes.HP, statsScript[StatTypes.MHP], false);
        for (int i = 0; i < UnitStatData.statOrder.Length; ++i)
        {
            StatTypes type = UnitStatData.statOrder[i];
            int fixedGrowth = Mathf.FloorToInt(statData.growStats[i]);
            float chanceGrowth = statData.growStats[i] - fixedGrowth;
            int value = statsScript[type];
            value += fixedGrowth;
            if (Random.value > (1f - chanceGrowth))
                value++;
            statsScript.SetValue(type, value, false);
        }
        statsScript.SetValue(StatTypes.HP, statsScript[StatTypes.MHP], false);
    }
    
    public void GainExperience(int xpGained){
        XP += xpGained;
        while(XP >= xpData.experiencePerLevel[LV]){
            if(LV >= xpData.maxLevel){
                XP = 0;
                return;
            }
            else{
                XP -= xpData.experiencePerLevel[LV];
                statsScript.SetValue(StatTypes.LV, LV+1, false);
            }
            LevelUp();
        }
    }


    
}