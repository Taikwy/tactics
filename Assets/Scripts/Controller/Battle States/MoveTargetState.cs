using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetState : BattleState
{
    List<Tile> tiles, pathTiles = new List<Tile>();
    Movement moveScript;
    bool updating = false;
    
    public override void Enter (){
        Debug.Log("enter moving state");
        base.Enter ();
        moveScript = turn.actingUnit.GetComponent<Movement>();
        tiles = moveScript.GetTilesInRange(board);
        // board.SelectTiles(tiles);
        tiles.Add(turn.actingUnit.tile);
        // board.HighlightMoveTiles(tiles);
        board.HighlightTiles(tiles, Board.OverlayColor.MOVE);
        RefreshPrimaryPanel(selectPos);

        updating = true;
    }
    
    public override void Exit () {
        Debug.Log("exiting moving state");
        updating = false;

        base.Exit ();
        board.UnhighlightTiles(tiles);
        board.UntargetTiles(pathTiles);
        tiles = null;
        // statPanelController.HidePrimary();
        panelController.HidePrimary();
    } 

    protected void Update(){
        if(!updating)
            return;
        // Debug.Log("movestate updating");

        SelectTile(board.selectedPoint);
        TargetTiles();
    }
    
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        SelectTile(e.info + selectPos);
        TargetTiles();
        RefreshPrimaryPanel(selectPos);
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
            Debug.Log("movement fire");
        if (e.info == 0){
            if (tiles.Contains(owner.selectedTile)){
                if(owner.selectedTile != turn.actingUnit.tile){
                    Debug.Log("moving");
                    owner.ChangeState<MoveSequenceState>();
                }
                else{
                    // Debug.Log("going back");
                    // owner.ChangeState<CommandSelectionState>();
                }
                // else
                //     owner.ChangeState<CommandSelectionState>();
            }
        }
        else{
            Debug.Log("going back");
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