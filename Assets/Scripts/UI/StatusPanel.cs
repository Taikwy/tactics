using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class StatusPanel : MonoBehaviour 
{
	public const string GetAttackEvent = "BaseAbilityEffect.GetAttackEvent";
	public const string GetDefenseEvent = "BaseAbilityEffect.GetDefenseEvent";
	public const string GetCritRateEvent = "BaseAbilityEffect.GetCritRareEvent";
	public const string GetCritDMGEvent = "BaseAbilityEffect.GetCritDMGEvent";
	public const string GetSpeedEvent = "BaseAbilityEffect.GetSpeedEvent";
    public GameObject statusBG;
    public GameObject equipmentLabelPrefab, statusLabelPrefab, statusInfoPanelPrefab;
    [Space(2)][Header("Unit stuff")]
    public Image background;
    public Image portrait;
    public Sprite unitBackground;
    [Space(2)][Header("Unit Stats")]
    public TMP_Text nameLabel;
    public TMP_Text lvLabel, xpLabel, hpLabel, bpLabel;
    public TMP_Text skpLabel, atLabel, dfLabel, spLabel, cpLabel, cdLabel;
    // public TMP_Text weaponLabel, armorLabel, trinketLabel;
    [Header("Status Effect stuff")]
    public GameObject statusHolder;
    [Header("Equipment stuff")]
    public GameObject equipmentHolder, weaponLabel, armorLabel, trinketLabel;

    GameObject statusInfoPanel;
    
    //takes in unit gaemeobject
    public void Display (GameObject unit)
    {
        // Temp until I add a component to determine unit alliances
        // background.sprite = Random.value > 0.5f? enemyBackground : unitBackground;
        
        // portrait.sprite = null; Need a component which provides this data
        Unit unitScript = unit.GetComponent<Unit>();
        nameLabel.text = unitScript.name;
        portrait.sprite = unitScript.portrait;
        portrait.color = unitScript.portraitColor;
        Stats stats = unit.GetComponent<Stats>();
        Equipment equipment = unit.GetComponent<Equipment>();
        if (stats) {
            lvLabel.text = string.Format( "LV. {0}", stats[StatTypes.LV]);
            xpLabel.text = string.Format( "XP. {0} / {1}", stats.GetCurrentXP(), unit.GetComponent<UnitLevel>().xpData.experiencePerLevel[stats[StatTypes.LV]]);
            hpLabel.text = string.Format( "HEALTH {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP] );
            bpLabel.text = string.Format( "BURST {0} / {1}", stats[StatTypes.BP], stats[StatTypes.MBP] );
            skpLabel.text = string.Format( "SKILL PTS {0} / {1}", stats[StatTypes.SK], stats[StatTypes.MSK] );

            int modifiedAttack = GetStatForDisplay(unitScript, GetAttackEvent, 0);
            Debug.Log("modified attack " + modifiedAttack + " | base attack " + stats[StatTypes.AT]);
            // if(GetStatForDisplay(unitScript, GetAttackEvent, 0) >  stats[StatTypes.AT]){
                
            //     atLabel.text = string.Format( "ATTACK {0}", GetStatForDisplay(unitScript, GetAttackEvent, 0));
            // }
            atLabel.text = string.Format( "ATTACK {0}", stats[StatTypes.AT]);
            dfLabel.text = string.Format( "DEFENSE {0}", stats[StatTypes.DF]);
            spLabel.text = string.Format( "SPEED {0}", stats[StatTypes.SP]);
            cpLabel.text = string.Format( "CRIT% {0}", stats[StatTypes.CR]);
            cdLabel.text = string.Format( "CRITDMG {0}", stats[StatTypes.CD]);
        }


        weaponLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format("EMPTY ");
        armorLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format("EMPTY ");
        trinketLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format("EMPTY ");
        if(equipment){
            // foreach(Equippable item in equipment.equippedItems){
            //     GameObject equipmentLabel = Instantiate(equipmentLabelPrefab, equipmentHolder.transform);
            //     TMP_Text slotLabel = equipmentLabel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            //     TMP_Text itemLabel = equipmentLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

            //     slotLabel.text = string.Format(item.equippedSlot + " slot: ");
            //     itemLabel.text = string.Format(item +  " ");
            // }
            foreach(Equippable item in equipment.equippedItems){
                switch(item.equippedSlot){
                    case EquipSlots.Weapon:
                        weaponLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format(item +  " ");
                        break;
                    case EquipSlots.Armor:
                        armorLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format(item +  " ");
                        break;
                    case EquipSlots.Trinket:
                        trinketLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format(item +  " ");
                        break;

                }
            }
            // weaponLabel.text = string.Format( "Weapon: " + equipment.equippedWeapon);
            // armorLabel.text = string.Format( "Armor: " + equipment.equippedArmor);
            // trinketLabel.text = string.Format( "Trinket: " + equipment.equippedTrinket);

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
        Debug.Log("getting stats for status panel");
        // this.PostEvent(GetStatEvent, StatTypes.AT);
        
		var modifiers = new List<ValueModifier>();															//list of all modifiers, INCLUDING base stat (ie unit's stat would jhsut be an addvaluemodifier with that stat)
		var info = new Info<Unit, Unit, List<ValueModifier>>(actor, null, modifiers);
		actor.PostEvent(eventName, info);																	//posts the event, power script auto adds the modifier for the base stat
		modifiers.Sort(Compare);
		
        //applies all the modifiers to the value
		float value = startValue;
		for (int i = 0; i < modifiers.Count; ++i)
			value = modifiers[i].Modify(startValue, value);
		
		return (int)value;
	}
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