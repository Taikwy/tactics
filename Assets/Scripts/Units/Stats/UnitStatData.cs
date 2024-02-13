using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Stat Data", menuName = "ScriptableObjects/UnitStatData", order = 1)]
public class UnitStatData : ScriptableObject
{
    

    // public static readonly StatTypes[] statOrder = new StatTypes[]
    // {
    //     StatTypes.MHP,
    //     StatTypes.AT,
    //     StatTypes.DF,
    //     StatTypes.SK,
    //     StatTypes.LU,
    //     StatTypes.SP,
    //     StatTypes.MV
    // };

    public static readonly StatTypes[] statOrder = new StatTypes[]{
        StatTypes.MHP,
        StatTypes.MBP,
        StatTypes.SK,
        StatTypes.MV,

        StatTypes.AT,
        StatTypes.DF,
        StatTypes.SP,
        
        StatTypes.CP,
        StatTypes.CD,
    };
    public static readonly StatTypes[] fixedStatOrder = new StatTypes[]{
        StatTypes.MBP,
        StatTypes.MSK,
        StatTypes.MV,

        StatTypes.SP,
        StatTypes.CP,
        StatTypes.CD,
    };
    public static readonly StatTypes[] combatStatOrder = new StatTypes[]{
        StatTypes.MHP,
        StatTypes.AT,
        StatTypes.DF,
    };

    // [Tooltip("Max HP, Burst, Skill Points, Move, Attack, Defense, Speed, Crit%, CritDMG")]
    // public int[] baseStats = new int[ statOrder.Length ];
    // [Tooltip("Max HP, Burst, Skill Points, Move, Attack, Defense, Speed, Crit%, CritDMG")]
    // public float[] growStats = new float[ statOrder.Length ];
    

    [Tooltip("Burst, Skill Points, Move, Speed, Crit%, CritDMG")]
    public int[] fixedStats = new int[ fixedStatOrder.Length ];
    [Tooltip("Health, Attack, Defense")]
    public int[] combatStats = new int[ combatStatOrder.Length ];
    [Tooltip("Health, Attack, Defense")]
    public float[] growthStats = new float[ combatStatOrder.Length ];
    
    public int minHP = 0;

    // public int minLevel = 1;
    // public int maxLevel = 20;

}