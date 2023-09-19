using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSelectionState : BaseAbilityMenuState
{
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
    protected override void LoadMenu () {
        // Debug.Log("loading default commands");
        if (menuOptions == null){
            menuTitle = "Commands";
            menuOptions = new List<string>(4);
            menuOptions.Add("MOVE");
            menuOptions.Add("ACTION");
            menuOptions.Add("STATUS");
            menuOptions.Add("PASS");
        }

        //SETS THE OTHER ENTRIES AS LOCKED DEPENDING ON WHAT THE UNIT HAS ALREADY DONE
        abilityMenuPanelController.Show(menuTitle, menuOptions);
        abilityMenuPanelController.SetLocked(0, turn.hasUnitMoved);
        abilityMenuPanelController.SetLocked(1, turn.hasUnitActed);
        abilityMenuPanelController.SetLocked(2, turn.hasUnitActed);

        abilityPanelController.Show(menuOptions);
        abilityPanelController.SetLocked(0, turn.hasUnitMoved);
        abilityPanelController.SetLocked(1, turn.hasUnitActed);
        abilityPanelController.SetLocked(2, turn.hasUnitActed);
    }

    protected override void Confirm ()
    {
        switch (abilityPanelController.selection)
        {
            case 0: // Move
                Debug.Log("move");
                owner.ChangeState<MoveTargetState>();
                break;
            case 1: // Action
                Debug.Log("act");
                owner.ChangeState<CategorySelectionState>();
                break;
            case 2: // Defend, add state later
                Debug.Log("defend");
                // owner.ChangeState<EndFacingState>();
                owner.ChangeState<SelectUnitState>();
                break;
            case 3: // Pass
                // Debug.Log("wait");
                // owner.ChangeState<EndFacingState>();
                
                owner.ChangeState<SelectUnitState>();
                break;
        }
    }

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
