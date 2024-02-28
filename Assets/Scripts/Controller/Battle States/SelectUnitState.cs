using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitState : BattleState 
{
    public override void Enter (){
        base.Enter ();
        if (IsBattleOver()){
            Debug.Log("selecting says battle is over BEFORE roling over");
			owner.ChangeState<EndBattleState>();
        }
        else
            StartCoroutine("ChangeCurrentUnit");
    }
    public override void Exit (){
        base.Exit ();
        panelController.HidePrimary();
        panelController.HideStatus();
    }

    //logic for cycling thru and selecting the next acting unit
    IEnumerator ChangeCurrentUnit (){
        // if (IsBattleOver()){
        //     Debug.Log("selecting says battle is over AGAIN");
		// 	owner.ChangeState<EndBattleState>();
        // }
        owner.round.MoveNext();
        SelectTile(turn.actingUnit.tile.position);
		driver = (turn.actingUnit != null) ? turn.actingUnit.GetComponent<Driver>() : null;

        if (driver.Current == Drivers.Computer){
            // board.humanDriver = false;
            RefreshPrimaryStatusPanel(selectPos);
        }
        else{
            RefreshPrimaryPanel(selectPos);
            // board.humanDriver = true;
        }
        
        yield return null;
        if (IsBattleOver()){
            Debug.Log("selecting says battle is over PLEASE");
			owner.ChangeState<EndBattleState>();
        }
        else
            owner.ChangeState<CommandSelectionState>();
    }
}
