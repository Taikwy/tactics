using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionState : BaseAbilityMenuState 
{
    public static int category;
    string[] whiteMagicOptions = new string[] { "Cure", "Raise", "Holy" };
    string[] blackMagicOptions = new string[] { "Fire", "Ice", "Lightning" };
    AbilityCatalog catalog;
    public override void Enter (){
        base.Enter ();
        statPanelController.ShowPrimary(turn.actingUnit.gameObject);
        panelController.ShowBase(turn.actingUnit.gameObject);
    }
    public override void Exit (){
        base.Exit ();
        statPanelController.HidePrimary();
        panelController.HideBase();
    }
    protected override void LoadMenu ()
    {
        catalog = turn.actingUnit.GetComponentInChildren<AbilityCatalog>();
        GameObject container = catalog.GetCategory(category);
        menuTitle = container.name;
        int count = catalog.AbilityCount(container);
        if (menuOptions == null)
            menuOptions = new List<string>(count);
        else
            menuOptions.Clear();
        bool[] locks = new bool[count];
        for (int i = 0; i < count; ++i)
        {
            Ability ability = catalog.GetAbility(category, i);
            // AbilityMagicCost cost = ability.GetComponent<AbilityMagicCost>();
            // if (cost)
            //     menuOptions.Add(string.Format("{0}: {1}", ability.name, cost.amount));
            // else
                menuOptions.Add(ability.name);
            locks[i] = !ability.CanPerform();
        }
        abilityMenuPanelController.Show(menuTitle, menuOptions);
        abilityPanelController.Show(menuOptions);
        for (int i = 0; i < count; ++i){
            abilityMenuPanelController.SetLocked(i, locks[i]);
            abilityPanelController.SetLocked(i, locks[i]);

        }
    }
    protected override void Confirm ()
    {
        turn.selectedAbility = catalog.GetAbility(category, abilityMenuPanelController.selection);
        turn.selectedAbility = catalog.GetAbility(category, abilityPanelController.selection);
        owner.ChangeState<AbilityTargetState>();
    }

    // protected override void LoadMenu () {
    //     if (menuOptions == null)
    //         menuOptions = new List<string>(3);
    //     if (category == 0){
    //         Debug.Log("loading white magic");
    //         menuTitle = "White Magic";
    //         SetOptions(whiteMagicOptions);
    //     }
    //     else {
    //         Debug.Log("loading black magic");
    //         menuTitle = "Black Magic";
    //         SetOptions(blackMagicOptions);
    //     }
    //     abilityMenuPanelController.Show(menuTitle, menuOptions);
    // }
    // protected override void Confirm (){
    //         Debug.Log("confirming but idk when this happens");
    //     turn.hasUnitActed = true;
    //     if (turn.hasUnitMoved)
    //         turn.lockMove = true;
    //     owner.ChangeState<CommandSelectionState>();
    // }
    protected override void Cancel (){
        owner.ChangeState<CategorySelectionState>();
    }
    void SetOptions (string[] options){
        menuOptions.Clear();
        for (int i = 0; i < options.Length; ++i)
            menuOptions.Add(options[i]);
    }
}