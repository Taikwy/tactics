using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class MoveTargetState : BattleState
{
    
    List<Tile> tiles, pathTiles, allyTiles, foeTiles = new List<Tile>();
    Movement moveScript;
    bool updating = false;
    Tile currentlyHoveredTile;
    // List<Tile> currentPathTiles;
    
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

        IndicateTimeline(board.selectedTile);
        if(currentlyHoveredTile != owner.selectedTile){
            TargetTiles();        
            currentlyHoveredTile = owner.selectedTile;
        }
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
                    audioManager.PlaySFX(owner.confirmSound);
                    owner.ChangeState<MoveSequenceState>();
                }
                else{
                    // Debug.Log("going back");
                    // owner.ChangeState<CommandSelectionState>();
                }
                // else
                //     owner.ChangeState<CommandSelectionState>();
            }
            else{
                audioManager.PlaySFX(owner.invalidSound);
            }
        }
        else{
            // Debug.Log("going back");
            owner.ChangeState<CommandSelectionState>();
            audioManager.PlaySFX(owner.cancelSound);
        }
    }
    void TargetTiles(){
        // print("targeting");
        // if(pathTiles != null){
        // }
        if (tiles.Contains(owner.selectedTile)){
            // pathTiles = new List<Tile>();
            // pathTiles.Add(board.GetTile(selectPos));
            // board.TargetTiles(pathTiles);
            List<Tile> tempTiles = moveScript.GetPath(owner.selectedTile);
            if(pathTiles != null && CompareTiles(pathTiles, tempTiles))
                return;
            board.UntargetTiles(pathTiles);
            pathTiles = tempTiles;
            board.TargetTiles(pathTiles, Board.OverlayColor.MOVE);
        }
        else{
            board.UntargetTiles(pathTiles);
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