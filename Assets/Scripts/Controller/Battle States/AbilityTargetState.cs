using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargetState : BattleState 
{
    List<Tile> tiles;
    AbilityRange rangeScript;
    public override void Enter (){
        base.Enter ();
        rangeScript = turn.selectedAbility.GetComponent<AbilityRange>();
        SelectTiles ();
        panelController.ShowBase(turn.actingUnit.gameObject);
        statPanelController.ShowPrimary(turn.actingUnit.gameObject);
        if (rangeScript.directionOriented)
            RefreshSecondaryStatPanel(selectPos);
    }
    public override void Exit (){
        base.Exit ();
        board.UnhighlightTiles(tiles);
        panelController.HideBase();
        statPanelController.HidePrimary();
        statPanelController.HideSecondary();
    }
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        if (rangeScript.directionOriented){
            ChangeDirection(e.info);
        }
        else{
            SelectTile(e.info + selectPos);
            RefreshSecondaryStatPanel(selectPos);
        }
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            if (rangeScript.directionOriented || tiles.Contains(board.GetTile(selectPos)))
                owner.ChangeState<ConfirmAbilityTargetState>();
        }
        else{
            owner.ChangeState<CategorySelectionState>();
        }
    }
    void ChangeDirection (Point p){
        Directions dir = p.GetDirection();
        if (turn.actingUnit.dir != dir){
            board.UnhighlightTiles(tiles);
            turn.actingUnit.dir = dir;
            turn.actingUnit.Match();
            SelectTiles ();
        }
    }
    void SelectTiles (){
        Debug.Log(rangeScript.gameObject);
        tiles = rangeScript.GetTilesInRange(board);
        // board.SelectTiles(tiles);
        board.HighlightAttackTiles(tiles);
    }
}