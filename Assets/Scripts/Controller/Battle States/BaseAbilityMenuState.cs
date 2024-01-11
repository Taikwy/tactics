using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityMenuState : BattleState
{
    protected string menuTitle;
    protected List<string> menuOptions;
    protected List<Action> menuFunctions;
    public override void Enter (){
        // Debug.Log("entering base ability menu state");
        base.Enter ();
        SelectTile(turn.actingUnit.tile.position);
        LoadMenu();
        
    }
    public override void Exit (){
        base.Exit ();
        // abilityMenuPanelController.Hide();
        abilityPanelController.Hide();
    }
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            // Confirm();
            Select();
        }
        else
            Cancel();
    }
    protected override void OnMove (object sender, InfoEventArgs<Point> e) {
        if (e.info.x > 0 || e.info.y < 0){
            // abilityMenuPanelController.Next();
            abilityPanelController.Next();
        }
        else{
            // abilityMenuPanelController.Previous();
            abilityPanelController.Previous();  
        }
    }
    
    protected abstract void LoadMenu ();
    protected abstract void Select ();
    protected abstract void Confirm ();
    protected abstract void Cancel ();
}