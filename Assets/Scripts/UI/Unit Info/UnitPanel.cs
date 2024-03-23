using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitPanel : MonoBehaviour 
{
    public GameObject panelBG;
    [Header("Unit stuff")]
    public Image background;
    public Image portrait;
    public Sprite unitBackground;
    [Header("Unit Stats")]
    public TMP_Text nameLabel;
    public TMP_Text hpLabel, bpLabel;
    public Slider hpSlider, bpSlider;
    public SkillIcons skillIcons;
    [Header("Ailments")]
    public GameObject statusHolder;
    public GameObject statusIconPrefab;
    public GameObject statusInfoPanelPrefab;
    GameObject statusInfoPanel;
    
    [Space(15)][Header("Unused for now, this shit goes to status panel later")]
    public TMP_Text lvLabel;
    public TMP_Text xpLabel, mvLabel;
    public TMP_Text skpLabel, atLabel, dfLabel, spLabel, cpLabel, cdLabel;
    
    //takes in unit gaemeobject
    public void Display (GameObject unit){
        // Temp until I add a component to determine unit alliances
        // background.sprite = Random.value > 0.5f? enemyBackground : unitBackground;
        
        // portrait.sprite = null; Need a component which provides this data
        nameLabel.text = unit.GetComponent<Unit>().name.ToUpper();
        nameLabel.text = unit.GetComponent<Unit>().name;
        nameLabel.color = unit.GetComponent<Unit>().unitColor;


        portrait.sprite = unit.GetComponent<Unit>().portrait;
        // portrait.color = unit.GetComponent<Unit>().portraitColor;
        Stats stats = unit.GetComponent<Stats>();
        if (stats) {
            // lvLabel.text = string.Format( "LV. {0}", stats[StatTypes.LV]);
            // xpLabel.text = string.Format( "XP. {0} / {1}", stats.GetCurrentXP(), unit.GetComponent<UnitLevel>().xpData.experiencePerLevel[stats[StatTypes.LV]]);
            // if(unit.GetComponent<Unit>().moveScript.GetType() == typeof(WalkMovement)){
            //      mvLabel.text = "WALKING";
            // }
            // if(unit.GetComponent<Unit>().moveScript.GetType() == typeof(FlyMovement)){
            //      mvLabel.text = "FLYING";
            // }
            // if(unit.GetComponent<Unit>().moveScript.GetType() == typeof(TeleportMovement)){
            //      mvLabel.text = "TELEPORTING";
            // }
            hpSlider.maxValue = stats[StatTypes.MHP];
            hpSlider.value = stats[StatTypes.HP];
            bpSlider.maxValue = stats[StatTypes.MBP];
            bpSlider.value = stats[StatTypes.BP];

            hpLabel.text = string.Format( "{0}/{1}", stats[StatTypes.HP], stats[StatTypes.MHP] );
            bpLabel.text = string.Format( "{0}/{1}", stats[StatTypes.BP], stats[StatTypes.MBP] );
            if(stats[StatTypes.MBP] <= 0)
                bpLabel.text = "\\\\\\";

            skillIcons.FillIcons(stats[StatTypes.SK]);
            skillIcons.FillBgs(stats[StatTypes.MSK]);
            // skpLabel.text = string.Format( "SKILL PTS {0} / {1}", stats[StatTypes.SK], stats[StatTypes.MSK] );
            // atLabel.text = string.Format( "ATTACK {0}", stats[StatTypes.AT]);
            // dfLabel.text = string.Format( "DEFENSE {0}", stats[StatTypes.DF]);
            // spLabel.text = string.Format( "SPEED {0}", stats[StatTypes.SP]);
            // cpLabel.text = string.Format( "CRIT% {0}", stats[StatTypes.CR]);
            // cdLabel.text = string.Format( "CRITDMG {0}", stats[StatTypes.CD]);
        }
        foreach(Transform icon in statusHolder.transform){
            Destroy(icon.gameObject);
        }
        Status status = unit.GetComponent<Status>();
        if(status){
            int numIcons = 0;
            // Debug.LogError("num statusses " + status.statuses.Count + " | " + unit);
            foreach(GameObject effect in status.statuses){
                //cuz i only have space to show 6 ailmens on the unit panel lmao
                if(numIcons >= 6)
                    break;
                GameObject icon = Instantiate(statusIconPrefab, statusHolder.transform);
                StatusIcon statusIcon = icon.GetComponent<StatusIcon>();
                if(effect.GetComponent<DurationStatusCondition>())
                    statusIcon.turns.text = effect.GetComponent<DurationStatusCondition>().duration.ToString();
                statusIcon.Setup(effect.GetComponent<StatusEffect>().statusName);
                
                statusIcon.highlightFunc = delegate { CreateStatusInfoPanel(icon, effect); };
                statusIcon.unhighlightFunc = delegate { DestroyStatusInfoPanel(); };
                numIcons++;
            }
        }
    }

    void CreateStatusInfoPanel(GameObject status, GameObject effect){
        Debug.Log("creating status panel");
        Destroy(statusInfoPanel);
        Vector2 localPos = new Vector2(-42, 10);
        statusInfoPanel = Instantiate(statusInfoPanelPrefab, localPos, Quaternion.identity, status.transform);
        statusInfoPanel.transform.localPosition = localPos;
        statusInfoPanel.GetComponent<StatusInfoPanel>().Setup(effect);
        statusInfoPanel.GetComponent<StatusInfoPanel>().ShowPanel();
    }
    void DestroyStatusInfoPanel(){
        if(statusInfoPanel && statusInfoPanel.GetComponent<StatusInfoPanel>())
            statusInfoPanel.GetComponent<StatusInfoPanel>().HidePanel();
        Destroy(statusInfoPanel);
    }

    public void ShowPanel(){
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        panelBG.SetActive(false);
    }
}