using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExploreState : BattleState 
{
    List<Tile> tiles, allyTiles, foeTiles;
    bool updating = false;
    public override void Enter (){
        base.Enter ();
        tiles = allyTiles = foeTiles = new List<Tile>();
        RefreshPrimaryPanel(selectPos);

        cameraRig.selectMovement = false;
        updating = true;
    }
    public override void Exit (){
        updating = false;
        cameraRig.selectMovement = true;

        UnhighlightTiles();
        // UnhighlightTiles();

        base.Exit ();
        panelController.HidePrimary();
        audioManager.PlaySFX(owner.confirmSound);
    }
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        SelectTile(e.info + selectPos);
        RefreshPrimaryPanel(selectPos);
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        //exits back to acting unit, or selects the currently targeted unit to show status info
        if(!panelController.showingPrimaryStatus){
            //left click
            if (e.info == 0){
                
                UnhighlightTiles();
                //if current unit, goes back to command selection state
                if(board.selectedTile.content == owner.turn.actingUnit.gameObject){
                    owner.timeline.IndicateActor(turn.actingUnit);
                    owner.ChangeState<CommandSelectionState>();
                }
                //if not current unit, shows the unit's info
                else{
                    if(board.GetTile(board.selectedPoint).content != null){
                        Unit selectedUnit = board.GetTile(board.selectedPoint).content.GetComponent<Unit>();
                        if(selectedUnit != null){
                            HighlightMovement(selectedUnit);
                            cameraRig.selectMovement = true;
                            audioManager.PlaySFX(owner.confirmSound);
                        }
                    }
                    else
                        cameraRig.selectMovement = false;
                    
                    RefreshPrimaryStatusPanel(board.selectedPoint);
                }
            }
            if (e.info == 1){
                owner.timeline.IndicateActor(turn.actingUnit);
                owner.ChangeState<CommandSelectionState>();
            }
        }
        //if currently showing status info, right click goes back to default explore state
        else{
            panelController.HideStatus();
            UnhighlightTiles(); 
        }
        
    }

    void HighlightMovement(Unit selectedUnit){
        Movement moveScript = selectedUnit.moveScript;
        tiles = moveScript.GetAllTilesInRange(board);
        allyTiles = moveScript.FilterAllies(tiles);
        foeTiles = moveScript.FilterFoes(tiles);
        
        moveScript.FilterOccupied(tiles);
        tiles.Add(board.GetTile(board.selectedPoint));

        HighlightTiles();
    }

    protected void Update(){
        if(!updating)
            return;
        //only updates primary panel if in default explore state and not currently showing statuses
        if(!panelController.showingPrimaryStatus){
            RefreshPrimaryPanel(board.selectedPoint);
            SelectTile(board.selectedPoint);

            if(board.GetTile(board.selectedPoint).content != null){
                Unit selectedUnit = board.GetTile(board.selectedPoint).content.GetComponent<Unit>();
                if(selectedUnit != null){
                    HighlightMovement(selectedUnit);
                    owner.timeline.IndicateActor(selectedUnit);
                }
                else{
                    owner.timeline.IndicateActor(turn.actingUnit);
                    UnhighlightTiles();
                }
            }
            else{
                // owner.timeline.IndicateActor(turn.actingUnit);
                owner.timeline.StopIndicate();
                UnhighlightTiles();
            }
        }
    }

    void HighlightTiles(){
        board.HighlightTiles(tiles, Board.OverlayColor.MOVE);
        board.HighlightTiles(allyTiles, Board.OverlayColor.PASS);
        board.HighlightTiles(foeTiles, Board.OverlayColor.ATTACK);
    }
    void UnhighlightTiles(){
        if(tiles.Count > 0)
            board.UnhighlightTiles(tiles);
        if(allyTiles.Count > 0)
            board.UnhighlightTiles(allyTiles);
        if(foeTiles.Count > 0)
            board.UnhighlightTiles(foeTiles);
    }
}