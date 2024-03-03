using UnityEngine;
using System.Collections;

public class BurstAbilityPicker : FixedAbilityPicker
{
    GameObject burstAbility;
    void Awake(){
		burstAbility = owner.gameObject.GetComponentInChildren<AbilityCatalog>().burstAbility;
    }

    public bool CanBurst(){
        return false;

    }

}