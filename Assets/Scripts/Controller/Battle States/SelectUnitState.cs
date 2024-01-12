using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitState : BattleState 
{
    public override void Enter (){
        base.Enter ();
        StartCoroutine("ChangeCurrentUnit");
    }
    public override void Exit (){
        base.Exit ();
        statPanelController.HidePrimary();
        panelController.HidePrimary();
    }

    //logic for cycling thru and selecting the next acting unit
    IEnumerator ChangeCurrentUnit (){
        owner.round.MoveNext();
        SelectTile(turn.actingUnit.tile.position);
        RefreshPrimaryPanel(selectPos);
        yield return null;
        owner.ChangeState<CommandSelectionState>();
    }
}
