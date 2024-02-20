using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategorySelectionState : BaseAbilityMenuState 
{
    public override void Enter (){
        // Debug.Log("entering category state");
        base.Enter ();
        // statPanelController.ShowPrimary(turn.actingUnit.gameObject);
        panelController.ShowPrimary(turn.actingUnit.gameObject);
    }
    public override void Exit (){
        // Debug.Log("exiting category state");
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
        performable = new List<bool>();
        menuOptions = new List<string>();
        menuFunctions = new List<UnityEngine.Events.UnityAction>();

        AbilityCatalog catalog = turn.actingUnit.GetComponentInChildren<AbilityCatalog>();
        for (int i = 0; i < catalog.CategoryCount(); ++i){
            GameObject ability = catalog.GetCategory(i);
            //checks whether the ability should be locked depending on whether the unit has enough skill points
            performable.Add(ability.GetComponent<Ability>().CanPerform());
            //new, checks the abilities' cost to add to the name
            AbilitySkillCost skillCost = ability.GetComponent<AbilitySkillCost>();
            AbilityBurstCost burstCost = ability.GetComponent<AbilityBurstCost>();
            if (skillCost)
                menuOptions.Add(string.Format("{0}: {1} skpts", ability.name, skillCost.skillCost));
            else if (burstCost)
                menuOptions.Add(string.Format("{0}: {1} bpts", ability.name, burstCost.burstCost));
            else
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
        

        List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, menuFunctions, performable);
    
        for (int i = 0; i < menuEntries.Count; ++i){
            menuEntries[i].button.interactable = performable[i]; 
        }
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
}