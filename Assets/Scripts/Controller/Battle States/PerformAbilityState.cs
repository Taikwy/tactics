using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Analytics;
// using Unity.Mathematics;

public class PerformAbilityState : BattleState 
{
    //stores info for animation, each index is for each target
    bool[] hit;
    bool[] crit;
    string[] effect;
    List<string>[] effects;
    bool finishedCalculating;

    public override void Enter (){
        base.Enter ();

        this.AddObserver(OnAbilityHit, BaseAbilityEffect.EffectHitEvent);
        this.AddObserver(OnAbilityMiss, BaseAbilityEffect.EffectMissedEvent);
        this.AddObserver(OnAbilityFinishedPerforming, Ability.FinishedPerformingEvent);
        this.AddObserver(OnAbilityCrit, BaseAbilityEffect.EffectCritEvent);
        hit = new bool[turn.targets.Count];
        crit = new bool[turn.targets.Count];
        effect = new string[turn.targets.Count];
        // effects = List<string>()[turn.targets.Count];
        effects = new List<String>[turn.targets.Count];
        for(int i = 0; i < turn.targets.Count; i++){
            effects[i] = new List<string>();
        }
        finishedCalculating = false;

        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;

        //THIS IS RESET IN SELECT UNIT STATE AFTER THE DELAY CALL
        cameraRig.unitMovement = true;
        cameraRig.selectMovement = false;

        panelController.HideTimeline();
        //if there is no selected ability, that means focus was selected
            StartCoroutine(Animate(!turn.selectedAbility));
        // if(!turn.selectedAbility){
        //     StartCoroutine(Focus());

        // }
        // else{
        //     StartCoroutine(Animate(!turn.selectedAbility));
        // }
    }
    public override void Exit (){
        panelController.ShowTimeline();

		this.RemoveObserver(OnAbilityHit, BaseAbilityEffect.EffectHitEvent);
        this.RemoveObserver(OnAbilityMiss, BaseAbilityEffect.EffectMissedEvent);
        this.RemoveObserver(OnAbilityFinishedPerforming, Ability.FinishedPerformingEvent);
        this.RemoveObserver(OnAbilityCrit, BaseAbilityEffect.EffectCritEvent);
        base.Exit ();
    }

    // IEnumerator Focus (){
    //     // Debug.Log("focusing ");
    //     panelController.ShowAbilityDisplay(null, true);
    //     yield return new WaitForSeconds(.5f);
    //     panelController.HideAbilityDisplay();
		
	// 	if (IsBattleOver()){
    //         Debug.Log("changing to end");
	// 		owner.ChangeState<EndBattleState>();

    //     }
	// 	else if (UnitHasDied())
	// 		owner.ChangeState<SelectUnitState>();
	// 	else{
	// 		// owner.ChangeState<CommandSelectionState>();
    //         owner.ChangeState<CommandSelectionState>();
    //     }
    // }
    
    IEnumerator Animate (bool focusing = false){
        Debug.Log("starting to animate " + turn.targets.Count);
        if(focusing){
            panelController.ShowAbilityDisplay(null, true);
            yield return new WaitForSeconds(performStateUI.focusStartDelay);
            performStateUI.DisplayEffect(turn.actingUnit.tile, "+2 SKILL POINTS");
            yield return new WaitForSeconds(performStateUI.focusEndDelay);
            panelController.HideAbilityDisplay();
        }
        else{
            panelController.ShowAbilityDisplay(turn.selectedAbility.gameObject);
            yield return new WaitForSeconds(performStateUI.abilityDisplayDelay);

            ApplyAbility();
            float timeElapsed = 0;
            while(!finishedCalculating || timeElapsed > 2f){
                yield return new WaitForSeconds(Time.deltaTime);
                timeElapsed += Time.deltaTime;
            }
            // print("starting to perform after " + timeElapsed);
            yield return StartCoroutine(PerformTargets());
            
            panelController.HideAbilityDisplay();
        }
        // yield return new WaitForSeconds(.05f);
		
		if (IsBattleOver()){
            Debug.Log("changing to end");
			owner.ChangeState<EndBattleState>();
        }
		else if (UnitHasDied()){
            // Debug.Log("unit died,");
			owner.ChangeState<SelectUnitState>();
        }
		else{
            Debug.Log("finsihed performing, back to command,"); 
			// owner.ChangeState<CommandSelectionState>();
            owner.ChangeState<CommandSelectionState>();
        }
    }

    bool UnitHasDied (){
        return turn.actingUnit.GetComponentInChildren<DeadStatusEffect>() != null;
    }

    void ApplyAbility (){
        print("selected ability " + turn.selectedAbility);
		turn.selectedAbility.Perform(turn.targets);
	}

    public virtual IEnumerator PerformTargets(){
        // Debug.Log("starting to perform " + turn.targets.Count);
        for (int i = 0; i < turn.targets.Count; ++i){
            Tile target = turn.targets[i];
            // Debug.Log("targeting " +target);
            // panelController.ShowSecondary(target.content);
            yield return StartCoroutine(Perform(target));
            yield return new WaitForSeconds(performStateUI.performDelay);
        }
        panelController.HideSecondary();
        // Debug.Log("finishing attacks " + turn.targets.Count);
        yield return null;
    }
    IEnumerator Perform (Tile target){
        int targetIndex = turn.targets.IndexOf(target);
        // Debug.Log("performing " + target + " | " + hit[targetIndex] + " | " + crit[targetIndex] + " | " + effect[targetIndex]);
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
        if(hit[targetIndex]){
            StartCoroutine(AnimateHit(target));
        }
        else if(!hit[targetIndex]){
            StartCoroutine(AnimateMiss(target));
            effects[targetIndex].Add("MISS!");
        }
        // StartCoroutine(AnimateMiss(target));
        // DisplayEffect(target);
        StartCoroutine(DisplayEffects(target));
        while((Vector2)actorTransform.position != startPos){
            // Debug.Log("moving back  " );
            actorTransform.position  = Vector2.MoveTowards(actorTransform.position, startPos, adjustedAttackSpeed*Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator AnimateHit (Tile target){
        // print("animating HIT");
        int targetIndex = turn.targets.IndexOf(target);
        if(crit[targetIndex]){
            for(int i =0; i <8; i++){
                target.content.GetComponent<SpriteRenderer>().color = Color.red;
                yield return new WaitForSeconds(.04f);
                target.content.GetComponent<SpriteRenderer>().color = Color.yellow;
                yield return new WaitForSeconds(.04f);
            }
        }
        else{
            target.content.GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(.15f);
        }
        target.content.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(.1f);
    }
    IEnumerator AnimateMiss (Tile target){
        print("animating miss");
        Transform targetTransform = target.content.transform;
        Vector2 startPos = target.center;
        Vector2 dodgePos = target.center + new Vector2(.75f, 0f);
        
        float dodgeSpeed = 5f;

        print(targetTransform.position + " | " + dodgePos + " | " + startPos);
        while((Vector2)targetTransform.position != dodgePos){
            targetTransform.position  = Vector2.MoveTowards(targetTransform.position, dodgePos, dodgeSpeed*Time.deltaTime);
            yield return null;
        }
        print(targetTransform.position + " | " + dodgePos);
        while((Vector2)targetTransform.position != startPos){
            targetTransform.position  = Vector2.MoveTowards(targetTransform.position, startPos, dodgeSpeed*Time.deltaTime);
            yield return null;
        }
        // print(targetTransform.position + " | " + startPos);
    }
    // void DisplayEffect (Tile target){
    //     print("displaying effect");
    //     int targetIndex = turn.targets.IndexOf(target);
        
    //     performStateUI.DisplayEffect(target, effect[targetIndex]);
    //     // Vector2 targetPos = (Vector2)target.transform.position + performStateUI.unitEffectLabelOffset;
    //     // Unit unit = target.content.GetComponent<Unit>();
    //     // GameObject effectLabel = Instantiate(performStateUI.effectLabelPrefab, targetPos, Quaternion.identity, unit.canvasObj);
    //     // effectLabel.GetComponent<EffectLabel>().Initialize(effect[targetIndex], performStateUI.effectFloatSpeed, performStateUI.effectFadeSpeed);
    //     // print(effect[targetIndex] + " | pos " + targetPos);
    // }

    IEnumerator DisplayEffects (Tile target){
        int targetIndex = turn.targets.IndexOf(target);
        // print("displaying effect " + targetIndex + " ______________________");
        for(int effectIndex = 0; effectIndex < effects[targetIndex].Count; effectIndex++){
            performStateUI.DisplayEffect(target, effects[targetIndex][effectIndex]);




            // Vector2 labelOffset = new Vector2(Random.Range(-.2f,.2f), Random.Range(.55f,.6f));
            // Vector2 targetPos = (Vector2)target.transform.position + performStateUI.unitEffectLabelOffset;
            // Unit unit = target.content.GetComponent<Unit>();
            // GameObject effectLabel = Instantiate(performStateUI.effectLabelPrefab, targetPos, Quaternion.identity, unit.canvasObj);
            // effectLabel.GetComponent<EffectLabel>().Initialize(effects[targetIndex][effectIndex], performStateUI.effectFloatSpeed, performStateUI.effectFadeSpeed);
            // print(effects[targetIndex][effectIndex] + " | pos " + targetPos);

            yield return new WaitForSeconds(performStateUI.effectDisplayDelay);
        }
        yield return null;
    }

    void OnAbilityFinishedPerforming(object sender, object args){
        // print("ABILITY DONE CALCULATING STUFF ");
        finishedCalculating = true;
    }

    void OnAbilityHit(object sender, object args){
        // print("HELLLOOOOOOOOOOOOOOO");
        var info = args as Info<Tile, int>;
        // print("ON HIT TILE " + info.arg0);
        // print("ON HIT INT " + info.arg1);
        int targetIndex = turn.targets.IndexOf(info.arg0);
        hit[targetIndex] = true;

        // print("ON HIT SENDER " + sender);
        if(sender.GetType() == typeof(DamageAbilityEffect)){
            // effect[targetIndex] = info.arg1.ToString();
            effects[targetIndex].Add(info.arg1.ToString());
        }
        else if(sender.GetType() == typeof(HealAbilityEffect)){
            // effect[targetIndex] = info.arg1.ToString();
            effects[targetIndex].Add(info.arg1.ToString());
        }
        else if(sender.GetType() == typeof(InflictAbilityEffect)){
            // effect[targetIndex] = (sender as InflictAbilityEffect).abilityEffectName + "!";
            effects[targetIndex].Add( (sender as InflictAbilityEffect).abilityEffectName + "!");
        }
        else if(sender.GetType() == typeof(PurifyAbilityEffect)){
            // effect[targetIndex] = (sender as PurifyAbilityEffect).abilityEffectName + "!";
            effects[targetIndex].Add( (sender as PurifyAbilityEffect).abilityEffectName + "!");
        }
        // print(info.arg0 + " | " + targetIndex + " ON HIT " + effects[targetIndex].Count);
    }
    void OnAbilityMiss(object sender, object args){
        // print("ON ABILITY MISS " + args);
        int targetIndex = turn.targets.IndexOf(args as Tile);
        // effect[targetIndex] = "MISS!";
        // effects[targetIndex].Add("MISS!");
    }
    void OnAbilityCrit(object sender, object args){
        print("ON ABILITY CRIT " + args);
        int targetIndex = turn.targets.IndexOf(args as Tile);
        crit[targetIndex] = true;
        // effects[targetIndex].Add("CRIT!");
    }

}