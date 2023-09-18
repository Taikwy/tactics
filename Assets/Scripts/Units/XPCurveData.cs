using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XP Curve", menuName = "ScriptableObjects/XPCurve", order = 1)]
public class XPCurveData : ScriptableObject
{
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
}
