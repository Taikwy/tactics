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

        List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, menuFunctions);
        //logic for disabling actions the unit cannot take, ie not enough burst or currently silenced or whatever
        // abilityPanelController.SetLocked(0, turn.hasUnitMoved);                           
    }



    void Attack (GameObject ability){
        Debug.Log("attacking with " + ability);
        turn.selectedAbility = ability.GetComponent<Ability>();
        owner.ChangeState<AbilityTargetState>();
    }

    protected override void Cancel(){
        owner.ChangeState<CommandSelectionState>();
    }

    
    void SetCategory (int index){
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }
}