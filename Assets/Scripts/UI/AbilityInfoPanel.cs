using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityInfoPanel : MonoBehaviour 
{
    public GameObject panelBG;
    [Header("Ability Info")]
    public TMP_Text nameLabel;
    public TMP_Text effectLabel, conditionLabel;
    
    //takes in ability gameobject
    public void Display (GameObject ability)
    {
        Ability abilityScript = ability.GetComponent<Ability>();
        AbilityRange rangeScript = ability.GetComponent<AbilityRange>();
        AbilityArea areaScript = ability.GetComponent<AbilityArea>();
        if(!abilityScript || !rangeScript || !areaScript)  
            return;

        nameLabel.text = ability.name;
        effectLabel.text = abilityScript.primaryEffect.name;
        nameLabel.text = "nothin for now";
    }

    public void ShowPanel(){
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        panelBG.SetActive(false);
    }
}