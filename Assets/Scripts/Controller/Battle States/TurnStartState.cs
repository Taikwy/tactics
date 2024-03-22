using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnStartState : BattleState
{
    bool updating = false;
    public override void Enter (){
        Debug.Log("enter turn start state");
        base.Enter ();
        SelectTile(turn.actingUnit.tile.position);

        if (driver.Current == Drivers.Computer){
            panelController.ShowPrimary(turn.actingUnit.gameObject);
            board.humanDriver = false;
			StartCoroutine( ComputerTurn() );
        }
        else{
            // panelController.ShowStatus(turn.actingUnit.gameObject);
            panelController.ShowPrimary(turn.actingUnit.gameObject);
            board.humanDriver = true;
        }

        updating = true;
        panelController.ShowMouseControls();
    }
    public override void Exit (){
        // Debug.Log("exiting command selection state");
        updating = false;

        base.Exit ();
        panelController.HidePrimary();
        // panelController.HideStatus();
        panelController.HideMouseControls();
    }
    IEnumerator ComputerTurn ()
	{
		if (turn.plan == null){
			turn.plan = owner.cpu.Evaluate();
			turn.selectedAbility = turn.plan.ability;
		}
return null;
			owner.ChangeState<SelectUnitState>();
	}

    
}
