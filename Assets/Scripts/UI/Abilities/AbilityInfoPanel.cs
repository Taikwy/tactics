using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq.Expressions;

public class AbilityInfoPanel : MonoBehaviour 
{
    public GameObject panelBG;
    [Header("Ability Info")]
    public TMP_Text nameLabel;
    public TMP_Text costLabel, descLabel;
    public TMP_Text effectLabel, rangeLabel, areaLabel;
    
    //takes in ability gameobject
    public void Display (GameObject ability){
        Ability abilityScript = ability.GetComponent<Ability>();
        // EffectZone abilityZone = abilityScript.primaryEffect.GetComponent<EffectZone>();
        AbilityRange rangeScript = ability.GetComponent<AbilityRange>();
        AbilityArea areaScript = ability.GetComponent<AbilityArea>();
        if(!abilityScript || !rangeScript || !areaScript)  {
            Debug.LogError("Ability missing scripts for info display");
            return;
        }

        nameLabel.text = string.Format("{0}", ability.name);

        costLabel.text = "COST NOT FOUND";
        if(ability.GetComponent<AbilitySkillCost>()){
            if(ability.GetComponent<AbilitySkillCost>().cost <= 0)
                costLabel.text = string.Format("NO COST");
            else
                costLabel.text = string.Format("SKILL POINTS REQUIRED: {0}", ability.GetComponent<AbilitySkillCost>().cost);
        }
        if(ability.GetComponent<AbilityBurstCost>()){
            if(ability.GetComponent<AbilityBurstCost>().cost < 0){
                Debug.Log("should be max " + GetComponentInParent<Stats>());
                costLabel.text = string.Format("BURST METER REQUIRED: {0}", ability.GetComponentInParent<Stats>()[StatTypes.MBP]);
                Debug.Log("should be max");
            }
            else
                costLabel.text = string.Format("BURST METER REQUIRED: {0}", ability.GetComponent<AbilityBurstCost>().cost);
        }

        descLabel.text = abilityScript.abilityDescription;
        effectLabel.text = string.Format("EFFECT: {0}, ZONE: {1}", abilityScript.primaryEffect.name, abilityScript.primaryEffect.GetComponent<EffectZone>());
        rangeLabel.text = string.Format("RANGE TYPE: {0}, RANGE: {1}", rangeScript.name, rangeScript.range);
        // areaLabel.text = string.Format("AREA: {0}, NUM TARGETS: under testing rn", areaScript.name, areaScript.numTargets);
    }

    public void ShowPanel(){
        // Debug.Log("showing ability panel");
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        panelBG.SetActive(false);
    }
}