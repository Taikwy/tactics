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
        // targetedTiles = areaScript.GetTargetedTiles(board, selectPos);
        targetedTiles = areaScript.targets;
        // board.SelectTiles(tiles);
        SelectTile(areaScript.targets[0].position);
        // board.HighlightAttackTiles(targetedTiles);
        board.HighlightTiles(targetedTiles, Board.OverlayColor.ATTACK);
        FindTargets();
        RefreshBasePanel(turn.actingUnit.tile.position);
        SetTarget(0);
        if (turn.targets.Count > 0)
        {
            // Debug.Log("targetting " + turn.targets.Count);
            forecastPanel.Show();
            SetTarget(0);
        }
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
            // SelectTile(turn.targets[currentTarget].position);
            // Tile target = turn.targets[currentTarget];
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
    void FindTargets (){
        turn.targets = new List<Tile>();
        AbilityEffectTarget[] targeters = turn.selectedAbility.GetComponentsInChildren<AbilityEffectTarget>();
        for (int i = 0; i < targetedTiles.Count; ++i){
            if (IsTarget(targetedTiles[i], targeters))
                turn.targets.Add(targetedTiles[i]);
        }
    }
    
    bool IsTarget (Tile tile, AbilityEffectTarget[] list){
        for (int i = 0; i < list.Length; ++i)
        if (list[i].IsTarget(tile))
            return true;
        
        return false;
    }
    void SetTarget (int target)
    {
        currentTarget = target;
        if (currentTarget < 0)
            currentTarget = turn.targets.Count - 1;
        if (currentTarget >= turn.targets.Count)
            currentTarget = 0;
        if (turn.targets.Count > 0)
        {
            RefreshSecondaryBasePanel(turn.targets[currentTarget].position);
            UpdateHitSuccessIndicator ();
            SelectTile(turn.targets[currentTarget].position);
        }
    }
    // void UpdateHitSuccessIndicator ()
    // {
    //     float hitrate = CalculateHitRate();
    //     int damage = EstimateDamage();
    //     hitSuccessIndicator.SetStats(hitrate, damage);
    // }
    // void UpdateHitSuccessIndicator (){
    //     float hitrate = 0;
    //     int amount = 0;
    //     Tile target = turn.targets[currentTarget];
    //     AbilityEffectTarget[] targeters = turn.ability.GetComponentsInChildren<AbilityEffectTarget>();
    //     for (int i = 0; i < targeters.Length; ++i)
    //     {
    //         if (targeters[i].IsTarget(target))
    //         {
    //             HitRate hitRate = targeters[i].GetComponent<HitRate>();
    //             hitrate = hitRate.CalculateHitRate(target);
    //             BaseAbilityEffect effect = targeters[i].GetComponent<BaseAbilityEffect>();
    //             amount = effect.Predict(target);
    //             break;
    //         }
    //     }
    //     hitSuccessIndicator.SetStats(hitrate, amount);
    // }
    void UpdateHitSuccessIndicator ()
	{
        Debug.Log("updating");
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
    float CalculateHitRate ()
    {
        //Will have to change this if i want to let things attack inanimate objects
        // Unit target = turn.targets[currentTarget].content.GetComponent<Unit>();
        Tile target = turn.targets[currentTarget];
        HitRate hitrateScript = turn.selectedAbility.GetComponentInChildren<HitRate>();
        return hitrateScript.CalculateHitRate(target);
    }
    int EstimateDamage ()
    {
        return 50;
    }
}