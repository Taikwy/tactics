using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSelectionState : BaseAbilityMenuState
{
    bool updating = false;
    public override void Enter (){
        // Debug.Log("enter command selection  state");
        base.Enter ();
        panelController.ShowStatus(turn.actingUnit.gameObject);
        // panelController.ShowPrimary(turn.actingUnit.gameObject);
        SelectTile(turn.actingUnit.tile.position);

        updating = true;

    }
    public override void Exit (){
        // Debug.Log("exiting command selection state");
        updating = false;

        base.Exit ();
        // panelController.HidePrimary();
        panelController.HideStatus();
    }
    protected void Update(){
        if(!updating)
            return;
        // Debug.Log("commandstate updating");

        // SelectTile(owner.turn.actingUnit.tile.position);
        // SelectTile(turn.actingUnit.tile.position);
    }

    protected override void LoadMenu () {
        // Debug.Log("loading default commands");
        if (menuOptions == null){
            menuOptions = new List<string>(4){
                "MOVE",
                "ACTION",
                // "STATUS",
                "DEFEND",
                "PASS"
            };
            menuFunctions = new List<UnityEngine.Events.UnityAction>(4){
                delegate { Move(); },
                delegate { Act(); },
                // delegate { Status(); },
                delegate { Defend(); },
                delegate { Pass(); }
            };
        }
        
        // List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, altMenuFunctions);
        List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, menuFunctions);
        

        //logic for setting stuff as locked depending on actions the playe rhas taken
        abilityPanelController.SetLocked(0, turn.hasUnitMoved || turn.hasUnitActed);                             //disable movement if already moved or acted
        abilityPanelController.SetLocked(1, turn.hasUnitActed);                             //disable action if already acted
        // abilityPanelController.SetLocked(2, turn.hasUnitActed);                             //disable action if already acted
        // abilityPanelController.SetLocked(2, turn.hasUnitActed);                             

    }

    //for when a button is clicked
    protected void Move(){
        // Debug.Log("move clicked! " + owner);
        owner.ChangeState<MoveTargetState>();
    }protected void Act(){
        // Debug.Log("act clicked!");
        owner.ChangeState<CategorySelectionState>();
    }protected void Status(){
        // Debug.Log("status clicked!");
        owner.ChangeState<SelectUnitState>();
    }protected void Defend(){
        // Debug.Log("defend clicked!");
        owner.ChangeState<SelectUnitState>();
    }protected void Pass(){
        // Debug.Log("pass clicked!");
        owner.ChangeState<SelectUnitState>();
    }


    //goes to the previous state. pressing cancel or RMB triggers this
    protected override void Cancel (){
        if (turn.hasUnitMoved && !turn.lockMove){
            turn.UndoMove();
            abilityPanelController.SetLocked(0, false);
            SelectTile(turn.actingUnit.tile.position);
        }
        else{
            owner.ChangeState<ExploreState>();
        }
    }
    
}
