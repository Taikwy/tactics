using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Stat Data", menuName = "ScriptableObjects/UnitStatData", order = 1)]
public class UnitStatData : ScriptableObject
{
    

    public static readonly StatTypes[] statOrder = new StatTypes[]
    {
        StatTypes.MHP,
        StatTypes.AT,
        StatTypes.DF,
        StatTypes.SK,
        StatTypes.LU,
        StatTypes.SP,
        StatTypes.MV
    };

    [Tooltip("Max HP, Attack, Defense, Skill, Luck, Speed, Move")]
    public int[] baseStats = new int[ statOrder.Length ];
    [Tooltip("Max HP, Attack, Defense, Skill, Luck, Speed, Move")]
    public float[] growStats = new float[ statOrder.Length ];
    
    // Stats statsScript;
    // public String unitName;

    public bool canFly = false;


    public int minLevel = 1;
    public int maxLevel = 20;
    public int[] experiencePerLevel = new int[21];

    public int GetTotalXP(int currentLevel, int currentXP){
        int totalXP = currentXP;
        for(int i = 0; i < currentLevel; i++){
            totalXP += experiencePerLevel[i];
        }
        return totalXP;
    }

  
    // public void SetStats(Stats stats){
    //     this.statsScript = stats;
    // }

    // public void LoadDefaultStats (){
    //     for (int i = 0; i < statOrder.Length; ++i)
    //     {
    //         StatTypes type = statOrder[i];
    //         statsScript.SetValue(type, stats[i], false);
    //     }
    //     statsScript.SetValue(StatTypes.HP, statsScript[StatTypes.MHP], false);
    // }
    
}