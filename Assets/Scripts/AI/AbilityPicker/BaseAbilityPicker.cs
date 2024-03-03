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
	[Header("Substitute stuff in case ability doesn't get performed")]
	[Tooltip("What ACTION to do if unable to perform")]public PlanOfAttack.SubAction subAction;
	[Tooltip("Where to MOVE if unable to perform")]public PlanOfAttack.SubMovement subMovement;
	[Tooltip("SKIP or STAY on this picker")]public ProceedType proceedType;
	
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
		plan.subAction = subAction;
		if(!plan.ability.CanPerform())
			plan.ability = null;
		
		Debug.Log("checking whether can perform " + plan.ability + " | " +  (plan.ability != null) + "         assigning sub action " + plan.subAction);
	}

	

}