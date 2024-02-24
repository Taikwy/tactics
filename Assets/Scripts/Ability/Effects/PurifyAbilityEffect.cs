using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurifyAbilityEffect : BaseAbilityEffect 
{

	void Start(){
		abilityEffectName = "PURIFY";
	}
	void OnEnable (){
		base.OnEnable();
	}void OnDisable (){
		base.OnEnable();
	}

	public override int Predict (Tile target){
		return 0;
	}
	
	protected override int OnApply (Tile target){
		Debug.Log("PURIFYING");
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

        defender.GetComponent<Status>().RemoveAll();

		return 0;
	}
	protected override int OnSubApply (Tile target, bool crit){
		Debug.Log("SUB PURIFYING");
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

        defender.GetComponent<Status>().RemoveAll();

		return 0;
	}

	protected override void OnPrimaryHit(object sender, object args){}
	protected override void OnPrimaryMiss(object sender, object args){}
	
}