using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExploreState : BattleState 
{

    public override void Enter (){
        base.Enter ();
        RefreshBasePanel(pos);
    }
    public override void Exit (){
        base.Exit ();
        statPanelController.HidePrimary();
        panelController.HideBase();
    }
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        SelectTile(e.info + pos);
        RefreshBasePanel(pos);
    }
    
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0)
            owner.ChangeState<CommandSelectionState>();
    }
}