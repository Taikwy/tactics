using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatusInfoPanel : MonoBehaviour 
{
    public GameObject panelBG;
    [Header("Status Info")]
    public TMP_Text nameLabel;
    public TMP_Text descLabel, effectLabel, durationLabel;
    public Color damageColor, debuffColor, buffColor;
    
    //takes in ability gameobject
    public void Setup (GameObject statusEffect){
        StatusEffect effectScript = statusEffect.GetComponent<StatusEffect>();
        DurationStatusCondition durationScript = statusEffect.GetComponent<DurationStatusCondition>();
        if(!effectScript || !durationScript)  {
            Debug.LogError("STATUS EFFECT missing scripts for info display");
            return;
        }

        nameLabel.text = string.Format("{0}", statusEffect.name);
        switch(statusEffect.name){
            default:
                Debug.LogError("uncrecognized effect name");
                nameLabel.color = Color.white;
                break;
            case "BREAK":
                nameLabel.color = debuffColor;
                break;
            case "CRUSH":
                nameLabel.color = debuffColor;
                break;
            case "BURN":
                nameLabel.color = damageColor;
                break;
            case "TOXIC":
                nameLabel.color = damageColor;
                break;
            case "SHOCK":
                nameLabel.color = damageColor;
                break;
            case "COLD":
                nameLabel.color = debuffColor;
                break;
            case "STRENGTHEN":
                nameLabel.color = buffColor;
                break;
            case "FORTIFY":
                nameLabel.color = buffColor;
                break;
            case "HASTE":
                nameLabel.color = buffColor;
                break;
            case "RESTORE":
                nameLabel.color = buffColor;
                break;
            case "HALT":
                nameLabel.color = debuffColor;
                break;
        }
        descLabel.text = effectScript.statusEffectDescription;
        // effectLabel.text = string.Format( "EFFECT: {0}", effectScript.statusName);
        durationLabel.text = string.Format( "{0} TURNS LEFT", durationScript.duration);
        // nameLabel.text = string.Format("{0}", ability.name);
        // descLabel.text = abilityScript.abilityDescription;
        // effectLabel.text = string.Format("EFFECT: {0}, ZONE: {1}", abilityScript.primaryEffect.name, abilityScript.primaryEffect.GetComponent<EffectZone>());
        // durationLabel.text = string.Format("RANGE TYPE: {0}, RANGE: {1}", rangeScript.name, rangeScript.range);
    }

    public void ShowPanel(){
        Debug.Log("showing status panel");
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        Debug.Log("hiding status panel");
        panelBG.SetActive(false);
    }
}