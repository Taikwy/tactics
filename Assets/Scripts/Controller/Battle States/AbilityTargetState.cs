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
        // zoneScript = turn.selectedAbility.primaryEffectZone;
        zoneScript = turn.selectedAbility.primaryEffect.GetComponent<EffectZone>();

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
        panelController.HideSecondary();
        // statPanelController.HidePrimary();
        // statPanelController.HideSecondary();
    }

    protected void Update(){
        if(!updating )     // || !highlightedTiles.Contains(board.selectedTile)
            return;
        // Debug.Log("abilitytarget updating");
        
        RefreshSecondaryPanel(board.selectedPoint);
        SelectTile(board.selectedPoint);
        SelectTile(board.selectedPoint, tiles.Contains(board.selectedTile));
        TargetTiles();
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
                owner.ChangeState<ActionSelectionState>();
            return;
        }
        Debug.Log("post on fire");
        TargetTiles();
        SelectTiles();
    }

    //unused rn
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

        //gets all the currently targeted tiles
        List<Tile> centerTiles = areaScript.ShowTargetedTiles(board);
        if(!centerTiles.Contains(owner.selectedTile))
            centerTiles.Add(owner.selectedTile);
        
        targetedTiles = new List<Tile>();
        //adds the zone like the splash zones of any targeted tiles
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