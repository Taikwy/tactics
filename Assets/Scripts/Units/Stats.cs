using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    //indexer, lets me ref this class like an array, directly accessing _data[] in this case
    public int this[StatTypes s]
    {
        get { return _data[(int)s]; }
        set { SetValue(s, value, true); }
    }
    int[] _data = new int[ (int)StatTypes.Count ];

    //dictionary storing eventnames based on stat type
    static Dictionary<StatTypes, string> _willChangeEvents = new Dictionary<StatTypes, string>();
    static Dictionary<StatTypes, string> _didChangeEvents = new Dictionary<StatTypes, string>();
    public UnitStatData statData;

    //returns notifcation name for use with the eventcenter
    public static string WillChangeEvent (StatTypes type){
        //checks if a willchangenoti exists for this stat, creates a new one if not
        if (!_willChangeEvents.ContainsKey(type))
            _willChangeEvents.Add(type, string.Format("Stats.{0}WillChange", type.ToString()));
        return _willChangeEvents[type];
    }
    //returns notifcation name for use with the eventcenter
    public static string DidChangeEvent (StatTypes type){
        //checks if a didchangenoti exists for this stat, creates a new one if not
        if (!_didChangeEvents.ContainsKey(type))
            _didChangeEvents.Add(type, string.Format("Stats.{0}DidChange", type.ToString()));
        return _didChangeEvents[type];
    }

    public void SetValue (StatTypes type, int value, bool allowExceptions){
        int oldValue = this[type];
        if (oldValue == value)
            return;
        
        if (allowExceptions){
            // Allow exceptions to the rule here
            ValueChangeException exc = new ValueChangeException( oldValue, value );
            
            //posts event that this stat type will change
            this.PostEvent(WillChangeEvent(type), exc);
            
            //gets the new value after applying all modifiers
            value = Mathf.FloorToInt(exc.GetModifiedValue());
            
            //check if stat changed or not
            if (exc.toggle == false || value == oldValue)
                return;
        }
        
        _data[(int)type] = value;
        //posts event that this stat type did change
        this.PostEvent(DidChangeEvent(type), oldValue);
    }

    public int GetCurrentXP(){
        int value = this[StatTypes.XP] - GetComponent<UnitLevel>().ExperienceForLevel(this[StatTypes.LV]);
        return value;
    }
    
    //originally in unit.cs, moved here to simplify unit script. loads in and sets all the default stats for unit during spawning
    public void InitBaseStats (){
		// SetValue(StatTypes.LV, 1, false);
        // for (int i = 0; i < UnitStatData.statOrder.Length; ++i){
        //     StatTypes type = UnitStatData.statOrder[i];
        //     SetValue(type, statData.baseStats[i], false);
        // }

        //ALWAYS SET INITITAL STAT VALUE AS 1 SO WHEN ADDING THE REAL LEVEL I DON'T ADD AN EXTRA LAYER OF GROWTH STATS
        SetValue(StatTypes.LV, 1, false);
        for (int i = 0; i < UnitStatData.fixedStatOrder.Length; ++i){
            StatTypes type = UnitStatData.fixedStatOrder[i];
            SetValue(type, statData.fixedStats[i], false);
        }
        for (int i = 0; i < UnitStatData.combatStatOrder.Length; ++i){
            StatTypes type = UnitStatData.combatStatOrder[i];
            SetValue(type, statData.combatStats[i], false);
        }
        SetValue(StatTypes.HP, this[StatTypes.MHP], false);
        SetValue(StatTypes.BP, this[StatTypes.MBP], false);
        SetValue(StatTypes.SK, this[StatTypes.MSK], false);
    }
}
