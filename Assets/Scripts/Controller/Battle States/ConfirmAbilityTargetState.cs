using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//shows ability forecast state p much
public class ConfirmAbilityTargetState : BattleState
{
    List<Tile> targetedTiles, highlightedTiles;
    AbilityArea areaScript;
    int currentTarget = 0;
    AbilityEffectTarget[] targeters;
    public override void Enter (){
        base.Enter ();
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
			SetTarget(0);
		}
        else{
            HideSelect();
            DisplayEffects(turn.actingUnit.tile);
            // SelectTile(turn.actingUnit.tile.position, false);
        }
		if (driver.Current == Drivers.Computer){
            // Debug.LogError("asdasd");
            board.humanDriver = false;
			StartCoroutine(ComputerDisplayAbilitySelection());
        }
        else
            board.humanDriver = true;
    }
    void DisplayEffects (Tile target){
        int targetIndex = turn.targets.IndexOf(target);
        Vector2 labelOffset = new Vector2(0, .6f);
        Vector2 targetPos = (Vector2)target.transform.position + labelOffset;
        Unit unit = target.content.GetComponent<Unit>();
        GameObject effectLabel = Instantiate(owner.performStateUI.effectLabelPrefab, targetPos, Quaternion.identity, unit.canvasObj);

        effectLabel.GetComponent<EffectLabel>().Initialize("NO VALID TARGETS", .75f, 2);
    }

    public override void Exit (){
        // Debug.Log("exiting confirm ability state");
        base.Exit ();
        areaScript.targets.Clear();
        // Debug.Log(highlightedTiles + "        | " + highlightedTiles.Count);
        board.UnhighlightTiles(highlightedTiles);
        board.UntargetTiles(targetedTiles);
        panelController.HidePrimary();
        panelController.HideSecondary();

        ShowSelect();
        forecastPanel.Hide();
    }
    //scrolls thru all the targets affected by the current attack
    protected override void OnMove (object sender, InfoEventArgs<Point> e){
        if (e.info.y > 0 || e.info.x > 0){
            SetTarget(currentTarget + 1);
        }
        else{
            SetTarget(currentTarget - 1);
        }
    }
    protected override void OnFire (object sender, InfoEventArgs<int> e){
        if (e.info == 0){
            // Debug.Log("firing? " + turn.targets.Count);
            if (turn.targets.Count > 0){
                owner.ChangeState<PerformAbilityState>();
            }
            else{
                // Debug.LogError("NO TARGETS, CANNOT CONTINUE");
                DisplayEffects(turn.actingUnit.tile);
            }
        }
        else{
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
    
    void SetTarget (int target)
    {
        currentTarget = target;
        if (currentTarget < 0)
            currentTarget = turn.targets.Count - 1;
        if (currentTarget >= turn.targets.Count)
            currentTarget = 0;

        if (turn.targets.Count > 0){
            RefreshSecondaryPanel(turn.targets[currentTarget].position);
            UpdateForecastPanel ();
            SelectTile(turn.targets[currentTarget].position);
        }
    }

    void UpdateForecastPanel(){
        // Debug.Log("updating forecast panel");
		float hitrate = 0;
		int amount = 0;
        float subHitrate;
        int subAmount;
		Tile target = turn.targets[currentTarget];

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
        SetTarget(0);
        // UpdateForecastPanel();
		yield return new WaitForSeconds (owner.actionDelays.displayActionDelay);
        // print("delaying display action " + owner.actionDelays.displayActionDelay);
		owner.ChangeState<PerformAbilityState>();
	}
}