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
        tileSelectionIndicator.ChangeTarget();

        rangeScript = turn.selectedAbility.GetComponent<AbilityRange>();
        areaScript = turn.selectedAbility.GetComponent<AbilityArea>();
        zoneScript = turn.selectedAbility.primaryEffect.GetComponent<EffectZone>();

        areaScript.targets.Clear();
        SelectTile(turn.actingUnit.tile.position);
        highlightedTiles = rangeScript.FilterGround(rangeScript.GetTilesInRange(board));
        board.HighlightTiles(highlightedTiles, turn.selectedAbility.overlayColor);

        selectedTiles = new List<Tile>();
        
        panelController.ShowPrimary(turn.actingUnit.gameObject);
        panelController.ShowSecondary(turn.actingUnit.gameObject);
        if (rangeScript.directionOriented)
            RefreshSecondaryPanel(selectPos);
        if (driver.Current == Drivers.Computer){
			StartCoroutine(ComputerHighlightTarget());
            updating = false;
        }
        else{
            board.humanDriver = true;
            updating = true;
        }
    }
    public override void Exit (){
        updating = false;
        // Debug.Log("exitingggg");
        base.Exit ();
        tileSelectionIndicator.ChangeSelect();

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
        // SelectTile(board.selectedPoint);
        SelectTile(board.selectedPoint, highlightedTiles.Contains(board.selectedTile));
        TargetTiles();
    }

    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            //selected tile must be in currently highlighted area
            if(!highlightedTiles.Contains(owner.selectedTile)){
                return;
            }
            //only relevant for unit area, cuz full area will just auto return the full range
            if(areaScript.GetType() == typeof(UnitAbilityArea)){
                UnitAbilityArea unitArea = areaScript as UnitAbilityArea;
                if(unitArea.targets.Count < unitArea.numTargets){
                    unitArea.targets.Add(owner.selectedTile);
                }
                if(unitArea.targets.Count >= unitArea.numTargets){
                    owner.ChangeState<ConfirmAbilityTargetState>();
                    return;
                }
            }
            else{
                //this is gonna be if area script is full area, in which case selecting will auto proceed
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
                owner.ChangeState<AbilitySectionState>();
            return;
        }
        Debug.Log("post on fire");
        TargetTiles();
        SelectTiles();
    }

    //unused rn
    // void ChangeDirection (Point p){
    //     Directions dir = p.GetDirection();
    //     if (turn.actingUnit.dir != dir){
    //         board.UnhighlightTiles(highlightedTiles);
    //         turn.actingUnit.dir = dir;
    //         turn.actingUnit.Match();
    //         // HighlightTiles ();
    //         highlightedTiles = rangeScript.GetTilesInRange(board);
    //         board.HighlightTiles(highlightedTiles, Board.OverlayColor.ATTACK);
    //     }
    // }

    void TargetTiles (){
        // targetedTiles = rangeScript.GetTargetsInRange(board);
        if(targetedTiles != null)
            board.UntargetTiles(targetedTiles);

        //gets all the currently targeted tiles
        // List<Tile> centerTiles = areaScript.ShowTargetedTiles(board);
        List<Tile> centerTiles = new List<Tile>(areaScript.targets);
        if(!centerTiles.Contains(owner.selectedTile) && highlightedTiles.Contains(owner.selectedTile))    //this got changed when i made the select indicator change color targeting things out of range
            centerTiles.Add(owner.selectedTile);
        
        targetedTiles = new List<Tile>();
        //adds the zone like the splash zones of any targeted tiles
        foreach(Tile centerTile in centerTiles){
            targetedTiles.AddRange(zoneScript.ShowTilesInZone(board, centerTile.position));
        }
        // Debug.Log("targeting tiles " + targetedTiles.Count + " | center tiles " + centerTiles.Count);
        board.TargetTiles(targetedTiles, turn.selectedAbility.overlayColor);
    }

    void SelectTiles(){
        if(selectedTiles != null)
            board.UnselectTiles(selectedTiles);

        selectedTiles = new List<Tile>(areaScript.targets);
        // board.SelectTiles(selectedTiles);
        board.SelectTiles(selectedTiles, turn.selectedAbility.overlayColor);
    }

    IEnumerator ComputerHighlightTarget ()
	{
        //for direction orientated abilities
		// if (ar.directionOriented)
		// {
		// 	ChangeDirection(turn.plan.attackDirection.GetNormal());
		// 	yield return new WaitForSeconds(0.25f);
		// }
		// else
		// {
			Point cursorPos = selectPos;
            // Debug.Log("fire location " + turn.plan.fireLocation);
			while (cursorPos != turn.plan.fireLocation){
				if (cursorPos.x < turn.plan.fireLocation.x) cursorPos.x++;
				if (cursorPos.x > turn.plan.fireLocation.x) cursorPos.x--;
				if (cursorPos.y < turn.plan.fireLocation.y) cursorPos.y++;
				if (cursorPos.y > turn.plan.fireLocation.y) cursorPos.y--;
                // Debug.Log("after moving cursor for target " + cursorPos);
                RefreshSecondaryPanel(board.selectedPoint);
				SelectTile(cursorPos);
                TargetTiles();
				yield return new WaitForSeconds(owner.actionDelays.actionSelectDelay);
			}
		// }
				SelectTile(cursorPos);
                TargetTiles();
		yield return new WaitForSeconds(owner.actionDelays.actionFinishDelay);
				SelectTile(cursorPos);
                TargetTiles();
        if(highlightedTiles.Contains(owner.selectedTile)){
            areaScript.targets.Add(owner.selectedTile);
            // if(areaScript.targets.Count >= areaScript.numTargets){
                owner.ChangeState<ConfirmAbilityTargetState>();
            // }
        }
        
		// owner.ChangeState<ConfirmAbilityTargetState>();
	}
}