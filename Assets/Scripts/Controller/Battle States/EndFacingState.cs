using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndFacingState : BattleState 
{
    Directions startDir;
    public override void Enter (){
        base.Enter ();
        startDir = turn.actingUnit.dir;
        SelectTile(turn.actingUnit.tile.position);
        owner.ChangeState<SelectUnitState>();
    }
    
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        turn.actingUnit.dir = e.info.GetDirection();
        turn.actingUnit.Match();
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        switch (e.info){
            case 0:
                owner.ChangeState<SelectUnitState>();
                break;
            case 1:
                turn.actingUnit.dir = startDir;
                turn.actingUnit.Match();
                owner.ChangeState<CommandSelectionState>();
                break;
        }
    }
}