using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour 
{
    public Tile tile { get; protected set; }
    public Directions dir;
    // public string unitName;

    public const int minLevel = 1;
    public const int maxLevel = 20;
    // public XPCurveData xpData;
    // public UnitStatData statData;
    public Sprite portrait;
    public Color portraitColor;

    [HideInInspector] public Stats statsScript;
    [HideInInspector] public UnitLevel levelScript;

    //public getter for level and experience
    // public int LV{
    //     get { return statsScript[StatTypes.LV]; }
    // }
    // public int XP{
    //     get { return statsScript[StatTypes.XP]; }
    //     set { statsScript[StatTypes.XP] = value; }
    // }
    public void Init(Tile t){
        Place(t);
        Match();

        statsScript = GetComponent<Stats>();
        // LoadBaseStats();

		this.AddObserver(GetComponent<UnitLevel>().OnLvlChangeNotification, Stats.DidChangeNotification(StatTypes.LV), statsScript);
		Feature[] features = GetComponentsInChildren<Feature>();
		for (int i = 0; i < features.Length; ++i)
			features[i].Activate(gameObject);

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
    }

    // public void LoadBaseStats (){
    //     // statsScript[StatTypes.LV] = 1;
    //     for (int i = 0; i < UnitStatData.statOrder.Length; ++i)
    //     {
    //         StatTypes type = UnitStatData.statOrder[i];
    //         statsScript.SetValue(type, statData.baseStats[i], false);
    //     }
    //     statsScript.SetValue(StatTypes.HP, statsScript[StatTypes.MHP], false);
    // }

    //will need to change this when i actually deal with unit death
    public void OnDeath(){
        // Debug.Log("ondeath");
        Feature[] features = GetComponentsInChildren<Feature>();
		for (int i = 0; i < features.Length; ++i)
			features[i].Deactivate();

		this.RemoveObserver(GetComponent<UnitLevel>().OnLvlChangeNotification, Stats.DidChangeNotification(StatTypes.LV), statsScript);
		statsScript = null;
        Destroy(gameObject);
    }
    // public virtual void OnLvlChangeNotification (object sender, object args){
	// 	int oldValue = (int)args;
	// 	int newValue = LV;

	// 	for (int i = oldValue; i < newValue; ++i)
	// 		GetComponent<UnitLevel>().LevelUp();
	// }

    // public void LevelUp(){
    //     Debug.Log("levelling up! currently lv " + LV + " out of " + xpData.maxLevel);
    //     for (int i = 0; i < UnitStatData.statOrder.Length; ++i)
    //     {
    //         StatTypes type = UnitStatData.statOrder[i];
    //         int fixedGrowth = Mathf.FloorToInt(statData.growStats[i]);
    //         float chanceGrowth = statData.growStats[i] - fixedGrowth;
    //         int value = statsScript[type];
    //         value += fixedGrowth;
    //         if (Random.value > (1f - chanceGrowth))
    //             value++;
    //         statsScript.SetValue(type, value, false);
    //     }
    //     statsScript.SetValue(StatTypes.HP, statsScript[StatTypes.MHP], false);
    //     Debug.Log("leveled up! " + XP + " xp");
    // }
    
    // public void GainExperience(int xpGained){
    //     XP += xpGained;
    //     while(XP >= xpData.experiencePerLevel[LV]){
    //         if(LV >= xpData.maxLevel){
    //             XP = 0;
    //             return;
    //         }
    //         else{
    //             XP -= xpData.experiencePerLevel[LV];
    //             statsScript.SetValue(StatTypes.LV, LV+1, false);
    //         }
    //         LevelUp();
    //     }
    // }

}