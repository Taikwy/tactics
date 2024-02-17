using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifyStatusEffect : StatusEffect 
{
	Unit owner;
	Stats myStats;
    public StatTypes statType;
    public float incrementOrMultiply;

	public void AddObservers(){
		switch(statType){
			case StatTypes.AT:
				this.AddObserver(OnGetAttack, BaseAbilityEffect.GetAttackEvent);
				break;
			case StatTypes.DF:
				this.AddObserver(OnGetDefense, BaseAbilityEffect.GetDefenseEvent);
				break;
			case StatTypes.CR:
				this.AddObserver(OnGetCritRate, BaseAbilityEffect.GetCritRateEvent);
				break;
			case StatTypes.CD:
				this.AddObserver(OnGetCritDMG, BaseAbilityEffect.GetCritDMGEvent);
				break;
		}
	}protected void OnDisable (){
		switch(statType){
			case StatTypes.AT:
				this.RemoveObserver(OnGetAttack, BaseAbilityEffect.GetAttackEvent);
				break;
			case StatTypes.DF:
				this.RemoveObserver(OnGetDefense, BaseAbilityEffect.GetDefenseEvent);
				break;
			case StatTypes.CR:
				this.AddObserver(OnGetCritRate, BaseAbilityEffect.GetCritRateEvent);
				break;
			case StatTypes.CD:
				this.AddObserver(OnGetCritDMG, BaseAbilityEffect.GetCritDMGEvent);
				break;
		}
	}

	protected virtual void OnGetAttack (object sender, object args){}
	protected virtual void OnGetDefense (object sender, object args){}
	protected virtual void OnGetCritRate (object sender, object args){}
	protected virtual void OnGetCritDMG (object sender, object args){}

	//checks if event's attacker is this unit
	protected bool IsAttacker (object sender, object args){
		BaseAbilityEffect obj = sender as BaseAbilityEffect;
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
		Debug.Log("check is attacker | " + obj.owner + " | " + GetComponentInParent<Unit>() + "      | " + info.arg1);
		return obj != null && obj.owner == GetComponentInParent<Unit>();
	}
	//checks if event's target is this unit
	protected bool IsDefender (object sender, object args){
		BaseAbilityEffect obj = sender as BaseAbilityEffect;
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
		Debug.Log("check is defender | " + obj.owner + " | " + GetComponentInParent<Unit>() + "      | " + info.arg1);
		return obj != null && info.arg1 == GetComponentInParent<Unit>();
	}
}
