using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class InflictAbilityEffect : BaseAbilityEffect 
{
    // public GameObject statusEffect;
	[Header("Ailment Types (Class Names)")]
	public string effectName;
	public string conditionName;
	[Header("Ailment Condition")]
	public int duration;
	[Header("Ailment Effect Numbers")]
	[Tooltip("int flat damage or for percent dmg (out of 100)")]public int flatOrPercent;
	public int flatDMG;
	public int percentDMG;
	public int statChange;

	//returns 0 cuz no damage
	public override int Predict (Tile target)
	{
        Debug.Log("predicting inflict " + effectName);
		return 0;
	}

    // protected int NewOnApply (Tile target){
    //     Debug.Log("new inflicting status " + effectName);
    //     Status status = target.content.GetComponent<Status>();
    //     // status.Add(statusEffect);
    //     return 0;
	// }

	protected override int OnApply (Tile target){
        Debug.Log("new inflicting status " + effectName);
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

		//crit rate stuff
		int adjustedDuration = duration;
		float critRate = GetComponentInParent<Unit>().GetComponentInParent<Stats>()[StatTypes.CR];
		float critDMG = GetComponentInParent<Unit>().GetComponentInParent<Stats>()[StatTypes.CD];
		if(UnityEngine.Random.Range(0, 101) <= critRate){
			adjustedDuration += (int)Mathf.Ceil(critDMG/100f) * duration;
			Debug.Log("CRIT!!! " + critRate + " incrased duratioin " + ((int)Mathf.Ceil(critDMG/100f) * duration));
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
			case StatChangeStatusEffect:
				(effect as StatChangeStatusEffect).amountChanged = statChange;
				break;
		}

		return 0;
	}
	
	protected override void OnPrimaryHit(object sender, object args){
	}
	protected override void OnPrimaryMiss(object sender, object args){}
}