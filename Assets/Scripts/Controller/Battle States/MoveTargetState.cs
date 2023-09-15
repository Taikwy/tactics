using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetState : BattleState
{
    List<Tile> tiles, pathTiles = new List<Tile>();
    Movement moveScript;
    
    public override void Enter (){
        base.Enter ();
        moveScript = turn.actingUnit.GetComponent<Movement>();
        tiles = moveScript.GetTilesInRange(board);
        // board.SelectTiles(tiles);
        tiles.Add(turn.actingUnit.tile);
        // board.HighlightMoveTiles(tiles);
        board.HighlightTiles(tiles, Board.OverlayColor.MOVE);
        RefreshBasePanel(selectPos);
    }
    
    public override void Exit () {
        base.Exit ();
        board.UnhighlightTiles(tiles);
        board.UntargetTiles(pathTiles);
        tiles = null;
        statPanelController.HidePrimary();
        panelController.HideBase();
    } 
    
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        SelectTile(e.info + selectPos);
        TargetTiles();
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
    void TargetTiles(){
        // targetedTiles = rangeScript.GetTargetsInRange(board);
        if(pathTiles != null)
            board.UntargetTiles(pathTiles);
         if (tiles.Contains(owner.selectedTile)){
            // pathTiles = new List<Tile>();
            // pathTiles.Add(board.GetTile(selectPos));
            // board.TargetTiles(pathTiles);

            pathTiles = moveScript.GetPath(owner.selectedTile);
            board.TargetTiles(pathTiles, Board.OverlayColor.MOVE);
         }
        
    }
}