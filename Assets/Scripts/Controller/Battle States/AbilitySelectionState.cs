using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AbilitySectionState : BaseAbilityMenuState 
{
    // List<UnityEngine.Events.UnityAction> menuHighlightFunctions;
    // List<UnityEngine.Events.UnityAction> menuUnhighlightFunctions;

    public override void Enter (){
        // Debug.Log("entering category state");
        base.Enter ();
        panelController.ShowPrimary(turn.actingUnit.gameObject);
        // panelController.ShowStatus(turn.actingUnit.gameObject);
        panelController.ShowMouseControls();
    }
    public override void Exit (){
        // Debug.Log("exiting category state");
        base.Exit ();
        abilityPanelController.DestroyAbilityInfoPanel();
        // panelController.HideStatus();
        panelController.HidePrimary();
        panelController.HideMouseControls();
    }


    //loads menu from the current unit's ability catalog
    protected override void LoadMenu ()
    {
        performable = new List<bool>();
        menuOptions = new List<string>();
        menuFunctions = new List<UnityEngine.Events.UnityAction>();
        // menuHighlightFunctions = new List<UnityEngine.Events.UnityAction>();
        // menuUnhighlightFunctions = new List<UnityEngine.Events.UnityAction>();
        abilities = new List<GameObject>();

        AbilityCatalog catalog = turn.actingUnit.GetComponentInChildren<AbilityCatalog>();
        for (int i = 0; i < catalog.CategoryCount(); ++i){
            GameObject ability = catalog.GetCategory(i);
            abilities.Add(ability);
            performable.Add(ability.GetComponent<Ability>().CanPerform());                                  //checks whether the ability should be locked depending on whether the unit has enough skill points
            //new, checks the abilities' cost to add to the name
            AbilitySkillCost skillCost = ability.GetComponent<AbilitySkillCost>();
            AbilityBurstCost burstCost = ability.GetComponent<AbilityBurstCost>();
            if (skillCost)
                menuOptions.Add(string.Format(ability.name));
                // menuOptions.Add(string.Format("{0}: {1} skpts", ability.name, skillCost.cost));
            else if (burstCost)
                menuOptions.Add(string.Format(ability.name));
                // menuOptions.Add(string.Format("{0}: {1} bpts", ability.name, burstCost.cost));
            else
                menuOptions.Add( ability.name );
            switch(ability.GetComponent<Ability>().type){
                case AbilityTypes.BASIC:
                    menuFunctions.Add(delegate { Attack(catalog.basicAbility); });
                    break;
                // case AbilityTypes.TRAIT:
                //     menuFunctions.Add(delegate { Attack(catalog.traitAbility); });
                //     break;
                case AbilityTypes.SKILL:
                    if(ability == catalog.primarySkillAbility)
                        menuFunctions.Add(delegate { Attack(catalog.primarySkillAbility ); });
                    if(ability == catalog.secondarySkillAbility)
                        menuFunctions.Add(delegate { Attack(catalog.secondarySkillAbility ); });
                    break;
                case AbilityTypes.BURST:
                    menuFunctions.Add(delegate { Attack(catalog.burstAbility); });
                    break;
            }
        }
        

        // List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, performable, menuFunctions, menuHighlightFunctions, menuUnhighlightFunctions);
        List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(abilities, menuOptions, performable, menuFunctions);
    
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
        audioManager.PlaySFX(owner.cancelSound);
    }
}