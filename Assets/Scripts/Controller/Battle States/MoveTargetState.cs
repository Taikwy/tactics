using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetState : BattleState
{
    
    List<Tile> tiles, pathTiles, allyTiles, foeTiles = new List<Tile>();
    Movement moveScript;
    bool updating = false;
    
    public override void Enter (){
        // Debug.Log("enter moving state");
        base.Enter ();
        moveScript = turn.actingUnit.GetComponent<Movement>();
        tiles = moveScript.GetAllTilesInRange(board);
        allyTiles = moveScript.FilterAllies(tiles);
        foeTiles = moveScript.FilterFoes(tiles);

        
        moveScript.FilterOccupied(tiles);
        tiles.Add(turn.actingUnit.tile);

        board.HighlightTiles(tiles, Board.OverlayColor.MOVE);
        board.HighlightTiles(allyTiles, Board.OverlayColor.PASS);
        board.HighlightTiles(foeTiles, Board.OverlayColor.ATTACK);
        RefreshPrimaryPanel(selectPos);

        if (driver.Current == Drivers.Computer){
            board.humanDriver = false;
            updating = false;
			StartCoroutine(ComputerHighlightMoveTarget());
        }
        else{
            board.humanDriver = true;
            
            cameraRig.selectMovement = false;
            updating = true;
        }

        panelController.ShowMouseControls();
    }
    
    public override void Exit () {
        // Debug.Log("exiting moving state");
        updating = false;
        cameraRig.selectMovement = true;

        base.Exit ();
        board.UnhighlightTiles(tiles);
        board.UnhighlightTiles(allyTiles);
        board.UnhighlightTiles(foeTiles);
        board.UntargetTiles(pathTiles);
        tiles = null;
        // statPanelController.HidePrimary();
        panelController.HidePrimary();
        panelController.HideSecondary();
        panelController.HideMouseControls();
    } 

    protected void Update(){
        if(!updating)
            return;
        // Debug.Log("movestate updating");

        RefreshSecondaryPanel(board.selectedPoint);                 //highlights hovered unit
        // SelectTile(board.selectedPoint, tiles.Contains(board.selectedTile));
        if(tiles.Contains(board.selectedTile)){
            if(board.selectedTile.content != null){
                SelectTile(board.selectedPoint, Board.SelectColor.ENEMY);
            }
            else{
                SelectTile(board.selectedPoint, Board.SelectColor.VALID);
            }
        }else{
            SelectTile(board.selectedPoint, Board.SelectColor.EMPTY);
        }

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

    IEnumerator ComputerHighlightMoveTarget (){
		Point cursorPos = selectPos;
		while (cursorPos != turn.plan.moveLocation){
			if (cursorPos.x < turn.plan.moveLocation.x) cursorPos.x++;
			else if (cursorPos.x > turn.plan.moveLocation.x) cursorPos.x--;
			else if (cursorPos.y < turn.plan.moveLocation.y) cursorPos.y++;
			else if (cursorPos.y > turn.plan.moveLocation.y) cursorPos.y--;
            // Debug.Log("after moving cursor " + cursorPos);
			SelectTile(cursorPos);
            RefreshSecondaryPanel(board.selectedPoint);                 //highlights hovered unit
            TargetTiles();       
			yield return new WaitForSeconds(owner.actionDelays.moveSelectDelay);
		}
		yield return new WaitForSeconds(owner.actionDelays.moveFinishDelay);
		owner.ChangeState<MoveSequenceState>();
	}
}