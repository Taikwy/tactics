using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PerformAbilityState : BattleState 
{
    //stores info for animation, each index is for each target
    bool[] hit;
    bool[] crit;
    string[] effect;
    bool finishedCalculating;

    public override void Enter (){
        base.Enter ();

        
        this.AddObserver(OnAbilityHit, BaseAbilityEffect.EffectHitEvent);
        this.AddObserver(OnAbilityMiss, BaseAbilityEffect.EffectMissedEvent);
        this.AddObserver(OnAbilityFinishedPerforming, Ability.FinishedPerformingEvent);
        // this.AddObserver(OnAbilityCrit, BaseAbilityEffect.EffectHitEvent);
        hit = new bool[turn.targets.Count];
        crit = new bool[turn.targets.Count];
        effect = new string[turn.targets.Count];
        finishedCalculating = false;

        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;

        //THIS IS RESET IN SELECT UNIT STATE AFTER THE DELAY CALL
        cameraRig.unitMovement = true;
        cameraRig.selectMovement = false;
        StartCoroutine(Animate());
    }
    public override void Exit (){

		this.RemoveObserver(OnAbilityHit, BaseAbilityEffect.EffectHitEvent);
        this.RemoveObserver(OnAbilityMiss, BaseAbilityEffect.EffectMissedEvent);
        this.RemoveObserver(OnAbilityFinishedPerforming, Ability.FinishedPerformingEvent);
        // this.RemoveObserver(OnAbilityCrit, BaseAbilityEffect.EffectHitEvent);
        base.Exit ();
    }
    
    IEnumerator Animate (){
        Debug.Log("starting to animate " + turn.targets.Count);
		ApplyAbility();
        panelController.ShowAbilityDisplay(turn.selectedAbility.gameObject);
        float timeElapsed = 0;
        while(!finishedCalculating || timeElapsed > 2f){
            yield return new WaitForSeconds(Time.deltaTime);
            timeElapsed += Time.deltaTime;
        }
        print("starting to perform after " + timeElapsed);
        yield return StartCoroutine(PerformTargets());
        
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
        // Debug.Log("starting to perform " + turn.targets.Count);
        for (int i = 0; i < turn.targets.Count; ++i){
            Tile target = turn.targets[i];
            // Debug.Log("targeting " +target);
            panelController.ShowSecondary(target.content);
            yield return StartCoroutine(Perform(target));
            yield return new WaitForSeconds(.5f);
        }
        panelController.HideSecondary();
        // Debug.Log("finishing attacks " + turn.targets.Count);
        yield return null;
    }
    IEnumerator Perform (Tile target){
        int targetIndex = turn.targets.IndexOf(target);
        Debug.Log("performing " + target + " | " + hit[targetIndex] + " | " + crit[targetIndex] + " | " + effect[targetIndex]);
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
        if(hit[targetIndex])
            StartCoroutine(AnimateHit(target));
        else if(!hit[targetIndex])
            StartCoroutine(AnimateMiss(target));
        DisplayEffect(target);
        while((Vector2)actorTransform.position != startPos){
            // Debug.Log("moving back  " );
            actorTransform.position  = Vector2.MoveTowards(actorTransform.position, startPos, adjustedAttackSpeed*Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator AnimateHit (Tile target){
        print("animating HIT");
        int targetIndex = turn.targets.IndexOf(target);
        if(crit[targetIndex])
            target.content.GetComponent<SpriteRenderer>().color = Color.red;
        else
            target.content.GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(.15f);
        target.content.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(.1f);
    }
    IEnumerator AnimateMiss (Tile target){
        print("animating miss");
        Transform targetTransform = target.content.transform;
        Vector2 startPos = target.center;
        Vector2 dodgePos = target.center + new Vector2(.75f, 0f);
        
        float dodgeSpeed = 5f;

        while((Vector2)targetTransform.position != dodgePos){
            targetTransform.position  = Vector2.MoveTowards(targetTransform.position, dodgePos, dodgeSpeed*Time.deltaTime);
            yield return null;
        }
        while((Vector2)targetTransform.position != startPos){
            targetTransform.position  = Vector2.MoveTowards(targetTransform.position, startPos, dodgeSpeed*Time.deltaTime);
            yield return null;
        }
    }
    void DisplayEffect (Tile target){
        print("displaying effect");
        Camera cam = owner.cameraRig.GetComponentInChildren<Camera>();
        // Vector2 targetPos = cam.WorldToScreenPoint((Vector2)target.transform.position + new Vector2(0, 1f));
        Vector2 targetPos = (Vector2)target.transform.position + new Vector2(0, 1f);
        Unit unit = target.content.GetComponent<Unit>();
        GameObject effectLabel = Instantiate(owner.performStateUI.effectLabelPrefab, targetPos, Quaternion.identity, unit.canvasObj);
        // GameObject effectLabel = Instantiate(owner.performStateUI.effectLabelPrefab, targetPos, Quaternion.identity, owner.performStateUI.effectLabelContainer.transform);

        int targetIndex = turn.targets.IndexOf(target);
        effectLabel.GetComponent<EffectLabel>().Initialize(effect[targetIndex], 1, 3);
        print(effect[targetIndex] + " | pos " + targetPos);
    }
    IEnumerator AnimateEffect (Tile target){
        return null;
    }

    void OnAbilityFinishedPerforming(object sender, object args){
        print("ABILITY DONE CALCULATING STUFF ");
        finishedCalculating = true;
    }

    void OnAbilityHit(object sender, object args){
        var info = args as Info<Tile, int>;
        // print("ON HIT TILE " + info.arg0);
        // print("ON HIT INT " + info.arg1);
        int targetIndex = turn.targets.IndexOf(info.arg0);
        hit[targetIndex] = true;

        // print("ON HIT SENDER " + sender);
        if(sender.GetType() == typeof(DamageAbilityEffect)){
            effect[targetIndex] = info.arg1.ToString();
        }
        else if(sender.GetType() == typeof(HealAbilityEffect)){
            effect[targetIndex] = info.arg1.ToString();
        }
        else if(sender.GetType() == typeof(InflictAbilityEffect)){
            effect[targetIndex] = (sender as InflictAbilityEffect).abilityEffectName;
        }
        else if(sender.GetType() == typeof(PurifyAbilityEffect)){
            effect[targetIndex] = (sender as InflictAbilityEffect).abilityEffectName;
        }
        // print("ON HIT EFFECT " + effect[targetIndex]);
    }
    void OnAbilityMiss(object sender, object args){
        // print("ON ABILITY MISS " + args);
        int targetIndex = turn.targets.IndexOf(args as Tile);
        effect[targetIndex] = "MISS!";
    }
    void OnAbilityCrit(object sender, object args){
        print("ON ABILITY CRIT " + args);
        int targetIndex = turn.targets.IndexOf(args as Tile);
        crit[targetIndex] = true;
    }

}