using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSelectionState : BaseAbilityMenuState
{
    public override void Enter (){
        base.Enter ();
        statPanelController.ShowPrimary(turn.actingUnit.gameObject);
            panelController.ShowPrimary(turn.actingUnit.gameObject);
    }
    public override void Exit (){
        base.Exit ();
        statPanelController.HidePrimary();
        panelController.HidePrimary();
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
        // abilityMenuPanelController.Show(menuTitle, menuOptions);
        // abilityMenuPanelController.SetLocked(0, turn.hasUnitMoved);
        // abilityMenuPanelController.SetLocked(1, turn.hasUnitActed);
        // abilityMenuPanelController.SetLocked(2, turn.hasUnitActed);

        abilityPanelController.Show(menuOptions);
        abilityPanelController.SetLocked(0, turn.hasUnitMoved);
        abilityPanelController.SetLocked(1, turn.hasUnitActed);
        abilityPanelController.SetLocked(2, turn.hasUnitActed);

        for(int i = 0; i < abilityPanelController.menuEntries.Count; i++){
            int temp = i;
            // abilityPanelController.menuEntries[i].gameObject.GetComponent<Button>().onClick.AddListener(delegate{ButtonClicked(temp);});
            abilityPanelController.menuEntries[i].gameObject.GetComponent<Button>().onClick.AddListener(delegate{Select();});
            // Debug.Log(i);   
        }
    }

    // private void ButtonClicked(int index)
    // {
    //     Debug.Log("Button: " + index + " was selected ");   
    //     switch (index)
    //     {
    //         case 0: // Move
    //             Debug.Log("move " + index);
    //             owner.ChangeState<MoveTargetState>();
    //             break;
    //         case 1: // Action
    //             Debug.Log("act " + index);
    //             owner.ChangeState<CategorySelectionState>();
    //             break;
    //         case 2: // Defend, add state later
    //             Debug.Log("defend " + index);
    //             // owner.ChangeState<EndFacingState>();
    //             owner.ChangeState<SelectUnitState>();
    //             break;
    //         case 3: // Pass
    //             // Debug.Log("wait");
    //             // owner.ChangeState<EndFacingState>();
                
    //             owner.ChangeState<SelectUnitState>();
    //             break;
    //     }
    // }
    protected override void Select ()
    {
        switch (abilityPanelController.currentSelection)
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

    protected override void Confirm ()
    {
        switch (abilityPanelController.currentSelection)
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
