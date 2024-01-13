using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargetState : BattleState 
{
    bool updating = false;
    List<Tile> highlightedTiles, targetedTiles, selectedTiles;
    AbilityRange rangeScript;
    AbilityArea areaScript;
    EffectZone zoneScript;
    
    public override void Enter (){
        base.Enter ();
        rangeScript = turn.selectedAbility.GetComponent<AbilityRange>();
        areaScript = turn.selectedAbility.GetComponent<AbilityArea>();
        zoneScript = turn.selectedAbility.primaryEffectZone;

        areaScript.targets.Clear();
        SelectTile(turn.actingUnit.tile.position);
        HighlightTiles ();
        selectedTiles = new List<Tile>();
        // TargetTiles();
        // SelectTiles();
        
        panelController.ShowPrimary(turn.actingUnit.gameObject);
        // RefreshSecondaryPanel(selectPos);
        panelController.ShowSecondary(turn.actingUnit.gameObject);
        if (rangeScript.directionOriented)
            RefreshSecondaryPanel(selectPos);
        
        updating = true;
    }
    public override void Exit (){
        updating = false;
        // Debug.Log("exitingggg");
        base.Exit ();
        board.UnhighlightTiles(highlightedTiles);
        board.UntargetTiles(targetedTiles);
        selectedTiles.Add(turn.actingUnit.tile);
        board.UnselectTiles(selectedTiles);
        panelController.HidePrimary();
        // statPanelController.HidePrimary();
        // statPanelController.HideSecondary();
    }

    protected void Update(){
        if(!updating || !highlightedTiles.Contains(board.selectedTile))
            return;
        // Debug.Log("abilitytarget updating");
        
        RefreshSecondaryPanel(board.selectedPoint);
        SelectTile(board.selectedPoint);
        TargetTiles();
    }

    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        if (rangeScript.directionOriented){
            ChangeDirection(e.info);
            SelectTile(e.info + turn.actingUnit.tile.position);
            areaScript.targets.Clear();
            TargetTiles();
        }
        else{
            //checks if selecticon is within range of ability or over self unit
            if(highlightedTiles.Contains(board.GetTile(e.info + selectPos)) || e.info + selectPos == turn.actingUnit.tile.position)
                SelectTile(e.info + selectPos);
        }
        TargetTiles();
        SelectTiles();

        
        if(true)                                    //Add an if statement so abilities that cannot harm yourself cannot target urself and urself cannot come up on the second panel
            RefreshSecondaryPanel(selectPos);
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            //selected tile must be in currently highlighted area
            if(!highlightedTiles.Contains(owner.selectedTile)){
                return;
            }
            areaScript.targets.Add(owner.selectedTile);
            if(areaScript.targets.Count >= areaScript.numTargets){
                owner.ChangeState<ConfirmAbilityTargetState>();
                return;
            }
        }
        else{
            if(areaScript.targets.Count > 0){
                Tile toRemove = areaScript.targets[areaScript.targets.Count-1];
                areaScript.targets.Remove(toRemove);
                SelectTile(toRemove.position);
            }
            else
                owner.ChangeState<CategorySelectionState>();
            return;
        }
        Debug.Log("post on fire");
        TargetTiles();
        SelectTiles();
    }
    void ChangeDirection (Point p){
        Directions dir = p.GetDirection();
        if (turn.actingUnit.dir != dir){
            board.UnhighlightTiles(highlightedTiles);
            turn.actingUnit.dir = dir;
            turn.actingUnit.Match();
            HighlightTiles ();
        }
    }
    
    void HighlightTiles (){
        highlightedTiles = rangeScript.GetTilesInRange(board);
        // board.SelectTiles(tiles);
        // board.HighlightAttackTiles(highlightedTiles);
        board.HighlightTiles(highlightedTiles, Board.OverlayColor.ATTACK);
    }
    void TargetTiles (){
        // targetedTiles = rangeScript.GetTargetsInRange(board);
        if(targetedTiles != null)
            board.UntargetTiles(targetedTiles);

        List<Tile> centerTiles = areaScript.ShowTargetedTiles(board);
        if(!centerTiles.Contains(owner.selectedTile))
            // centerTiles.Add(board.GetTile(selectPos));
            centerTiles.Add(owner.selectedTile);
        
        targetedTiles = new List<Tile>();
        foreach(Tile centerTile in centerTiles){
            targetedTiles.AddRange(zoneScript.ShowTilesInZone(board, centerTile.position));
        }
        // Debug.Log("targeting " + targetedTiles.Count);
        board.TargetTiles(targetedTiles, Board.OverlayColor.ATTACK);
    }
    void SelectTiles(){
        if(selectedTiles != null)
            board.UnselectTiles(selectedTiles);

        selectedTiles = new List<Tile>(areaScript.targets);
        // board.SelectTiles(selectedTiles);
        board.SelectTiles(selectedTiles, Board.OverlayColor.ATTACK);
    }
}