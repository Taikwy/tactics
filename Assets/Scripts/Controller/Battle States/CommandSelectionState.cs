using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSelectionState : BaseAbilityMenuState
{
    bool updating = false;
    public override void Enter (){
        // Debug.Log("enter command selection  state");
        base.Enter ();
        if(!turn.actingUnit){
            Debug.LogError("NO DRIVER, IM ASSUMING HE DIED, GO BACK TO SELECT UNIT STATE");
            // SelectUnitAgain();
        }
        else{
            SelectTile(turn.actingUnit.tile.position);

            if (driver.Current == Drivers.Computer){
                panelController.ShowPrimary(turn.actingUnit.gameObject);
                board.humanDriver = false;
                StartCoroutine( ComputerTurn() );
            }
            else{
                panelController.ShowStatus(turn.actingUnit.gameObject);
                board.humanDriver = true;
            }
            cameraRig.selectMovement = true;
            cameraRig.unitMovement = false;

            updating = true;
            panelController.ShowMouseControls();
        }
        
    }
    public override void Exit (){
        // Debug.Log("exiting command selection state");
        updating = false;

        base.Exit ();
        panelController.HidePrimary();
        panelController.HideStatus();
        panelController.HideMouseControls();
    }
    void SelectUnitAgain(){
        owner.ChangeState<SelectUnitState>();
    }
    protected void Update(){
        if(!updating)
            return;
        // Debug.Log("commandstate updating");

        // SelectTile(owner.turn.actingUnit.tile.position);
        // SelectTile(turn.actingUnit.tile.position);
    }

    protected override void LoadMenu () {
        // Debug.Log("loading default commands");
        if (menuOptions == null){
            menuOptions = new List<string>(4){
                "MOVE",
                "ACTION",
                // "STATUS",
                "FOCUS",
                "PASS"
            };
            menuFunctions = new List<UnityEngine.Events.UnityAction>(4){
                delegate { Move(); },
                delegate { Act(); },
                // delegate { Status(); },
                delegate { Focus(); },
                delegate { Pass(); },
            };
        }
        
        // List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, altMenuFunctions);
        List<AbilityMenuEntry> menuEntries = abilityPanelController.Show(menuOptions, menuFunctions);
        

        //logic for setting stuff as locked depending on actions the playe rhas taken
        abilityPanelController.SetLocked(0, turn.hasUnitMoved || turn.hasUnitActed);                             //disable movement if already moved or acted
        abilityPanelController.SetLocked(1, turn.hasUnitActed);                             //disable action if already acted
        abilityPanelController.SetLocked(2, turn.hasUnitMoved || MaxedSkillPoints() || turn.hasUnitActed);                             //disable focus if unit has moved or already has maxed out skill points
        // abilityPanelController.SetLocked(2, turn.hasUnitActed);                             //disable action if already acted
        // abilityPanelController.SetLocked(2, turn.hasUnitActed);                             

    }

    //for when a button is clicked
    protected void Move(){
        // Debug.Log("move clicked! " + owner);
        owner.ChangeState<MoveTargetState>();
    }protected void Act(){
        // Debug.Log("act clicked!");
        owner.ChangeState<AbilitySectionState>();
    }
    // protected void Status(){
    //     // Debug.Log("status clicked!");
    //     owner.ChangeState<SelectUnitState>();
    // }
    protected void Focus(){
        // Debug.Log("Focus clicked!");
        // print("selected ability! " + turn.selectedAbility);
        if(MaxedSkillPoints()){
            Debug.LogError("skill points already max, cannot focus");
            return;
        }
        turn.selectedAbility = null;
        turn.hasUnitActed = true;
        owner.turn.actingUnit.GetComponent<SkillPoints>().SK += 2;
        owner.turn.actingUnit.GetComponent<Burst>().BP += owner.turn.actingUnit.GetComponent<Burst>().focusBP;
        owner.ChangeState<PerformAbilityState>();
        // owner.ChangeState<SelectUnitState>();
    }protected void Pass(){
        // Debug.Log("defend clicked!");
        owner.ChangeState<SelectUnitState>();
    }


    //goes to the previous state. pressing cancel or RMB triggers this
    protected override void Cancel (){
        if (turn.hasUnitMoved && !turn.lockMove){
            turn.UndoMove();
            abilityPanelController.SetLocked(0, false);
            abilityPanelController.SetLocked(2, false);
            SelectTile(turn.actingUnit.tile.position);
        }
        else{
            owner.ChangeState<ExploreState>();
        }
    }

    IEnumerator ComputerTurn ()
	{
		if (turn.plan == null){
			turn.plan = owner.cpu.Evaluate();
			turn.selectedAbility = turn.plan.ability;
		}

		yield return new WaitForSeconds (owner.actionDelays.startSelectCommandDelay);

		if (turn.hasUnitMoved == false && turn.plan.moveLocation != turn.actingUnit.tile.position)
			owner.ChangeState<MoveTargetState>();
		else if (turn.hasUnitActed == false && turn.plan.ability != null)
			owner.ChangeState<AbilityTargetState>();
        else if(turn.hasUnitActed == false && turn.plan.subAction == PlanOfAttack.SubAction.FOCUS){
            if(MaxedSkillPoints()){
                print("cpu has max skill points, passing instead");
                Pass();
            }
            else{
                print("cpu is focusing");
                Focus();
            }
        }
        else if(turn.hasUnitActed == false && turn.plan.subAction == PlanOfAttack.SubAction.PASS){
            print("cpu is passing");
            Pass();
        }
		else
			owner.ChangeState<SelectUnitState>();
	}

    bool MaxedSkillPoints(){
        return owner.turn.actingUnit.GetComponent<SkillPoints>().SK >= owner.turn.actingUnit.GetComponent<SkillPoints>().MSK;
    }
    
}
