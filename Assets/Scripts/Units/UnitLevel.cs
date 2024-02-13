using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class UnitLevel : MonoBehaviour
{
    public const int minLevel = 1;
    public const int maxLevel = 20;
    public XPCurveData xpData;
    
    //public getter for level and experience
    public int LV{
        get { return statsScript[StatTypes.LV]; }
    }
    public int XP{
        get { return statsScript[StatTypes.XP]; }
        set { statsScript[StatTypes.XP] = value; }
    }
    public int currentXP{
        get { return xpData.experiencePerLevel[LV]; }
    }
    //idk what this is, i think its unused but ij ust copied it over
    //maybe i can use for ui?
    public float LevelPercent{
		get { return (LV - minLevel) / (float)(maxLevel - minLevel); }
	}
    Stats statsScript;

    void Awake (){
        statsScript = GetComponent<Stats>();
    }

    //subscribes xpwillchange and xpdidchange methods to stats script's notification 
    void OnEnable (){
		this.AddObserver(OnExpWillChange, Stats.WillChangeNotification(StatTypes.XP), statsScript);
		this.AddObserver(OnExpDidChange, Stats.DidChangeNotification(StatTypes.XP), statsScript);
	}
	void OnDisable (){
		this.RemoveObserver(OnExpWillChange, Stats.WillChangeNotification(StatTypes.XP), statsScript);
		this.RemoveObserver(OnExpDidChange, Stats.DidChangeNotification(StatTypes.XP), statsScript);
	}

    void OnExpWillChange (object sender, object args){
		ValueChangeException vce = args as ValueChangeException;
        //clamps xp minimum to current xp, preventing it from ever decreasing. also clamps to max total xp, preventing it from ever going past
		vce.AddModifier(new ClampValueModifier(int.MaxValue, XP, ExperienceForLevel(maxLevel)));
	}
	
	void OnExpDidChange (object sender, object args){
		statsScript.SetValue(StatTypes.LV, LevelForExperience(XP), false);
	}

    //returns xp needed to level up AT this level
    public int ExperienceCurrentForLevel (int level){
        return xpData.experiencePerLevel[level];
    }
    //returns xp needed to level up TO this level
    public int ExperienceForLevel (int level){
        int totalXP = 0;
        for(int i = 0; i < level; i++){
            totalXP += xpData.experiencePerLevel[i];
        }
        return totalXP;
    }
    
    //returns level of unit with amount of total xp
    public int LevelForExperience (int exp){
        int lvl = maxLevel;
        for (; lvl >= minLevel; --lvl)
            if (exp >= ExperienceForLevel(lvl))
                break;
        return lvl;
    }


    //when initializing unit, sets the stuff
    public void Init (int level, XPCurveData data){
        xpData = data;
        statsScript.SetValue(StatTypes.LV, level, false);
        statsScript.SetValue(StatTypes.XP, ExperienceForLevel(level), false);
    }

    public virtual void OnLvlChangeNotification (object sender, object args){
		int oldValue = (int)args;
		int newValue = LV;

		for (int i = oldValue; i < newValue; ++i)
			LevelUp();
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
    public void LevelUp(){
        Debug.Log("levelling up! currently lv " + LV + " out of " + xpData.maxLevel);
        for (int i = 0; i < UnitStatData.statOrder.Length; ++i)
        {
            StatTypes type = UnitStatData.statOrder[i];
            int fixedGrowth = Mathf.FloorToInt(statsScript.statData.growStats[i]);
            float chanceGrowth = statsScript.statData.growStats[i] - fixedGrowth;
            int value = statsScript[type];
            value += fixedGrowth;
            if (Random.value > (1f - chanceGrowth))
                value++;
            statsScript.SetValue(type, value, false);
        }
        statsScript.SetValue(StatTypes.HP, statsScript[StatTypes.MHP], false);
        Debug.Log("leveled up! " + XP + " xp");
    }

    // public void AwardExperience(int amount){
    //     XP += amount;
    //     //check if unit has levelled up
    // }
}
