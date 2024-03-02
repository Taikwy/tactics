using UnityEngine;
using System.Collections;

public class FixedAbilityPicker : BaseAbilityPicker
{
	[Header("Ability Info")]
	public string ability;
	public Targets target;

	public override void Pick (PlanOfAttack plan){
		plan.target = target;
		plan.ability = Find(ability);

		if (plan.ability == null){
			Debug.LogError("couldn't find ability " + ability);
		}
		CheckAbility(plan);
		
		plan.subMovement = subMovement;
	}
}