using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//shows ability forecast state p much
public class ConfirmAbilityTargetState : BattleState
{
    bool updating = false;
    List<Tile> targetedTiles, highlightedTiles;
    AbilityArea areaScript;
    // int currentTarget = 0;
    AbilityEffectTarget[] targeters;
    List<GameObject> indicatedTiles;
    Tile currentTarget;
    public override void Enter (){
        base.Enter ();
        tileSelectionIndicator.ChangeTarget();
        areaScript = turn.selectedAbility.GetComponent<AbilityArea>();
        targetedTiles = areaScript.targets;
        highlightedTiles = new List<Tile>(targetedTiles);
        //sets cursor over the first target
        SelectTile(areaScript.targets[0].position);
        //highlights all the tiles that are targeted, will need new logic here to properly overlay things
        board.HighlightTiles(highlightedTiles, turn.selectedAbility.overlayColor);
        FindTargets();
        RefreshPrimaryPanel(turn.actingUnit.tile.position);
        
        // Debug.LogError(turn.targets.Count);
        if (turn.targets.Count > 0){
			// if (driver.Current == Drivers.Human)
            forecastPanel.Show();
            SetTarget(turn.targets[0]);
            indicatedTiles = IndicateTiles(turn.targets, Board.SelectColor.VALID);
            board.TargetTiles(turn.targets, turn.selectedAbility.overlayColor);
		}
        //this should no longer be used to this logic being in abilitytargetstate
        // else{
        //     HideSelect();
        //     DisplayEffects(turn.actingUnit.tile);
        //     // SelectTile(turn.actingUnit.tile.position, false);
        // }
		if (driver.Current == Drivers.Computer){
            // Debug.LogError("asdasd");
            updating = false;
            board.humanDriver = false;
			StartCoroutine(ComputerDisplayAbilitySelection());
        }
        else{
            // cameraRig.selectMovement = false;
            updating = true;
            board.humanDriver = true;
        }
        
        panelController.ShowMouseControls("PERFORM", "CANCEL");
    }
    void DisplayEffects (Tile target){
        
        performStateUI.DisplayEffect(target, "NO VALID TARGETS");
        // Vector2 targetPos = (Vector2)target.transform.position + performStateUI.unitEffectLabelOffset;
        // Unit unit = target.content.GetComponent<Unit>();
        // GameObject effectLabel = Instantiate(performStateUI.effectLabelPrefab, targetPos, Quaternion.identity, unit.canvasObj);

        // effectLabel.GetComponent<EffectLabel>().Initialize("NO VALID TARGETS", performStateUI.effectFloatSpeed, performStateUI.effectFadeSpeed);
    }

    public override void Exit (){
        // Debug.Log("exiting confirm ability state");
        tileSelectionIndicator.ChangeSelect();
        cameraRig.selectMovement = true;
        updating = false;
        base.Exit ();
        areaScript.targets.Clear();
        StopIndicating(indicatedTiles);
        // Debug.Log(highlightedTiles + "        | " + highlightedTiles.Count);
        board.UnhighlightTiles(highlightedTiles);
        board.UntargetTiles(targetedTiles);
        panelController.HidePrimary();
        panelController.HideSecondary();
        IndicateTimeline(turn.actingUnit.tile);

        ShowSelect();
        forecastPanel.Hide();
        panelController.HideMouseControls();
    }
    protected void Update(){
        if(!updating ) 
            return;
        
        // SelectTile(board.selectedPoint);
        if(turn.targets.Contains(board.selectedTile)){
            // forecastPanel.Show();
            SelectTile(board.selectedPoint, Board.SelectColor.ENEMY);
            // RefreshSecondaryPanel(board.selectedPoint);
            SetTarget(board.selectedTile);
        }
        // else
        //     forecastPanel.Hide();
        // UpdateForecastPanel(board.selectedTile);
        IndicateTimeline(board.selectedTile);
        // TargetTiles();
    }
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            // Debug.Log("firing? " + turn.targets.Count);
            if (turn.targets.Count > 0){
                audioManager.PlaySFX(owner.confirmSound);
                owner.ChangeState<PerformAbilityState>();
            }
            else{
                // Debug.LogError("NO TARGETS, CANNOT CONTINUE");
                DisplayEffects(turn.actingUnit.tile);
            }
        }
        else{
            turn.targets.Clear();
            owner.ChangeState<AbilityTargetState>();
            SelectTile(turn.actingUnit.tile.position);
        }
    }
    //find all tiles with a thing that can be targeted, usually units but also maybe other things in the map
    void FindTargets (){
        turn.targets = new List<Tile>();
        for (int i = 0; i < targetedTiles.Count; ++i)
			if (turn.selectedAbility.IsTarget(targetedTiles[i]))
				turn.targets.Add(targetedTiles[i]);
    }
    void SetTarget (Tile target){
        currentTarget = target;

        if (turn.targets.Count > 0){
            RefreshSecondaryPanel(currentTarget.position);
            // UpdateForecastPanel ();
            UpdateForecastPanel(currentTarget);
            SelectTile(currentTarget.position);
        }
    }
    // void SetTarget (int target){
    //     currentTarget = target;
    //     if (currentTarget < 0)
    //         currentTarget = turn.targets.Count - 1;
    //     if (currentTarget >= turn.targets.Count)
    //         currentTarget = 0;

    //     if (turn.targets.Count > 0){
    //         RefreshSecondaryPanel(turn.targets[currentTarget].position);
    //         // UpdateForecastPanel ();
    //         UpdateForecastPanel(turn.targets[currentTarget]);
    //         SelectTile(turn.targets[currentTarget].position);
    //     }
    // }
    void UpdateForecastPanel(Tile target){
        // Debug.Log("updating forecast panel");
		float hitrate = 0;
		int amount = 0;
        float subHitrate;
        int subAmount;
        if(target.content == null)
            return;

		Transform obj = turn.selectedAbility.transform;
		for (int i = 0; i < obj.childCount; ++i){
			AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
            //loops thru the possible targets the ability can have to check and calculate hitrate and effect
			if (targeter.IsTarget(target)){
				HitRate hitRate = targeter.GetComponent<HitRate>();
				hitrate = hitRate.CalculateHitRate(target);

				BaseAbilityEffect effect = targeter.GetComponent<BaseAbilityEffect>();
				amount = effect.Predict(target);
				break;
			}
		}

		forecastPanel.SetStats(turn.actingUnit, target.content, turn.selectedAbility.gameObject, hitrate, amount);
    }

    void UpdateForecastPanel(){
        // Debug.Log("updating forecast panel");
		float hitrate = 0;
		int amount = 0;
        float subHitrate;
        int subAmount;
		Tile target = currentTarget;

		Transform obj = turn.selectedAbility.transform;
		for (int i = 0; i < obj.childCount; ++i)
		{
			AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
            //loops thru the possible targets the ability can have to check and calculate hitrate and effect
			if (targeter.IsTarget(target))
			{
				HitRate hitRate = targeter.GetComponent<HitRate>();
				hitrate = hitRate.CalculateHitRate(target);

				BaseAbilityEffect effect = targeter.GetComponent<BaseAbilityEffect>();
				amount = effect.Predict(target);
                
                // if(effect.hasSubEffects){
                //     HitRate hitRate = targeter.GetComponent<HitRate>();
				//     hitrate = hitRate.CalculateHitRate(target);

                //     BaseAbilityEffect effect = targeter.GetComponent<BaseAbilityEffect>();
                //     amount = effect.Predict(target);
                // }
                // Debug.Log("predicted damage " + amount);
				break;
			}
		}

		forecastPanel.SetStats(turn.actingUnit, target.content, turn.selectedAbility.gameObject, hitrate, amount);
    }

    //old updateforecastpanel, unused
    // void UpdateHitSuccessIndicator ()
	// {
    //     Debug.Log("updating hit success indicator");
	// 	float hitrate = 0;
	// 	int amount = 0;
	// 	Tile target = turn.targets[currentTarget];

	// 	Transform obj = turn.selectedAbility.transform;
	// 	for (int i = 0; i < obj.childCount; ++i)
	// 	{
	// 		AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
	// 		if (targeter.IsTarget(target))
	// 		{
    //             // Debug.Log("targets");
	// 			HitRate hitRate = targeter.GetComponent<HitRate>();
	// 			hitrate = hitRate.CalculateHitRate(target);

	// 			BaseAbilityEffect effect = targeter.GetComponent<BaseAbilityEffect>();
	// 			amount = effect.Predict(target);
                
    //             // Debug.Log("predicted damage " + amount);
	// 			break;
	// 		}
	// 	}

	// 	forecastPanel.SetStats(turn.actingUnit, target.content, turn.selectedAbility.gameObject, hitrate, amount);
	// }

    //ai stuff later
    // IEnumerator ComputerDisplayAbilitySelection ()
	// {
	// 	owner.battleMessageController.Display(turn.ability.name);
	// 	yield return new WaitForSeconds (2f);
	// 	owner.ChangeState<PerformAbilityState>();
	// }
    IEnumerator ComputerDisplayAbilitySelection (){
		// owner.battleMessageController.Display(turn.ability.name);
        Debug.Log(" i need to add ui here to display this " + turn.selectedAbility.name);
        SetTarget(turn.targets[0]);
        // UpdateForecastPanel();
		yield return new WaitForSeconds (owner.actionDelays.displayActionDelay);
        // print("delaying display action " + owner.actionDelays.displayActionDelay);
		owner.ChangeState<PerformAbilityState>();
	}
}