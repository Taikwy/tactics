using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExploreState : BattleState 
{

    public override void Enter (){
        base.Enter ();
        RefreshPrimaryPanel(selectPos);
    }
    public override void Exit (){
        base.Exit ();
        statPanelController.HidePrimary();
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
}