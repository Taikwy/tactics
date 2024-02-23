using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class DetailsPanel : MonoBehaviour 
{
	public const string GetAttackEvent = "StatusPanel.GetAttackEvent";
	public const string GetDefenseEvent = "StatusPanel.GetDefenseEvent";
	public const string GetCritRateEvent = "StatusPanel.GetCritRateEvent";
	public const string GetCritDMGEvent = "StatusPanel.GetCritDMGEvent";
	public const string GetSpeedEvent = "StatusPanel.GetSpeedEvent";
    public GameObject detailsBG;
    public GameObject equipmentLabelPrefab, equipmentInfoPanelPrefab;
    [Space(2)][Header("Unit stuff")]
    public Image background;
    public Image portrait;
    public Sprite unitBackground;
    [Space(2)][Header("Unit Stats")]
    public TMP_Text nameLabel;
    public TMP_Text lvLabel, xpLabel;
    // public TMP_Text weaponLabel, armorLabel, trinketLabel;
    [Header("Equipment stuff")]
    public GameObject equipmentHolder, weaponLabel, armorLabel, trinketLabel;
    [Header("Ability stuff")]
    public GameObject abilityHolder;

    GameObject equipmentInfoPanel;
    
    //takes in unit gaemeobject
    public void Display (GameObject unit){
        Unit unitScript = unit.GetComponent<Unit>();
        nameLabel.text = unitScript.name;
        portrait.sprite = unitScript.portrait;
        portrait.color = unitScript.portraitColor;
        Equipment equipment = unit.GetComponent<Equipment>();

        weaponLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format("EMPTY ");
        armorLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format("EMPTY ");
        trinketLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format("EMPTY ");
        if(equipment){
            foreach(Equippable item in equipment.equippedItems){
                switch(item.equippedSlot){
                    case EquipSlots.Weapon:
                        weaponLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format(item +  " ");
                        // EquipLabel label = weaponLabel.GetComponent<EquipLabel>();
                        // label.highlightFunc = delegate { CreateEquipmentInfoPanel(statusLabel, effect); };
                        // label.unhighlightFunc = delegate { DestroyEquipmentInfoPanel(); };
                        break;
                    case EquipSlots.Armor:
                        armorLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format(item +  " ");
                        break;
                    case EquipSlots.Trinket:
                        trinketLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = string.Format(item +  " ");
                        break;

                }
            }
        }

        Status status = unit.GetComponent<Status>();
        // if(status){
            
        //     foreach(GameObject effect in status.statuses){
		//         GameObject statusLabel = Instantiate(statusLabelPrefab, statusHolder.transform);
        //         StatusLabel label = statusLabel.GetComponent<StatusLabel>();
        //         TMP_Text effectLabel = statusLabel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        //         TMP_Text durationLabel = statusLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

        //         effectLabel.text = string.Format( "{0} EFFECT", effect.GetComponent<StatusEffect>().statusName);
        //         if(effect.GetComponent<DurationStatusCondition>())
        //             durationLabel.text = string.Format( "{0} TURNS LEFT", effect.GetComponent<DurationStatusCondition>().duration);
                
        //         label.highlightFunc = delegate { CreateStatusInfoPanel(statusLabel, effect); };
        //         label.unhighlightFunc = delegate { DestroyStatusInfoPanel(); };
        //     }
        // }

    }

    public void ShowPanel(){
        detailsBG.SetActive(true);
    }
    public void HidePanel(){
        detailsBG.SetActive(false);
    }

    void CreateEquipmentInfoPanel(GameObject label, GameObject equipment){
        Debug.Log("creating weapon panel");
        Destroy(equipmentInfoPanel);
        Vector2 pos = label.transform.position;
        pos += new Vector2(0, 10);
        equipmentInfoPanel = Instantiate(equipmentInfoPanelPrefab, pos, Quaternion.identity, label.transform);
        equipmentInfoPanel.GetComponent<StatusInfoPanel>().Setup(equipment);
        equipmentInfoPanel.GetComponent<StatusInfoPanel>().ShowPanel();
    }
    void DestroyEquipmentInfoPanel(){
        if(equipmentInfoPanel && equipmentInfoPanel.GetComponent<StatusInfoPanel>())
            equipmentInfoPanel.GetComponent<StatusInfoPanel>().HidePanel();
        Destroy(equipmentInfoPanel);
    }
}