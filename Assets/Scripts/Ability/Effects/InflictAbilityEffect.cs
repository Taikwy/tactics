using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class InflictAbilityEffect : BaseAbilityEffect 
{
    public GameObject statusEffect;
	public string statusName;
	public int duration;

	//returns 0 cuz no damage
	public override int Predict (Tile target)
	{
        Debug.Log("predicting inflict " + statusName);
		return 0;
	}

    protected override int OnApply (Tile target){
        Debug.Log("new inflicting status " + statusName);
        Status status = target.content.GetComponent<Status>();
        status.Add(statusEffect);
        return 0;

        
		// Type statusType = Type.GetType(statusName);
		// if (statusType == null || !statusType.IsSubclassOf(typeof(StatusEffect))){
		// 	Debug.LogError("Invalid Status Type");
		// 	return 0;
		// }

		// MethodInfo mi = typeof(Status).GetMethod("Add");
		// Type[] types = new Type[]{ statusType, typeof(DurationStatusCondition) };
		// MethodInfo constructed = mi.MakeGenericMethod(types);

		// Status status = target.content.GetComponent<Status>();
		// object retValue = constructed.Invoke(status, null);

		// DurationStatusCondition condition = retValue as DurationStatusCondition;
		// condition.duration = duration;
		// return 0;
	}

	protected  int OldOnApply (Tile target){
        Debug.Log("inflicting status " + statusName);
		Type statusType = Type.GetType(statusName);
		if (statusType == null || !statusType.IsSubclassOf(typeof(StatusEffect))){
			Debug.LogError("Invalid Status Type");
			return 0;
		}

		MethodInfo mi = typeof(Status).GetMethod("Add");
		Type[] types = new Type[]{ statusType, typeof(DurationStatusCondition) };
		MethodInfo constructed = mi.MakeGenericMethod(types);

		Status status = target.content.GetComponent<Status>();
		object retValue = constructed.Invoke(status, null);

		DurationStatusCondition condition = retValue as DurationStatusCondition;
		condition.duration = duration;
		return 0;
	}
	
	protected override void OnPrimaryHit(object sender, object args){
	}
	protected override void OnPrimaryMiss(object sender, object args){}
}