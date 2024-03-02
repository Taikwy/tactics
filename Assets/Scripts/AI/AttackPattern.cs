using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackPattern : MonoBehaviour 
{
	public List<BaseAbilityPicker> pickers;
	int index;
	
	public void Pick (PlanOfAttack plan){
		pickers[index].Pick(plan);
	}
	public void IncrementPicker(){
		print("incrementing picker");
		index++;
		if (index >= pickers.Count)
			index = 0;
		
	}
	public void HandlePicker(PlanOfAttack plan){
		if(!plan.canPerformAbility && pickers[index].proceedType == BaseAbilityPicker.ProceedType.SKIP){
			IncrementPicker();
		}
		else{
			IncrementPicker();
		}
	}

}
