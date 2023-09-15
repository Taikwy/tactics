using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargetState : BattleState 
{
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
        TargetTiles();
        SelectTiles();
        
        panelController.ShowBase(turn.actingUnit.gameObject);
        // statPanelController.ShowPrimary(turn.actingUnit.gameObject);
        if (rangeScript.directionOriented)
            RefreshSecondaryBasePanel(selectPos);
    }
    public override void Exit (){
        base.Exit ();
        board.UnhighlightTiles(highlightedTiles);
        board.UntargetTiles(targetedTiles);
        board.UnselectTiles(selectedTiles);
        panelController.HideBase();
        statPanelController.HidePrimary();
        statPanelController.HideSecondary();
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
        // RefreshSecondaryBasePanel(selectPos);
        RefreshBasePanel(selectPos);
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            //selected tile must be in currently highlighted area
            if(!highlightedTiles.Contains(owner.selectedTile)){
                return;
            }
            areaScript.targets.Add(owner.selectedTile);
            if(areaScript.targets.Count >= areaScript.numTargets)
                owner.ChangeState<ConfirmAbilityTargetState>();
        }
        else{
            if(areaScript.targets.Count > 0){
                Tile toRemove = areaScript.targets[areaScript.targets.Count-1];
                areaScript.targets.Remove(toRemove);
                SelectTile(toRemove.position);
            }
            else
                owner.ChangeState<CategorySelectionState>();
        }
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