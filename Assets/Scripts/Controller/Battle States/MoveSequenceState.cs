using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : BattleState 
{
    public override void Enter (){
        base.Enter ();
        panelController.HideTimeline();
        StartCoroutine("Sequence");
    }
    public override void Exit()
    {
        base.Exit();
        
        panelController.ShowTimeline();
    }


    IEnumerator Sequence (){
        Movement m = turn.actingUnit.GetComponent<Movement>();
        // yield return StartCoroutine(m.Traverse(owner.currentTile));
        yield return StartCoroutine(m.Move(owner.selectedTile));
        turn.hasUnitMoved = true;
        owner.ChangeState<CommandSelectionState>();
    }
}