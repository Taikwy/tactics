using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityMenuState : BattleState
{
    // protected string menuTitle;
    protected List<string> menuOptions;
    protected List<UnityEngine.Events.UnityAction> menuFunctions;
    protected List<bool> performable;
    protected List<GameObject> abilities;
    public override void Enter (){
        // Debug.Log("entering base ability menu state");
        base.Enter ();
        SelectTile(turn.actingUnit.tile.position);
        LoadMenu();
        
    }
    public override void Exit (){
        base.Exit ();
        abilityPanelController.Hide();
    }
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            //no longer allowing keyboard controls, just mouse clicking the menu items
            // Confirm();
        }
        else
            Cancel();
    }
    protected override void OnMove (object sender, InfoEventArgs<Point> e) {
        if (e.info.x > 0 || e.info.y < 0){
            //no longer allowing keyboard controls here either
            // abilityPanelController.Next();
        }
        else{
            //no longer allowing keyboard controls here either
            // abilityPanelController.Previous();  
        }
    }
    
    protected abstract void LoadMenu ();
    // protected abstract void Confirm ();
    protected abstract void Cancel ();
}