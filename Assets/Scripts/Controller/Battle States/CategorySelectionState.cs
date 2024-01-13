using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategorySelectionState : BaseAbilityMenuState 
{
    public override void Enter (){
        Debug.Log("entering category state");
        base.Enter ();
        // statPanelController.ShowPrimary(turn.actingUnit.gameObject);
        panelController.ShowPrimary(turn.actingUnit.gameObject);
    }
    public override void Exit (){
        Debug.Log("exiting category state");
        base.Exit ();
        // statPanelController.HidePrimary();
        panelController.HidePrimary();
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

    //loads menu from the current unit's ability catalog
    protected override void LoadMenu ()
    {
        menuOptions = new List<string>();
        menuFunctions = new List<UnityEngine.Events.UnityAction>();

        AbilityCatalog catalog = turn.actingUnit.GetComponentInChildren<AbilityCatalog>();
        for (int i = 0; i < catalog.CategoryCount(); ++i){
            GameObject ability = catalog.GetCategory(i);
            menuOptions.Add( ability.name );
            switch(ability.GetComponent<Ability>().type){
                case AbilityTypes.BASIC:
                    menuFunctions.Add(delegate { Attack(catalog.basicAbility); });
                    break;
                case AbilityTypes.TRAIT:
                    menuFunctions.Add(delegate { Attack(catalog.traitAbility); });
                    break;
                case AbilityTypes.SKILL:
                    menuFunctions.Add(delegate { Attack(catalog.skillAbility); });
                    break;
                case AbilityTypes.BURST:
                    menuFunctions.Add(delegate { Attack(catalog.burstAbility); });
                    break;
            }
        }

        // menuOptions.Add( catalog.GetCategory(0).name);
        // menuOptions.Add( catalog.GetCategory(1).name);
        // menuOptions.Add( catalog.GetCategory(2).name);
        // menuOptions.Add( catalog.GetCategory(3).name);
        // menuFunctions = new List<UnityEngine.Events.UnityAction>(4){
        //     delegate { Basic(); },
        //     delegate { Trait(); },
        //     delegate { Skill(); },
        //     delegate { Burst(); }
        // };
        

        
        List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, menuFunctions);
        //logic for disabling actions the unit cannot take, ie not enough burst or currently silenced or whatever
        // abilityPanelController.SetLocked(0, turn.hasUnitMoved);                             //disable movement if already moved
    }

    // void Basic(){
    //     Debug.Log("basic clicked");
    //     turn.selectedAbility = turn.actingUnit.GetComponentInChildren<Ability>();
    //     // turn.ability = turn.actingUnit.GetComponentInChildren<AbilityRange>().gameObject;
    //     owner.ChangeState<AbilityTargetState>();
    // }void Trait(){
    //     Debug.Log("trait clicked");
    // }void Skill(){
    //     Debug.Log("skill clicked");
    // }void Burst(){
    //     Debug.Log("burst clicked");
    // }

    void Attack (GameObject ability){
        Debug.Log("attacking with " + ability);
        turn.selectedAbility = ability.GetComponent<Ability>();
        owner.ChangeState<AbilityTargetState>();
    }


    // protected override void Confirm ()
    // {
    //     //will need to change this to be separate ex and normal attack
    //     if (abilityPanelController.currentSelection == 0 || abilityPanelController.currentSelection == 1)
    //         Attack();
    //     else
    //         SetCategory(abilityPanelController.currentSelection - 1);
    // }


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

    
    void SetCategory (int index){
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }
}