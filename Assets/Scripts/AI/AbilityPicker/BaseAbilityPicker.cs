using UnityEngine;
using System.Collections;

public abstract class BaseAbilityPicker : MonoBehaviour
{
	protected Unit owner;
	protected AbilityCatalog abilityCatalog;

	public enum ProceedType{
		SKIP, 
		STAY
	}
	// public enum SubAction{
	// 	BASIC,
	// 	FOCUS,
	// 	PASS
	// }
	public ProceedType proceedType;
	public PlanOfAttack.SubAction subAction;
	public PlanOfAttack.SubMovement subMovement;
	
	void Start (){
		owner = GetComponentInParent<Unit>();
		abilityCatalog = owner.GetComponentInChildren<AbilityCatalog>();
	}
	public abstract void Pick (PlanOfAttack plan);

	//gets the abilty script from catalog of unit
	protected Ability Find (string abilityName){
		for (int i = 0; i < abilityCatalog.transform.childCount; ++i){
			Transform catalog = abilityCatalog.transform;
			Transform child = catalog.Find(abilityName);
			if (child != null){
				print("ability " + child + " found!!!");
				return child.GetComponent<Ability>();
			}
		}
		return null;
	}
	//returns ANY ability
	// protected Ability Default (){
	// 	print("getting default ability");
	// 	return owner.GetComponentInChildren<Ability>();
	// }
	// protected Ability Basic (){
	// 	print("getting default ability");
	// 	return abilityCatalog.basicAbility.GetComponent<Ability>();;
	// }

	//sets canperform as whether the ability can be performed
	protected void CheckAbility(PlanOfAttack plan){
		if(plan.ability == null)
			return;
		Debug.Log("checking whether can perform " + plan.ability + " | " +  plan.ability.CanPerform());
		plan.canPerformAbility = plan.ability.CanPerform();
		plan.subAction = subAction;
		Debug.Log("assigning sub action " + plan.subAction);
	}

	

}