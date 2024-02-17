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
				this.AddObserver(GetAttack, BaseAbilityEffect.GetAttackEvent);
				break;
			case StatTypes.DF:
				this.AddObserver(GetDefense, BaseAbilityEffect.GetDefenseEvent);
				break;
			case StatTypes.CR:
				this.AddObserver(GetCritRate, BaseAbilityEffect.GetCritRateEvent);
				break;
			case StatTypes.CD:
				this.AddObserver(GetCritDMG, BaseAbilityEffect.GetCritDMGEvent);
				break;
		}
	}protected void OnDisable (){
		switch(statType){
			case StatTypes.AT:
				this.RemoveObserver(GetAttack, BaseAbilityEffect.GetAttackEvent);
				break;
			case StatTypes.DF:
				this.RemoveObserver(GetDefense, BaseAbilityEffect.GetDefenseEvent);
				break;
			case StatTypes.CR:
				this.AddObserver(GetCritRate, BaseAbilityEffect.GetCritRateEvent);
				break;
			case StatTypes.CD:
				this.AddObserver(GetCritDMG, BaseAbilityEffect.GetCritDMGEvent);
				break;
		}
	}

	// void OnGetBaseDefense (object sender, object args){
	// 	if (IsMyEffect(sender)){
	// 		var info = args as Info<Unit, Unit, List<ValueModifier>>;
	// 		info.arg2.Add( new AddValueModifier(0, info.arg1.GetComponent<Stats>()[StatTypes.DF]) );
	// 		// Debug.Log("on get base defense " +  info.arg1.GetComponent<Stats>()[StatTypes.DF]);
	// 	}
	// }

	protected virtual void OnStatWillChange (object sender, object args){}

	protected virtual void GetAttack (object sender, object args){}
	protected virtual void GetDefense (object sender, object args){}
	protected virtual void GetCritRate (object sender, object args){}
	protected virtual void GetCritDMG (object sender, object args){}

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
