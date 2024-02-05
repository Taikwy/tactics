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
        panelController.HidePrimary();
    }
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        SelectTile(e.info + selectPos);
        RefreshPrimaryPanel(selectPos);
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        //exits back to acting unit, or selects the currently targeted unit to show status info
        if(!panelController.showingPrimaryStatus){
            if (e.info == 0){
                RefreshPrimaryStatusPanel(board.selectedPoint);
            }
            if (e.info == 1){
                owner.ChangeState<CommandSelectionState>();
            }
        }
        //if currently showing status info, right click goes back to default explore state
        else{
            if (e.info == 1){
                panelController.HideStatus();
            }
        }
        
    }

    protected void Update(){
        if(!updating)
            return;
        //only updates primary panel if in default explore state and not currently showing statuses
        if(!panelController.showingPrimaryStatus){
            Debug.Log("explore updating");
            RefreshPrimaryPanel(board.selectedPoint);
            SelectTile(board.selectedPoint);
        }
    }
}