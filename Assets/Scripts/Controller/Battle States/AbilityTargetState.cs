using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargetState : BattleState 
{
    List<Tile> tiles;
    AbilityRange rangeScript;
    AbilityArea areaScript;
    
    public override void Enter (){
        base.Enter ();
        rangeScript = turn.selectedAbility.GetComponent<AbilityRange>();
        areaScript = turn.selectedAbility.GetComponent<AbilityArea>();
        HighlightTiles ();
        panelController.ShowBase(turn.actingUnit.gameObject);
        // statPanelController.ShowPrimary(turn.actingUnit.gameObject);
        if (rangeScript.directionOriented)
            RefreshSecondaryBasePanel(selectPos);
    }
    public override void Exit (){
        base.Exit ();
        board.UnhighlightTiles(tiles);
        board.UntargetTiles(tiles);
        panelController.HideBase();
        statPanelController.HidePrimary();
        statPanelController.HideSecondary();
    }
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        if (rangeScript.directionOriented){
            ChangeDirection(e.info);
            SelectTile(e.info + turn.actingUnit.tile.position);
            RefreshSecondaryBasePanel(selectPos);
        }
        else{
            SelectTile(e.info + selectPos);
            RefreshSecondaryBasePanel(selectPos);
        }
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            if (!areaScript.multipleTargets || tiles.Contains(board.GetTile(selectPos)))
                owner.ChangeState<ConfirmAbilityTargetState>();
            else if(areaScript.multipleTargets){
                if(areaScript.targets.Count >= areaScript.numTargets)
                    return;
                areaScript.targets.Add(board.GetTile(selectPos));
            }
        }
        else{
            owner.ChangeState<CategorySelectionState>();
        }
    }
    void ChangeDirection (Point p){
        Directions dir = p.GetDirection();
        if (turn.actingUnit.dir != dir){
            board.UntargetTiles(tiles);
            turn.actingUnit.dir = dir;
            turn.actingUnit.Match();
            TargetTiles ();
        }
        // if (turn.actingUnit.dir != dir){
        //     board.UnhighlightTiles(tiles);
        //     turn.actingUnit.dir = dir;
        //     turn.actingUnit.Match();
        //     HighlightTiles ();
        // }
    }
    void TargetTiles (){
        tiles = rangeScript.GetTargetsInRange(board);
        board.TargetTiles(tiles);
    }
    void HighlightTiles (){
        Debug.Log(rangeScript.gameObject);
        tiles = rangeScript.GetTilesInRange(board);
        // board.SelectTiles(tiles);
        board.HighlightAttackTiles(tiles);
    }
}