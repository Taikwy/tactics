using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class InflictAbilityEffect : BaseAbilityEffect 
{
    public GameObject statusEffect;
	public string effectName, conditionName;
	[Space(2)][Header("Condition stuff")]
	public int duration;
	[Space(2)][Header("Effect stuff")]
	public int flatDMG;
	public int percentDMG;
	public int statChange;

	//returns 0 cuz no damage
	public override int Predict (Tile target)
	{
        Debug.Log("predicting inflict " + effectName);
		return 0;
	}

    protected int NewOnApply (Tile target){
        Debug.Log("new inflicting status " + effectName);
        Status status = target.content.GetComponent<Status>();
        // status.Add(statusEffect);
        return 0;

	}

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

		//mi is status.add
		MethodInfo mi = typeof(Status).GetMethod("Add");
		Type[] types = new Type[]{ effectType, conditionType };
		MethodInfo constructed = mi.MakeGenericMethod(types);

		Status status = target.content.GetComponent<Status>();
		GameObject statusObj = constructed.Invoke(status, null) as GameObject;
		statusObj.name = abilityEffectName;

		StatusEffect effect = statusObj.GetComponent<StatusEffect>();
		StatusCondition condition = statusObj.GetComponent<StatusCondition>();
		
		switch(condition){
			case GlobalDurationStatusCondition:
				(condition as GlobalDurationStatusCondition).duration = duration;
				break;
			case UnitDurationStatusCondition:
				(condition as UnitDurationStatusCondition).duration = duration;
				break;
			case InfiniteStatusCondition:
				break;
		}

		switch(effect){
			case FlatDamageStatusEffect:
				(effect as FlatDamageStatusEffect).damage = flatDMG;
				break;
			case PercentDamageStatusEffect:
				(effect as PercentDamageStatusEffect).percent = percentDMG;
				break;
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