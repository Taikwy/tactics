using UnityEngine;
using System.Collections;

public abstract class BaseAbilityPicker : MonoBehaviour
{
	protected Unit owner;
	protected AbilityCatalog abilityCatalog;
	void Start (){
		owner = GetComponentInParent<Unit>();
		abilityCatalog = owner.GetComponentInChildren<AbilityCatalog>();
	}
	public abstract void Pick (PlanOfAttack plan);
	protected Ability Find (string abilityName){
		for (int i = 0; i < abilityCatalog.transform.childCount; ++i){
			Transform category = abilityCatalog.transform.GetChild(i);
			Transform child = category.Find(abilityName);
			if (child != null)
				return child.GetComponent<Ability>();
		}
		return null;
	}

	protected Ability Default (){
		return owner.GetComponentInChildren<Ability>();
	}
}