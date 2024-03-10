using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAbilityState : BattleState 
{
    public override void Enter (){
        base.Enter ();
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;

        //THIS IS RESET IN SELECT UNIT STATE AFTER THE DELAY CALL
        cameraRig.unitMovement = true;
        cameraRig.selectMovement = false;
        StartCoroutine(Animate());
    }
    public override void Exit (){

        base.Exit ();
    }
    
    IEnumerator Animate (){
        Debug.Log("starting to animate " + turn.targets.Count);
        panelController.ShowAbilityDisplay(turn.selectedAbility.gameObject);
        // for (int i = 0; i < turn.targets.Count; ++i){
        //     Debug.Log("looping? ");
        //     Tile target = turn.targets[i];
        //     yield return StartCoroutine(Attack(target));
        //     yield return new WaitForSeconds(10 * Time.deltaTime);
        //     // yield return new WaitForSeconds(.05f);
        // }
        // SelectTile(turn.actingUnit.tile.position);

        yield return StartCoroutine(PerformTargets());
		ApplyAbility();
        
        panelController.HideAbilityDisplay();
		
		if (IsBattleOver()){
            Debug.Log("changing to end");
			owner.ChangeState<EndBattleState>();

        }
		// else if (!UnitHasDied())
		// 	owner.ChangeState<SelectUnitState>();
		else{
			// owner.ChangeState<CommandSelectionState>();
            owner.ChangeState<SelectUnitState>();
        }
    }

    bool UnitHasDied (){
        return turn.actingUnit.GetComponentInChildren<DeadStatusEffect>() == null;
    }

    void ApplyAbility (){
		turn.selectedAbility.Perform(turn.targets);
	}

    public virtual IEnumerator PerformTargets(){
        Debug.Log("starting to perform " + turn.targets.Count);
        for (int i = 0; i < turn.targets.Count; ++i){
            Tile target = turn.targets[i];
            Debug.Log("targeting " +target);
            panelController.ShowSecondary(target.content);
            yield return StartCoroutine(Perform(target));
            yield return new WaitForSeconds(1.2f);
        }
        panelController.HideSecondary();
        Debug.Log("finishing attacks " + turn.targets.Count);
        yield return null;
    }
    IEnumerator Perform (Tile target){
        Debug.Log("performing " + target);
        Transform actorTransform = turn.actingUnit.transform;
        Vector2 startPos = actorTransform.position;
        
        SelectTile(target.position, Board.SelectColor.ENEMY);

        float baseAttackTime = .15f;
        float adjustedAttackSpeed = Vector2.Distance(actorTransform.position, target.center)/baseAttackTime;
        // print("attack move speed " + adjustedAttackSpeed);

        while((Vector2)actorTransform.position != target.center){
            // Debug.Log("moving to target " );
            actorTransform.position  = Vector2.MoveTowards(actorTransform.position, target.center, adjustedAttackSpeed*Time.deltaTime);
            yield return null;
        }
        while((Vector2)actorTransform.position != startPos){
            // Debug.Log("moving back  " );
            actorTransform.position  = Vector2.MoveTowards(actorTransform.position, startPos, adjustedAttackSpeed*Time.deltaTime);
            yield return null;
        }
    }

}