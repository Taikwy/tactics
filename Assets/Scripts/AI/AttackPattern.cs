using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackPattern : MonoBehaviour 
{
	public List<BaseAbilityPicker> pickers;
	public BurstAbilityPicker burstPicker;
	[HideInInspector]public bool burstTargets;
	int index;
	
	public void Pick (PlanOfAttack plan){
		if(burstPicker && burstPicker.CanBurst() && burstTargets){
			plan.bursting = true;
			burstPicker.Pick(plan);
		}
		else{
			pickers[index].Pick(plan);
		}

	}
	public void IncrementPicker(){
		// print("incrementing picker");
		index++;
		if (index >= pickers.Count)
			index = 0;
		
	}
	public void UpdatePicker(PlanOfAttack plan){
		// print("handling picker " + (plan.ability == null) + " | " + pickers[index].proceedType);
		if(plan.ability == null || plan.bursting){
			//incrememnt picker if picker skips when unable to perform, OR if there are no valid targets so it can't possibly stay
			if(pickers[index].proceedType == BaseAbilityPicker.ProceedType.SKIP || !plan.validTargetsLeft)
				IncrementPicker();
		}
		else{
			IncrementPicker();
		}
	}
	public void HandleBurst(){
		

	}

}
