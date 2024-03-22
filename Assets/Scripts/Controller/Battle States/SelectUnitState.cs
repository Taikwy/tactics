using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitState : BattleState 
{
    public override void Enter (){
        base.Enter ();

        // Debug.LogError("ENTERING SELECT STATE");
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
        // panelController.HideStatus();
    }

    //logic for cycling thru and selecting the next acting unit
    IEnumerator ChangeCurrentUnit (){
        // print("starting change current unit again");
        // if (IsBattleOver()){
        //     Debug.Log("selecting says battle is over AGAIN");
		// 	owner.ChangeState<EndBattleState>();
        // }
        owner.round.MoveNext();
        // if(turn.actingUnit)
        
        SelectTile(turn.actingUnit.tile.position, Board.SelectColor.VALID);
		driver = (turn.actingUnit != null) ? turn.actingUnit.GetComponent<Driver>() : null;
        // yield return new WaitForSeconds(.05f);

        if (driver.Current == Drivers.Computer){
            // board.humanDriver = false;
            RefreshPrimaryStatusPanel(selectPos);
        }
        else{
            RefreshPrimaryPanel(selectPos);
            // board.humanDriver = true;
        }

        if (IsBattleOver()){
            Debug.Log("selecting says battle is over PLEASE");
			owner.ChangeState<EndBattleState>();
        }
        else if(!turn.actingUnit){
            // Debug.LogError("START COROUTEIN AGAIN");
            StartCoroutine("ChangeCurrentUnit");
        }
        else{
            print("changing to command from select!!! " + turn.actingUnit);
            IndicateActor(turn.actingUnit);
            owner.timeline.IndicateActor(turn.actingUnit);
        yield return new WaitForSeconds(.05f);
            yield return new WaitForSeconds(.25f);
            owner.ChangeState<CommandSelectionState>();
        }
        
        yield return null;
    }
}
