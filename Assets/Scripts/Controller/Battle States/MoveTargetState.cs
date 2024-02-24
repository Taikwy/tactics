using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetState : BattleState
{
    
    List<Tile> tiles, pathTiles, allyTiles, enemyTiles = new List<Tile>();
    Movement moveScript;
    bool updating = false;
    
    public override void Enter (){
        // Debug.Log("enter moving state");
        base.Enter ();
        moveScript = turn.actingUnit.GetComponent<Movement>();
        // tiles = moveScript.GetTilesInRange(board);
        tiles = moveScript.GetAllTilesInRange(board);
        allyTiles = moveScript.FilterAllies(tiles);
        enemyTiles = moveScript.FilterEnemies(tiles);

        
        moveScript.FilterOccupied(tiles);
        tiles.Add(turn.actingUnit.tile);

        // board.SelectTiles(tiles);
        // board.HighlightMoveTiles(tiles);
        board.HighlightTiles(tiles, Board.OverlayColor.MOVE);
        board.HighlightTiles(allyTiles, Board.OverlayColor.PASS);
        board.HighlightTiles(enemyTiles, Board.OverlayColor.ATTACK);
        Debug.Log(allyTiles.Count);
        Debug.Log(enemyTiles.Count);
        RefreshPrimaryPanel(selectPos);

        updating = true;
    }
    
    public override void Exit () {
        // Debug.Log("exiting moving state");
        updating = false;

        base.Exit ();
        board.UnhighlightTiles(tiles);
        board.UnhighlightTiles(allyTiles);
        board.UnhighlightTiles(enemyTiles);
        board.UntargetTiles(pathTiles);
        tiles = null;
        // statPanelController.HidePrimary();
        panelController.HidePrimary();
        panelController.HideSecondary();
    } 

    protected void Update(){
        if(!updating)
            return;
        // Debug.Log("movestate updating");

        RefreshSecondaryPanel(board.selectedPoint);                 //highlights hovered unit
        SelectTile(board.selectedPoint, tiles.Contains(board.selectedTile));
        TargetTiles();        
    }
    
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        Debug.LogError("movetstate onmove?");
        // SelectTile(e.info + selectPos);
        // TargetTiles();
        // RefreshPrimaryPanel(selectPos);
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
            // Debug.Log("movement fire");
        if (e.info == 0){
            if (tiles.Contains(owner.selectedTile)){
                if(owner.selectedTile != turn.actingUnit.tile){
                    // Debug.Log("moving");
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