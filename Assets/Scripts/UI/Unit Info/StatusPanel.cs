using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class StatusPanel : MonoBehaviour 
{
	public const string GetAttackEvent = "StatusPanel.GetAttackEvent";
	public const string GetDefenseEvent = "StatusPanel.GetDefenseEvent";
	public const string GetCritRateEvent = "StatusPanel.GetCritRateEvent";
	public const string GetCritDMGEvent = "StatusPanel.GetCritDMGEvent";
	public const string GetSpeedEvent = "StatusPanel.GetSpeedEvent";
    public GameObject statusBG;
    public GameObject equipmentLabelPrefab, statusLabelPrefab, statusInfoPanelPrefab;
    [Space(2)][Header("Unit stuff")]
    public Image background;
    public Image portrait;
    public Sprite unitBackground;
    [Space(2)][Header("Unit Stats")]
    public TMP_Text nameLabel;
    public TMP_Text lvLabel, xpLabel, mvLabel, hpLabel, bpLabel;
    public TMP_Text skpLabel, atLabel, dfLabel, spLabel, cpLabel, cdLabel;
    // public TMP_Text weaponLabel, armorLabel, trinketLabel;
    [Header("Status Effect stuff")]
    public GameObject statusHolder;

    GameObject statusInfoPanel;
    void OnEnable(){
        // Debug.Log("enabked");
    }
    void AddObservers(){
		this.AddObserver(OnGetBaseAttack, StatusPanel.GetAttackEvent);
		// this.AddObserver(OnTestGet, StatusPanel.GetAttackEvent);
		this.AddObserver(OnGetBaseDefense, GetDefenseEvent);
		this.AddObserver(OnGetBaseCritRate, GetCritRateEvent);
		this.AddObserver(OnGetBaseCritDMG, GetCritDMGEvent);
		this.AddObserver(OnGetBaseSpeed, GetSpeedEvent);
    }
    void OnDisable(){
        // Debug.Log("disabled");
		this.RemoveObserver(OnGetBaseAttack, GetAttackEvent);
		// this.RemoveObserver(OnTestGet, StatusPanel.GetAttackEvent);
		this.RemoveObserver(OnGetBaseDefense, GetDefenseEvent);
		this.RemoveObserver(OnGetBaseCritRate, GetCritRateEvent);
		this.RemoveObserver(OnGetBaseCritDMG, GetCritDMGEvent);
		this.RemoveObserver(OnGetBaseSpeed, GetSpeedEvent);
    }
    
    //takes in unit gaemeobject
    public void Display (GameObject unit)
    {
        AddObservers();
        
        Unit unitScript = unit.GetComponent<Unit>();
        nameLabel.text = unitScript.name;
        nameLabel.color = unit.GetComponent<Unit>().unitColor;
        portrait.sprite = unitScript.portrait;
        // portrait.color = unitScript.portraitColor;
        Stats stats = unit.GetComponent<Stats>();
        Equipment equipment = unit.GetComponent<Equipment>();
        if (stats) {
            lvLabel.text = string.Format( "LV. {0}", stats[StatTypes.LV]);
            xpLabel.text = string.Format( "XP. {0} / {1}", stats.GetCurrentXP(), unit.GetComponent<UnitLevel>().xpData.experiencePerLevel[stats[StatTypes.LV]]);
            if(unit.GetComponent<Unit>().moveScript.GetType() == typeof(WalkMovement)){
                 mvLabel.text = "WALKING";
            }
            if(unit.GetComponent<Unit>().moveScript.GetType() == typeof(FlyMovement)){
                 mvLabel.text = "FLYING";
            }
            if(unit.GetComponent<Unit>().moveScript.GetType() == typeof(TeleportMovement)){
                 mvLabel.text = "TELEPORTING";
            }
            hpLabel.text = string.Format( "HEALTH {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP] );
            bpLabel.text = string.Format( "BURST {0} / {1}", stats[StatTypes.BP], stats[StatTypes.MBP] );
            skpLabel.text = string.Format( "SKILL PTS {0} / {1}", stats[StatTypes.SK], stats[StatTypes.MSK] );
            

            
            // Debug.Log("==================================");
            // Debug.Log("base atack tho " + stats[StatTypes.AT]);
            atLabel.text = string.Format( "ATK:{0}", GetStatForDisplay(unitScript, GetAttackEvent, 0));
            if(GetStatForDisplay(unitScript, GetAttackEvent, 0) >  stats[StatTypes.AT]) atLabel.color = Color.green;
            else if(GetStatForDisplay(unitScript, GetAttackEvent, 0) <  stats[StatTypes.AT]) atLabel.color = Color.red;
            else atLabel.color = Color.white;
            // Debug.Log("base attack " + stats[StatTypes.AT] + " | modified attack " + GetStatForDisplay(unitScript, GetAttackEvent, 0));
            dfLabel.text = string.Format( "DEF:{0}", GetStatForDisplay(unitScript, GetDefenseEvent, 0));
            if(GetStatForDisplay(unitScript, GetDefenseEvent, 0) >  stats[StatTypes.DF]) dfLabel.color = Color.green;
            else if(GetStatForDisplay(unitScript, GetDefenseEvent, 0) <  stats[StatTypes.DF]) dfLabel.color = Color.red;
            else dfLabel.color = Color.white;
            spLabel.text = string.Format( "SPD:{0}", GetStatForDisplay(unitScript, GetSpeedEvent, 0));
            if(GetStatForDisplay(unitScript, GetSpeedEvent, 0) >  stats[StatTypes.SP]) spLabel.color = Color.green;
            else if(GetStatForDisplay(unitScript, GetSpeedEvent, 0) <  stats[StatTypes.SP]) spLabel.color = Color.red;
            else spLabel.color = Color.white;
            cpLabel.text = string.Format( "CRIT%:{0}", GetStatForDisplay(unitScript, GetCritRateEvent, 0));
            if(GetStatForDisplay(unitScript, GetCritRateEvent, 0) >  stats[StatTypes.CR]) cpLabel.color = Color.green;
            else if(GetStatForDisplay(unitScript, GetCritRateEvent, 0) <  stats[StatTypes.CR]) cpLabel.color = Color.red;
            else cpLabel.color = Color.white;
            cdLabel.text = string.Format( "CRITDMG:{0}", GetStatForDisplay(unitScript, GetCritDMGEvent, 0));
            if(GetStatForDisplay(unitScript, GetCritDMGEvent, 0) >  stats[StatTypes.CD]) cdLabel.color = Color.green;
            else if(GetStatForDisplay(unitScript, GetCritDMGEvent, 0) <  stats[StatTypes.CD]) cdLabel.color = Color.red;
            else cdLabel.color = Color.white;

            // Debug.Log("================================================================");
            // int modifiedAttack = GetStatForDisplay(unitScript, GetAttackEvent, stats[StatTypes.AT]);
            // Debug.Log("modified attack " + modifiedAttack + " | base attack " + stats[StatTypes.AT]);
            // atLabel.text = string.Format( "ATTACK {0}", modifiedAttack);

            // atLabel.text = string.Format( "ATTACK {0}", stats[StatTypes.AT]);
            // dfLabel.text = string.Format( "DEFENSE {0}", stats[StatTypes.DF]);
            // spLabel.text = string.Format( "SPEED {0}", stats[StatTypes.SP]);
            // cpLabel.text = string.Format( "CRIT% {0}", stats[StatTypes.CR]);
            // cdLabel.text = string.Format( "CRITDMG {0}", stats[StatTypes.CD]);
        }
        foreach(Transform label in statusHolder.transform){
            Destroy(label.gameObject);
        }
        Status status = unit.GetComponent<Status>();
        if(status){
            
            foreach(GameObject effect in status.statuses){
		        GameObject statusLabel = Instantiate(statusLabelPrefab, statusHolder.transform);
                StatusLabel label = statusLabel.GetComponent<StatusLabel>();
                TMP_Text effectLabel = statusLabel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
                TMP_Text durationLabel = statusLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

                effectLabel.text = string.Format( "{0} EFFECT", effect.GetComponent<StatusEffect>().statusName);
                if(effect.GetComponent<DurationStatusCondition>())
                    durationLabel.text = string.Format( "{0} TURNS LEFT", effect.GetComponent<DurationStatusCondition>().duration);
                
                label.highlightFunc = delegate { CreateStatusInfoPanel(statusLabel, effect); };
                label.unhighlightFunc = delegate { DestroyStatusInfoPanel(); };
            }
        }
    }

    public void ShowStatus(){
        statusBG.SetActive(true);
    }
    public void HideStatus(){
        statusBG.SetActive(false);
    }
    protected virtual int GetStatForDisplay (Unit actor, string eventName, int startValue){
        // Debug.Log("getting stats for status panel");
        // this.PostEvent(GetStatEvent, StatTypes.AT);
        // Debug.Log(eventName + " getting  stats for status panel | starting val " + startValue);
        
		var modifiers = new List<ValueModifier>();															//list of all modifiers, INCLUDING base stat (ie unit's stat would jhsut be an addvaluemodifier with that stat)
		var info = new Info<Unit, Unit, List<ValueModifier>>(actor, actor, modifiers);
		this.PostEvent(eventName, info);																	
		modifiers.Sort(Compare);
        // Debug.Log(modifiers.Count + " num modifers");
        //applies all the modifiers to the value
		float value = startValue;
		for (int i = 0; i < modifiers.Count; ++i){
			// Debug.Log(modifiers[i] + " add amt " + (modifiers[i] as AddValueModifier).toAdd);		
			value = modifiers[i].Modify(startValue, value);
        }
		
		// Debug.Log("=============final modified value " + value);
		return (int)value;
	}
    void OnGetBaseAttack (object sender, object args){
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
        info.arg2.Add( new AddValueModifier(0, info.arg0.GetComponent<Stats>()[StatTypes.AT]) );
	}void OnGetBaseDefense (object sender, object args){
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
        info.arg2.Add( new AddValueModifier(0, info.arg0.GetComponent<Stats>()[StatTypes.DF]) );
	}void OnGetBaseSpeed (object sender, object args){
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
        info.arg2.Add( new AddValueModifier(0, info.arg0.GetComponent<Stats>()[StatTypes.SP]) );
	}void OnGetBaseCritRate (object sender, object args){
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
        info.arg2.Add( new AddValueModifier(0, info.arg0.GetComponent<Stats>()[StatTypes.CR]) );
	}void OnGetBaseCritDMG (object sender, object args){
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
        info.arg2.Add( new AddValueModifier(0, info.arg0.GetComponent<Stats>()[StatTypes.CD]) );
	}

    // protected virtual int GetStatForCombat (Unit attacker, Unit target, string eventName, int startValue){
    //     Debug.Log(eventName + " getting base stats for combat | starting val " + startValue);
	// 	var modifiers = new List<ValueModifier>();															//list of all modifiers, INCLUDING base stat (ie unit's stat would jhsut be an addvaluemodifier with that stat)
	// 	var info = new Info<Unit, Unit, List<ValueModifier>>(attacker, target, modifiers);
	// 	this.PostEvent(eventName, info);																	//posts the event, power script auto adds the modifier for the base stat
	// 	modifiers.Sort(Compare);
		
    //     //applies all the modifiers to the value
	// 	float value = startValue;
	// 	for (int i = 0; i < modifiers.Count; ++i){
	// 		Debug.Log(modifiers[i] + " add amt " + (modifiers[i] as AddValueModifier).toAdd);			
	// 		value = modifiers[i].Modify(startValue, value);

	// 	}
		
	// 	Debug.Log("middle modified value " + value);
    //     //floors value as an int and clamps within damage range
	// 	int retValue = Mathf.FloorToInt(value);
	// 	retValue = Mathf.Clamp(retValue, minDamage, maxDamage);
	// 	Debug.Log("final modified value " + retValue);
	// 	return retValue;
	// }

    //returns the modifier that should trigger first
	int Compare (ValueModifier x, ValueModifier y){
		return x.sortOrder.CompareTo(y.sortOrder);
	}

    void CreateStatusInfoPanel(GameObject status, GameObject effect){
        Debug.Log("creating status panel");
        Destroy(statusInfoPanel);
        Vector2 pos = status.transform.position;
        pos += new Vector2(0, 10);
        statusInfoPanel = Instantiate(statusInfoPanelPrefab, pos, Quaternion.identity, status.transform);
        statusInfoPanel.GetComponent<StatusInfoPanel>().Setup(effect);
        statusInfoPanel.GetComponent<StatusInfoPanel>().ShowPanel();
    }
    void DestroyStatusInfoPanel(){
        if(statusInfoPanel && statusInfoPanel.GetComponent<StatusInfoPanel>())
            statusInfoPanel.GetComponent<StatusInfoPanel>().HidePanel();
        Destroy(statusInfoPanel);
    }
}