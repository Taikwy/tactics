using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExploreState : BattleState 
{
    bool updating = false;
    public override void Enter (){
        base.Enter ();
        RefreshPrimaryPanel(selectPos);

        updating = true;
    }
    public override void Exit (){
        updating = false;

        base.Exit ();
        // statPanelController.HidePrimary();
        panelController.HidePrimary();
    }
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        SelectTile(e.info + selectPos);
        RefreshPrimaryPanel(selectPos);
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0)
            owner.ChangeState<CommandSelectionState>();
    }

    protected void Update(){
        if(!updating)
            return;
        // Debug.Log("explore updating");
        
        SelectTile(board.selectedPoint);
    }
}