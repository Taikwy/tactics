using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCatalog : MonoBehaviour 
{
    //these get set in the unity factory, wil need ways to ref these ig
    public GameObject basicAbility, primarySkillAbility, secondarySkillAbility, traitAbility, skillAbility, burstAbility;
    public int CategoryCount (){
        return transform.childCount;
    }
    public GameObject GetCategory (int index){
        if (index < 0 || index >= transform.childCount)
            return null;
        return transform.GetChild(index).gameObject;
    }
    public int AbilityCount (GameObject category){
        return category != null ? category.transform.childCount : 0;
    }
    public Ability GetAbility (int categoryIndex, int abilityIndex){
        GameObject category = GetCategory(categoryIndex);
        if (category == null || abilityIndex < 0 || abilityIndex >= category.transform.childCount)
            return null;
        return category.transform.GetChild(abilityIndex).GetComponent<Ability>();
    } 
}