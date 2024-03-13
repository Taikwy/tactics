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
        SelectTile(turn.actingUnit.tile.position, Board.SelectColor.VALID);
		driver = (turn.actingUnit != null) ? turn.actingUnit.GetComponent<Driver>() : null;

        IndicateActor(turn.actingUnit);



        if (driver.Current == Drivers.Computer){
            // board.humanDriver = false;
            RefreshPrimaryStatusPanel(selectPos);
        }
        else{
            RefreshPrimaryPanel(selectPos);
            // board.humanDriver = true;
        }
        yield return new WaitForSeconds(.1f);
        cameraRig.selectMovement = true;
        cameraRig.unitMovement = false;
        yield return new WaitForSeconds(.4f);
        yield return null;
        if (IsBattleOver()){
            Debug.Log("selecting says battle is over PLEASE");
			owner.ChangeState<EndBattleState>();
        }
        else
            owner.ChangeState<CommandSelectionState>();
    }
}
