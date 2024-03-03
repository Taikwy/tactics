using UnityEngine;
using System.Collections;

public class BurstAbilityPicker : FixedAbilityPicker
{
    // GameObject burstAbility;
    // void Awake(){
	// 	burstAbility = owner.gameObject.GetComponentInChildren<AbilityCatalog>().burstAbility;
    // }

    //returns whether the unit is able to perform burst right now
    public bool CanBurst(){
        GameObject burstAbility = owner.gameObject.GetComponentInChildren<AbilityCatalog>().burstAbility;;
        if(burstAbility != null && burstAbility.GetComponent<AbilityBurstCost>() != null){
            if(owner.statsScript[StatTypes.BP] >= burstAbility.GetComponent<AbilityBurstCost>().cost){
                return true;
			}
        }
        return false;

    }

}