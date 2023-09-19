using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityEffect : MonoBehaviour
{
    #region Consts & Events
	protected const int minDamage = -999;
	protected const int maxDamage = 999;

	public const string GetAttackEvent = "BaseAbilityEffect.GetAttackEvent";
	public const string GetDefenseEvent = "BaseAbilityEffect.GetDefenseEvent";
	public const string GetPowerEvent = "BaseAbilityEffect.GetPowerEvent";
	public const string TweakDamageEvent = "BaseAbilityEffect.TweakDamageEvent";

	public const string MissedEvent = "BaseAbilityEffect.MissedEvent";
	public const string HitEvent = "BaseAbilityEffect.HitEvent";
	#endregion

	#region Public
    public bool isSubEffect;                    //this thing is mine
	public abstract int Predict (Tile target);

    //handles applying the effect, checks if it can hit and raises events for the result
	public void Apply (Tile target){
        //checks if the target can get affected by the ability or not
		if (GetComponent<AbilityEffectTarget>().IsTarget(target) == false)
			return;

		if (GetComponent<HitRate>().RollForHit(target)){
            Debug.Log("HIT!");
			this.PostEvent(HitEvent, OnApply(target));
        }
		else{
            Debug.Log("MISS!");
			this.PostEvent(MissedEvent);
        }
	}
	#endregion

	#region Protected
    //actually applies the effect, done in child classes
	protected abstract int OnApply (Tile target);

    //
	protected virtual int GetStat (Unit attacker, Unit target, string eventName, int startValue){
        // Debug.Log("getting base stats");
		var modifiers = new List<ValueModifier>();
		var info = new Info<Unit, Unit, List<ValueModifier>>(attacker, target, modifiers);
		this.PostEvent(eventName, info);
        //sorts all the modifiers based on sort order
		modifiers.Sort(Compare);
		
        //applies all the modifiers to the value
		float value = startValue;
		for (int i = 0; i < modifiers.Count; ++i)
			value = modifiers[i].Modify(startValue, value);
		
        //floors value as an int and clamps within damage range
		int retValue = Mathf.FloorToInt(value);
		retValue = Mathf.Clamp(retValue, minDamage, maxDamage);
		return retValue;
	}

    //returns the modifier that should trigger first
	int Compare (ValueModifier x, ValueModifier y){
		return x.sortOrder.CompareTo(y.sortOrder);
	}
	#endregion
}