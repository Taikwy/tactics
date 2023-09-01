using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategorySelectionState : BaseAbilityMenuState 
{
    public override void Enter (){
        base.Enter ();
        statPanelController.ShowPrimary(turn.actingUnit.gameObject);
    }
    public override void Exit (){
        base.Exit ();
        statPanelController.HidePrimary();
    }
    // protected override void LoadMenu (){
    //     if (menuOptions == null){
    //         menuTitle = "Action";
    //         menuOptions = new List<string>(4);
    //         menuOptions.Add("Attack");
    //         menuOptions.Add("EX Attack");
    //         menuOptions.Add("Skill");
    //         menuOptions.Add("Super");
    //     }
        
    //     abilityMenuPanelController.Show(menuTitle, menuOptions);
    // }
    protected override void LoadMenu ()
    {
        if (menuOptions == null)
            menuOptions = new List<string>();
        else
            menuOptions.Clear();
        menuTitle = "Action";
        menuOptions.Add("Attack");
        AbilityCatalog catalog = turn.actingUnit.GetComponentInChildren<AbilityCatalog>();
        for (int i = 0; i < catalog.CategoryCount(); ++i)
            menuOptions.Add( catalog.GetCategory(i).name );
        
        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }
    protected override void Confirm ()
    {
        //will need to change this to be separate ex and normal attack
        if (abilityMenuPanelController.selection == 0 || abilityMenuPanelController.selection == 1)
            Attack();
        else
            SetCategory(abilityMenuPanelController.selection - 1);
    }
    // protected override void Confirm ()
    // {
    //     switch (abilityMenuPanelController.selection) {
    //         case 0:
    //             Debug.Log("attack");
    //             Attack();
    //             break;
    //         case 1:
    //             Debug.Log("ex attack");
    //             Attack();
    //             break;
    //         case 2:
    //             Debug.Log("skill");
    //             SetCategory(0);
    //             break;
    //         case 3:
    //             Debug.Log("super");
    //             SetCategory(1);
    //             break;
    //     }
    // }
    
    protected override void Cancel(){
        owner.ChangeState<CommandSelectionState>();
    }
    void Attack (){
        turn.ability = turn.actingUnit.GetComponentInChildren<Ability>();
        // turn.ability = turn.actingUnit.GetComponentInChildren<AbilityRange>().gameObject;
        owner.ChangeState<AbilityTargetState>();
    }
    void SetCategory (int index){
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }
}