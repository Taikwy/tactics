using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : BattleState 
{
    public override void Enter (){
        base.Enter ();
        StartCoroutine("Sequence");

        // Debug.Log("moving and adding xp");
        turn.actingUnit.GainExperience(100);
    }
    
    // IEnumerator Sequence (){
    //     Movement m = owner.currentUnit.GetComponent<Movement>();
    //     Debug.Log(m);
    //     yield return StartCoroutine(m.Traverse(owner.currentTile));
    //     owner.ChangeState<SelectUnitState>();
    // }

    IEnumerator Sequence (){
        Movement m = turn.actingUnit.GetComponent<Movement>();
        // yield return StartCoroutine(m.Traverse(owner.currentTile));
        yield return StartCoroutine(m.Move(owner.selectedTile));
        turn.hasUnitMoved = true;
        owner.ChangeState<CommandSelectionState>();
    }
}