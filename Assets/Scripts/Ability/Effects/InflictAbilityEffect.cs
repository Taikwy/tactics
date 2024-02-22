using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class InflictAbilityEffect : BaseAbilityEffect 
{
    // public GameObject statusEffect;
	[Header("Status Types (Script Names)")]
	[Tooltip("StatIncrementStatusEffect || StatMultiplyStatusEffect")]public string effectName;
	public string conditionName;
	[Header("Status Condition")]
	public int duration;
	[Header("Damage Status Effect Numbers")]
	[Tooltip("int flat damage or for percent dmg (out of 100)")]public int flatOrPercent;
	[Header("Modification Status Effect Numbers")]
	[Tooltip("stat type to modify")]public StatTypes statType;
	[Tooltip("stat change increment (INT) or multiply (FLOAT)")] public float incrementOrMultiply;
	// public int flatDMG;
	// public int percentDMG;
	// public int statChange;

	//returns 0 cuz no damage
	public override int Predict (Tile target){
        Debug.Log("predicting inflict " + effectName);
		return 0;
	}

	protected override int OnApply (Tile target){
        // Debug.Log("new inflicting status " + effectName);
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

		int adjustedDuration = duration;
		float critRate = GetStatForCombat(attacker, defender, GetCritRateEvent, 0);
		float critDMG =  GetStatForCombat(attacker, defender, GetCritDMGEvent, 0);
		if(UnityEngine.Random.Range(0, 101) <= critRate){
			didCrit = true;
			int critDuration = (int) Mathf.Ceil( critDMG/100f* duration);
			adjustedDuration += critDuration;
			Debug.Log("CRIT!!! " + critRate + " | " + critDMG + " incrased duratioin: " + adjustedDuration);
		}
		else
			didCrit = false;
		
		CreateStatusObject(target, adjustedDuration);
		return 0;
	}
	protected override int OnSubApply (Tile target, bool crit){
        // Debug.Log("new inflicting SUB status " + effectName);
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

		//if this is a sub effect, it will crit depending on whether its parent effect critted
		int adjustedDuration = duration;
		float critDMG =  GetStatForCombat(attacker, defender, GetCritDMGEvent, 0);
		if(crit){
			int critDuration = (int) Mathf.Ceil( critDMG/100f* duration);
			adjustedDuration += critDuration;
			Debug.Log("PARENT ABILITY CRITED!!! " + crit + " | " + critDMG + " incrased duratioin: " + adjustedDuration);
		}
		CreateStatusObject(target, adjustedDuration);

		return 0;
	}
	
	int CreateStatusObject(Tile target, int adjustedDuration){
		//checks if there is a calss with the statusname
		Type effectType = Type.GetType(effectName);
		Type conditionType = Type.GetType(conditionName);
		if (effectType == null || !effectType.IsSubclassOf(typeof(StatusEffect))){
			Debug.LogError("Invalid Status Effect Type");
			return 0;
		}
		if (conditionType == null || !conditionType.IsSubclassOf(typeof(StatusCondition))){
			Debug.LogError("Invalid Status Condition Type");
			return 0;
		}

		//mi is status.add
		MethodInfo mi = typeof(Status).GetMethod("Add");
		Type[] types = new Type[]{ effectType, conditionType };
		MethodInfo constructed = mi.MakeGenericMethod(types);

		Status status = target.content.GetComponent<Status>();
		GameObject statusObj = constructed.Invoke(status, null) as GameObject;
		statusObj.name = abilityEffectName;

		StatusEffect effect = statusObj.GetComponent<StatusEffect>();
		effect.statusName = abilityEffectName;
		effect.statusEffectDescription = abilityEffectDescription;
		StatusCondition condition = statusObj.GetComponent<StatusCondition>();
		
		
		switch(condition){
			case RoundDurationStatusCondition:
				(condition as RoundDurationStatusCondition).duration = adjustedDuration;
				break;
			case TurnDurationStatusCondition:
				(condition as TurnDurationStatusCondition).duration = adjustedDuration;
				break;
			case InfiniteStatusCondition:
				break;
		}

		switch(effect){
			case DamageStatusEffect:
				(effect as DamageStatusEffect).flatOrPercent = flatOrPercent;
				break;
			// case FlatDamageStatusEffect:
			// 	(effect as FlatDamageStatusEffect).flatOrPercent = flatDMG;
			// 	break;
			// case PercentDamageStatusEffect:
			// 	(effect as PercentDamageStatusEffect).percent = percentDMG;
			// 	break;
			case StatModifyStatusEffect:
			Debug.Log(statType);
				(effect as StatModifyStatusEffect).incrementOrMultiply = incrementOrMultiply;
				(effect as StatModifyStatusEffect).statType = statType;
				(effect as StatModifyStatusEffect).AddObservers();
				break;
		}

		return 0;
	}

	protected override void OnPrimaryHit(object sender, object args){
	}
	protected override void OnPrimaryMiss(object sender, object args){}
}