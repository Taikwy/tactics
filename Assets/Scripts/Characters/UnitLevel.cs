using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLevel : MonoBehaviour
{
    public const int minLevel = 1;
    public const int maxLevel = 20;
    public XPCurveData xpData;
    

    public int LV{
        get { return stats[StatTypes.LV]; }
    }
    public int XP{
        get { return stats[StatTypes.XP]; }
        set { stats[StatTypes.XP] = value; }
    }
    Stats stats;


    // #region MonoBehaviour
    void Awake ()
    {
        stats = GetComponent<Stats>();
    }


    public int ExperienceForLevel (int level){
        return xpData.experiencePerLevel[level];
    }
    
    public int LevelForExperience (int exp){
        int lvl = maxLevel;
        for (; lvl >= minLevel; --lvl)
            if (exp >= ExperienceForLevel(lvl))
                break;
        return lvl;
    }
    public void Init (int level){
        stats.SetValue(StatTypes.LV, level, false);
        stats.SetValue(StatTypes.XP, ExperienceForLevel(level), false);
    }

    public void AwardExperience(int amount){
        XP += amount;
        //check if unit has levelled up
    }
}
