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
    public TMP_Text effectLabel, conditionLabel, rangeLabel, areaLabel;
    
    //takes in ability gameobject
    public void Display (GameObject ability){
        Ability abilityScript = ability.GetComponent<Ability>();
        // EffectZone abilityZone = abilityScript.primaryEffect.GetComponent<EffectZone>();
        AbilityRange rangeScript = ability.GetComponent<AbilityRange>();
        AbilityArea areaScript = ability.GetComponent<AbilityArea>();
        BaseAbilityEffect effectScript = abilityScript.primaryEffect.GetComponent<BaseAbilityEffect>();

        if(!abilityScript || !rangeScript || !areaScript)  {
            Debug.LogError("Ability missing scripts for info display");
            return;
        }

        nameLabel.text = string.Format("{0}", ability.name);

        costLabel.text = "COST NOT FOUND";
        if(ability.GetComponent<AbilitySkillCost>()){
            if(ability.GetComponent<AbilitySkillCost>().cost <= 0){
                costLabel.text = string.Format("COST: NONE");
            }
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
        effectLabel.text = string.Format("EFFECT: {0}", abilityScript.primaryEffect.name);
        conditionLabel.text = "";
        if(effectScript.GetType() == typeof(InflictAbilityEffect))
            conditionLabel.text = effectScript.abilityEffectDescription;


        if(rangeScript.GetType() == typeof(ConstantAbilityRange)){
            rangeLabel.text = string.Format("RANGE: {0}", rangeScript.range);
        }  
        else if(rangeScript.GetType() == typeof(InfiniteAbilityRange)){
            rangeLabel.text = string.Format("RANGE: INFINITE"); 
        }
        else if(rangeScript.GetType() == typeof(SelfAbilityRange)){
            rangeLabel.text = string.Format("RANGE: SELF");
        }

        if(areaScript.GetType() == typeof(FullAbilityArea)){
            areaLabel.text = string.Format("AREA: FULL");
        }
        else if(areaScript.GetType() == typeof(UnitAbilityArea)){
            areaLabel.text = string.Format("AREA: UNIT");
        }
    }

    public void ShowPanel(){
        // Debug.Log("showing ability panel");
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        panelBG.SetActive(false);
    }
}