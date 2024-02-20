using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAbilityState : BattleState 
{
    public override void Enter (){
        base.Enter ();
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        StartCoroutine(Animate());
    }
    
    IEnumerator Animate (){
        yield return null;
		ApplyAbility();
		
		if (IsBattleOver()){
            Debug.Log("changing to end");
			owner.ChangeState<EndBattleState>();

        }
		// else if (!UnitHasDied())
		// 	owner.ChangeState<SelectUnitState>();
		else{
			// owner.ChangeState<CommandSelectionState>();
            owner.ChangeState<SelectUnitState>();
        }
    }

    bool UnitHasDied (){
        return turn.actingUnit.GetComponentInChildren<DeadStatusEffect>() == null;
    }

    void ApplyAbility (){
		turn.selectedAbility.Perform(turn.targets);
	}
	
	// bool UnitHasControl ()
	// {
	// 	return turn.actingUnit.GetComponentInChildren<KnockOutStatusEffect>() == null;
	// }

}