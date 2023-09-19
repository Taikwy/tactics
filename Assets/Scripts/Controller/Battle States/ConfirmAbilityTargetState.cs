using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmAbilityTargetState : BattleState
{
    List<Tile> targetedTiles;
    AbilityArea areaScript;
    int currentTarget = 0;
    AbilityEffectTarget[] targeters;
    public override void Enter (){
        base.Enter ();
        areaScript = turn.selectedAbility.GetComponent<AbilityArea>();
        targetedTiles = areaScript.targets;
        //sets cursor over the first target
        SelectTile(areaScript.targets[0].position);
        //highlights all the tiles that are targeted, will need new logic here to properly overlay things
        board.HighlightTiles(targetedTiles, Board.OverlayColor.ATTACK);
        FindTargets();
        RefreshBasePanel(turn.actingUnit.tile.position);
        
        if (turn.targets.Count > 0)
        {
            // Debug.Log("targetting " + turn.targets.Count);
            forecastPanel.Show();
            SetTarget(0);
        }

        if (turn.targets.Count > 0)
		{
			// if (driver.Current == Drivers.Human)
			// 	hitSuccessIndicator.Show();
			SetTarget(0);
		}
		// if (driver.Current == Drivers.Computer)
		// 	StartCoroutine(ComputerDisplayAbilitySelection());
    }

    public override void Exit (){
        base.Exit ();
        areaScript.targets.Clear();
        board.UnhighlightTiles(targetedTiles);
        board.UntargetTiles(targetedTiles);
        statPanelController.HidePrimary();
        statPanelController.HideSecondary();
        panelController.HideBase();

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
            if (turn.targets.Count > 0){
                owner.ChangeState<PerformAbilityState>();
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
            RefreshSecondaryBasePanel(turn.targets[currentTarget].position);
            UpdateHitSuccessIndicator ();
            SelectTile(turn.targets[currentTarget].position);
        }
    }

    void UpdateHitSuccessIndicator ()
	{
        Debug.Log("updating hit success indicator");
		float hitrate = 0;
		int amount = 0;
		Tile target = turn.targets[currentTarget];

		Transform obj = turn.selectedAbility.transform;
		for (int i = 0; i < obj.childCount; ++i)
		{
			AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
			if (targeter.IsTarget(target))
			{
                // Debug.Log("targets");
				HitRate hitRate = targeter.GetComponent<HitRate>();
				hitrate = hitRate.CalculateHitRate(target);

				BaseAbilityEffect effect = targeter.GetComponent<BaseAbilityEffect>();
				amount = effect.Predict(target);
                
                // Debug.Log("predicted damage " + amount);
				break;
			}
		}

		forecastPanel.SetStats(turn.actingUnit, target.content, turn.selectedAbility.gameObject, hitrate, amount);
	}

    //ai stuff later
    // IEnumerator ComputerDisplayAbilitySelection ()
	// {
	// 	owner.battleMessageController.Display(turn.ability.name);
	// 	yield return new WaitForSeconds (2f);
	// 	owner.ChangeState<PerformAbilityState>();
	// }
}