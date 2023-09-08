using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetState : BattleState
{
    List<Tile> tiles;
    
    public override void Enter (){
        base.Enter ();
        Movement moveScript = turn.actingUnit.GetComponent<Movement>();
        tiles = moveScript.GetTilesInRange(board);
        // board.SelectTiles(tiles);
        tiles.Add(turn.actingUnit.tile);
        board.HighlightMoveTiles(tiles);
        RefreshBasePanel(selectPos);
    }
    
    public override void Exit () {
        base.Exit ();
        board.UnhighlightTiles(tiles);
        tiles = null;
        statPanelController.HidePrimary();
        panelController.HideBase();
    } 
    
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        SelectTile(e.info + selectPos);
        RefreshBasePanel(selectPos);
    }
    
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            if (tiles.Contains(owner.selectedTile)){
                // if(owner.selectedTile != turn.actingUnit.tile)
                    owner.ChangeState<MoveSequenceState>();
                // else
                //     owner.ChangeState<CommandSelectionState>();
            }
        }
        else{
            // Debug.Log("going back");
            owner.ChangeState<CommandSelectionState>();
        }
    }
}