using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
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
				this.AddObserver(OnGetAttack, StatusPanel.GetAttackEvent);
				break;
			case StatTypes.DF:
				this.AddObserver(OnGetDefense, BaseAbilityEffect.GetDefenseEvent);
				this.AddObserver(OnGetDefense, StatusPanel.GetDefenseEvent);
				break;
			case StatTypes.CR:
				this.AddObserver(OnGetCritRate, BaseAbilityEffect.GetCritRateEvent);
				this.AddObserver(OnGetCritRate, StatusPanel.GetCritRateEvent);
				break;
			case StatTypes.CD:
				this.AddObserver(OnGetCritDMG, BaseAbilityEffect.GetCritDMGEvent);
				this.AddObserver(OnGetCritDMG, StatusPanel.GetCritDMGEvent);
				break;
			case StatTypes.SP:
				this.AddObserver(OnGetSpeed, TurnOrderController.GetSpeedEvent);
				this.AddObserver(OnGetSpeed, StatusPanel.GetSpeedEvent);

				// GetComponentInParent<Unit>().PostEvent(TurnOrderController.SpeedChangedEvent);
				this.PostEvent(TurnOrderController.SpeedChangedEvent);								//extra thing here cuz the moment a unit's speed gets changed, their turn order also changes
				break;
			default:
				Debug.LogError("Invalid Stat Type for modification");
				break;
		}
	}protected void OnDisable (){
		switch(statType){
			case StatTypes.AT:
				this.RemoveObserver(OnGetAttack, BaseAbilityEffect.GetAttackEvent);
				this.RemoveObserver(OnGetAttack, StatusPanel.GetAttackEvent);
				break;
			case StatTypes.DF:
				this.RemoveObserver(OnGetDefense, BaseAbilityEffect.GetDefenseEvent);
				this.RemoveObserver(OnGetDefense, StatusPanel.GetDefenseEvent);
				break;
			case StatTypes.CR:
				this.RemoveObserver(OnGetCritRate, BaseAbilityEffect.GetCritRateEvent);
				this.RemoveObserver(OnGetCritRate, StatusPanel.GetCritRateEvent);
				break;
			case StatTypes.CD:
				this.RemoveObserver(OnGetCritDMG, BaseAbilityEffect.GetCritDMGEvent);
				this.RemoveObserver(OnGetCritDMG, StatusPanel.GetCritDMGEvent);
				break;
			case StatTypes.SP:
				this.RemoveObserver(OnGetSpeed, TurnOrderController.GetSpeedEvent);
				this.RemoveObserver(OnGetSpeed, StatusPanel.GetSpeedEvent);
				break;
			default:
				Debug.LogError("Invalid Stat Type for modification");
				break;
		}
	}

	protected virtual void OnGetAttack (object sender, object args){}
	protected virtual void OnGetDefense (object sender, object args){}
	protected virtual void OnGetCritRate (object sender, object args){}
	protected virtual void OnGetCritDMG (object sender, object args){}
	protected virtual void OnGetSpeed (object sender, object args){}

	//checks if event's attacker is this unit
	protected bool IsAttacker (object sender, object args){
		BaseAbilityEffect obj = sender as BaseAbilityEffect;
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
		// Debug.Log("check is attacker | " + obj.owner + " | " + GetComponentInParent<Unit>() + "      | " + info.arg1);
		return obj != null && obj.owner == GetComponentInParent<Unit>();
	}
	//checks if event's target is this unit
	protected bool IsDefender (object sender, object args){
		BaseAbilityEffect obj = sender as BaseAbilityEffect;
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
		// Debug.Log("check is defender | " + obj.owner + " | " + GetComponentInParent<Unit>() + "      | " + info.arg1);
		return obj != null && info.arg1 == GetComponentInParent<Unit>();
	}
	//used for status info panel
	protected bool IsActor(object sender, object args){
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
		if(info.arg0 == GetComponentInParent<Unit>())
			return true;
		return false;
	}
}
