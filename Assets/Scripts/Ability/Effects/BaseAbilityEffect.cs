using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseAbilityEffect : MonoBehaviour
{
	protected const int minDamage = -999;
	protected const int maxDamage = 999;

	public const string GetAttackEvent = "BaseAbilityEffect.GetAttackEvent";
	public const string GetDefenseEvent = "BaseAbilityEffect.GetDefenseEvent";
	public const string GetPowerEvent = "BaseAbilityEffect.GetPowerEvent";
	public const string TweakDamageEvent = "BaseAbilityEffect.TweakDamageEvent";

	public const string HitEvent = "BaseAbilityEffect.HitEvent";
	public const string MissedEvent = "BaseAbilityEffect.MissedEvent";
	
	public string abilityEffectName;
    // public bool hasSubEffects;                    //this thing is mine
	// public BaseAbilityEffect subEffect;
	public List<BaseAbilityEffect> subEffects = new List<BaseAbilityEffect>();

	void Awake(){
		//auto adds the sub effects into the script's list
		// foreach(Transform child in transform){
		// 	BaseAbilityEffect subEffect = child.gameObject.GetComponent<BaseAbilityEffect>();
		// 	if(subEffect != null)
		// 		subEffects.Add(subEffect);
		// }
	}

	//needs a new way to handle status effect to show in forecast
	public abstract int Predict (Tile target);

    //handles applying the effect, checks if it can hit and raises events for the result
	public void Apply (Tile target){
		// Debug.Log("applying");
        //checks if the target can get affected by the ability or not
		if (GetComponent<AbilityEffectTarget>().IsTarget(target) == false)
			return;

		if (GetComponent<HitRate>().RollForHit(target)){
            // Debug.Log("HIT! " + abilityEffectName);
			this.PostEvent(HitEvent, OnApply(target));
			foreach(BaseAbilityEffect effect in subEffects){
				effect.SubApply(target);
			}
        }
		else{
            Debug.Log("MISS! " + abilityEffectName);
			this.PostEvent(MissedEvent);
        }
	}
	public void SubApply (Tile target){
		// Debug.Log("ub applying");
        //logic here for in case a unit is immune to a status effect or smt
		// if (GetComponent<AbilityEffectTarget>().IsTarget(target) == false)
		// 	return;

		if (GetComponent<HitRate>().RollForHit(target)){
            Debug.Log("HIT! " + abilityEffectName);
			this.PostEvent(HitEvent, OnApply(target));
			foreach(BaseAbilityEffect effect in subEffects){
				effect.Apply(target);
			}
        }
		else{
            Debug.Log("MISS! " + abilityEffectName);
			this.PostEvent(MissedEvent);
        }
	}

    //actually applies the effect, done in child classes
	protected abstract int OnApply (Tile target);
	
	//enables methods relying on primary effect hitting if this effect is a sub effect
	// public void OnEnable(){
	// 	if(!hasSubEffects)
	// 		return;
	// 	this.AddObserver(OnPrimaryHit, HitEvent, primaryEffect);
    // 	this.AddObserver(OnPrimaryMiss, MissedEvent, primaryEffect);
	// }
	// public void OnDisable(){
	// 	if(!hasSubEffects)
	// 		return;
	// 	this.RemoveObserver(OnPrimaryHit, HitEvent, primaryEffect);
    // 	this.RemoveObserver(OnPrimaryMiss, MissedEvent, primaryEffect);
	// }

	protected abstract void OnPrimaryHit(object sender, object args);
	protected abstract void OnPrimaryMiss(object sender, object args);
    //
	protected virtual int GetStat (Unit attacker, Unit target, string eventName, int startValue){
        Debug.Log("getting base stats");
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
}