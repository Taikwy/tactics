using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSelectionState : BaseAbilityMenuState
{
    public override void Enter (){
        Debug.Log("enter command selection  state");
        base.Enter ();
        statPanelController.ShowPrimary(turn.actingUnit.gameObject);
        panelController.ShowPrimary(turn.actingUnit.gameObject);

        
        SelectTile(turn.actingUnit.tile.position);
    }
    public override void Exit (){
        Debug.Log("exiting command selection state");
        base.Exit ();
        statPanelController.HidePrimary();
        panelController.HidePrimary();
    }

    protected override void LoadMenu () {
        // Debug.Log("loading default commands");
        if (menuOptions == null){
            menuTitle = "Commands";
            menuOptions = new List<string>(4){
                "MOVE",
                "ACTION",
                "STATUS",
                "PASS"
            };
            menuFunctions = new List<UnityEngine.Events.UnityAction>(4){
                delegate { Move(); },
                delegate { Act(); },
                delegate { Defend(); },
                delegate { Pass(); }
            };
        }
        
        // List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, altMenuFunctions);
        List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, menuFunctions);
        

        //logic for setting stuff as locked depending on actions the playe rhas taken
        // abilityPanelController.SetLocked(0, turn.hasUnitMoved);
        // abilityPanelController.SetLocked(1, turn.hasUnitActed);
        // abilityPanelController.SetLocked(2, turn.hasUnitActed);

    }

    //for when a button is clicked
    protected void Move(){
        Debug.Log("move clicked! " + owner);
        owner.ChangeState<MoveTargetState>();
    }protected void Act(){
        Debug.Log("act clicked!");
        owner.ChangeState<CategorySelectionState>();
    }protected void Defend(){
        Debug.Log("defend clicked!");
        owner.ChangeState<SelectUnitState>();
    }protected void Pass(){
        Debug.Log("pass clicked!");
        owner.ChangeState<SelectUnitState>();
    }

    //for when confirm is pressed
    protected override void Confirm ()
    {
        switch (abilityPanelController.currentSelection)
        {
            case 0: // Move
                // Debug.Log("move pressed");
                owner.ChangeState<MoveTargetState>();
                break;
            case 1: // Action
                // Debug.Log("act pressed");
                owner.ChangeState<CategorySelectionState>();
                break;
            case 2: // Defend, add state later
                // Debug.Log("defend pressed");
                owner.ChangeState<SelectUnitState>();
                break;
            case 3: // Pass
                // Debug.Log("pass pressed");
                owner.ChangeState<SelectUnitState>();
                break;
        }
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
