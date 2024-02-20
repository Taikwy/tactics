using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS CLASS IS UNUSED
public class ActionSelectionState : BaseAbilityMenuState 
{
    public static int category;
    string[] whiteMagicOptions = new string[] { "Cure", "Raise", "Holy" };
    string[] blackMagicOptions = new string[] { "Fire", "Ice", "Lightning" };
    AbilityCatalog catalog;
    public override void Enter (){
        Debug.Log("entering action state");
        base.Enter ();
        // statPanelController.ShowPrimary(turn.actingUnit.gameObject);
        panelController.ShowPrimary(turn.actingUnit.gameObject);
    }
    public override void Exit (){
        Debug.Log("exiting action state");
        base.Exit ();
        // statPanelController.HidePrimary();
        panelController.HidePrimary();
    }
    protected override void LoadMenu ()
    {
        Debug.LogError("does this happen?");
        catalog = turn.actingUnit.GetComponentInChildren<AbilityCatalog>();
        GameObject container = catalog.GetCategory(category);
        int count = catalog.AbilityCount(container);
        if (menuOptions == null)
            menuOptions = new List<string>(count);
        else
            menuOptions.Clear();
        bool[] locks = new bool[count];
        for (int i = 0; i < count; ++i)
        {
            Ability ability = catalog.GetAbility(category, i);
            // AbilitySkillCost cost = ability.GetComponent<AbilitySkillCost>();
            // if (cost)
            //     menuOptions.Add(string.Format("{0}: {1}", ability.name, cost.amount));
            // else
                menuOptions.Add(ability.name);
            locks[i] = !ability.CanPerform();
        }
        // abilityMenuPanelController.Show(menuTitle, menuOptions);
        abilityPanelController.Show(menuOptions);
        for (int i = 0; i < count; ++i){
            abilityMenuPanelController.SetLocked(i, locks[i]);
            abilityPanelController.SetLocked(i, locks[i]);

        }
    }

    protected override void Cancel (){
        owner.ChangeState<CategorySelectionState>();
    }
    void SetOptions (string[] options){
        menuOptions.Clear();
        for (int i = 0; i < options.Length; ++i)
            menuOptions.Add(options[i]);
    }
}