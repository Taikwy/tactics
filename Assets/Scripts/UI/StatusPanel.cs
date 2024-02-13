using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusPanel : MonoBehaviour 
{
    public GameObject statusBG;
    public GameObject statusLabelPrefab, equipmentLabelPrefab;
    [Space(2)][Header("Unit stuff")]
    public Image background;
    public Image portrait;
    public Sprite unitBackground;
    [Space(2)][Header("Unit Stats")]
    public TMP_Text nameLabel;
    public TMP_Text lvLabel, xpLabel, hpLabel, bpLabel;
    public TMP_Text skpLabel, atLabel, dfLabel, spLabel, cpLabel, cdLabel;
    public TMP_Text weaponLabel, armorLabel, trinketLabel;
    public GameObject statusHolder, equipmentHolder;
    
    //takes in unit gaemeobject
    public void Display (GameObject unit)
    {
        // Temp until I add a component to determine unit alliances
        // background.sprite = Random.value > 0.5f? enemyBackground : unitBackground;
        
        // portrait.sprite = null; Need a component which provides this data
        nameLabel.text = unit.GetComponent<Unit>().name;
        portrait.sprite = unit.GetComponent<Unit>().portrait;
        portrait.color = unit.GetComponent<Unit>().portraitColor;
        Stats stats = unit.GetComponent<Stats>();
        Equipment equipment = unit.GetComponent<Equipment>();
        if (stats) {
            lvLabel.text = string.Format( "LV. {0}", stats[StatTypes.LV]);
            xpLabel.text = string.Format( "XP. {0} / {1}", stats.GetCurrentXP(), unit.GetComponent<UnitLevel>().xpData.experiencePerLevel[stats[StatTypes.LV]]);
            hpLabel.text = string.Format( "HEALTH {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP] );
            bpLabel.text = string.Format( "BURST {0} / {1}", stats[StatTypes.BP], stats[StatTypes.MBP] );
            skpLabel.text = string.Format( "SKILL PTS {0} / {1}", stats[StatTypes.SKP], stats[StatTypes.MSKP] );

            atLabel.text = string.Format( "ATTACK {0}", stats[StatTypes.AT]);
            dfLabel.text = string.Format( "DEFENSE {0}", stats[StatTypes.DF]);
            spLabel.text = string.Format( "SPEED {0}", stats[StatTypes.SP]);
            cpLabel.text = string.Format( "CRIT% {0}", stats[StatTypes.CP]);
            cdLabel.text = string.Format( "CRITDMG {0}", stats[StatTypes.CD]);
        }
        if(equipment){
            foreach(Equippable item in equipment.equippedItems){
                GameObject equipmentLabel = Instantiate(equipmentLabelPrefab, equipmentHolder.transform);
                TMP_Text slotLabel = equipmentLabel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
                TMP_Text itemLabel = equipmentLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

                slotLabel.text = string.Format(item.equippedSlot + " slot: ");
                itemLabel.text = string.Format(item +  " ");
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
                TMP_Text effectLabel = statusLabel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
                TMP_Text durationLabel = statusLabel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

                effectLabel.text = string.Format( "{0} EFFECT", effect.GetComponent<StatusEffect>().statusName);
                if(effect.GetComponent<DurationStatusCondition>())
                    durationLabel.text = string.Format( "{0} TURNS LEFT", effect.GetComponent<DurationStatusCondition>().duration);
            }
        }
    }

    public void ShowStatus(){
        statusBG.SetActive(true);
    }
    public void HideStatus(){
        statusBG.SetActive(false);
    }
}