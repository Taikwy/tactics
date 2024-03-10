using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseAbilityEffect : MonoBehaviour
{
	protected const int minDamage = -999;
	protected const int maxDamage = 999;
	protected const int minHeal = -999;
	protected const int maxHeal = 999;

	public const string GetAttackEvent = "BaseAbilityEffect.GetAttackEvent";
	public const string GetDefenseEvent = "BaseAbilityEffect.GetDefenseEvent";
	public const string GetCritRateEvent = "BaseAbilityEffect.GetCritRareEvent";
	public const string GetCritDMGEvent = "BaseAbilityEffect.GetCritDMGEvent";
	// public const string GetPowerEvent = "BaseAbilityEffect.GetPowerEvent";
	// public const string TweakDamageEvent = "BaseAbilityEffect.TweakDamageEvent";

	public const string EffectHitEvent = "BaseAbilityEffect.EffectHitEvent";
	public const string EffectMissedEvent = "BaseAbilityEffect.EffectMissedEvent";
	public const string EffectCritEvent = "BaseAbilityEffect.EffectCritEvent";
	
	[Tooltip("Used for creating the status effect obj under a unit's status component")]public string abilityEffectName;
	[Tooltip("Shows when hovering the status effect under the status panel")][TextArea(5,20)]public string abilityEffectDescription;
    // public bool hasSubEffects;                    //this thing is mine
	// public BaseAbilityEffect subEffect;
	public List<BaseAbilityEffect> subEffects = new List<BaseAbilityEffect>();
	protected bool didCrit = false;
	[HideInInspector] public Unit owner;

	protected void OnEnable (){
		this.AddObserver(OnGetBaseCritRate, GetCritRateEvent);
		// this.AddObserver(OnGetBaseCritRate, StatusPanel.GetAttackEvent);
		this.AddObserver(OnGetBaseCritDMG, GetCritDMGEvent);
	}protected void OnDisable (){
		this.RemoveObserver(OnGetBaseCritRate, GetCritRateEvent);
		// this.RemoveObserver(OnGetBaseCritRate, StatusPanel.GetAttackEvent);
		this.RemoveObserver(OnGetBaseCritDMG, GetCritDMGEvent);
	}
	public abstract int Predict (Tile target);

    //handles applying the effect, checks if it can hit and raises events for the result
	public void Apply (Tile target){
		// Debug.Log("applying");
        //checks if the target can get affected by the ability or not
		if (GetComponent<AbilityEffectTarget>().IsTarget(target) == false)
			return;

		if (GetComponent<HitRate>().RollForHit(target)){
            // Debug.Log("HIT! " + abilityEffectName);
			//posts event with info for PERFORM ABILITY STATE - tile target, int damage/ability effect
			var info = new Info<Tile, int>(target, OnApply(target));
			this.PostEvent(EffectHitEvent, info);
			if(didCrit)
				this.PostEvent(EffectCritEvent, target);
			// this.PostEvent(EffectHitEvent, OnApply(target));
			foreach(BaseAbilityEffect effect in subEffects){
				effect.SubApply(target, didCrit);
			}
        }
		else{
            // Debug.Log("MISS! " + abilityEffectName);
			this.PostEvent(EffectMissedEvent, target);
			// this.PostEvent(EffectMissedEvent);
        }
	}
	public void SubApply (Tile target, bool crit){
		// Debug.Log("ub applying");
        //logic here for in case a unit is immune to a status effect or smt
		// if (GetComponent<AbilityEffectTarget>().IsTarget(target) == false)
		// 	return;

		if (GetComponent<HitRate>().RollForHit(target)){
            Debug.Log("SUB HIT! " + abilityEffectName);
			//posts event with info for PERFORM ABILITY STATE - tile target, int damage/ability effect
			var info = new Info<Tile, int>(target,  OnSubApply(target, crit));
			this.PostEvent(EffectHitEvent, info);
			// this.PostEvent(EffectHitEvent, OnSubApply(target, crit));
			foreach(BaseAbilityEffect effect in subEffects){
				effect.Apply(target);
			}
        }
		else{
            Debug.Log("MISS! " + abilityEffectName);
			this.PostEvent(EffectMissedEvent);
        }
	}

    //actually applies the effect, done in child classes
	protected abstract int OnApply (Tile target);
	//for sub effects
	protected abstract int OnSubApply (Tile target, bool crit);
	
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
    
	protected virtual int GetStatForCombat (Unit attacker, Unit target, string eventName, int startValue){
        // Debug.Log(eventName + " getting base stats for combat | starting val " + startValue);
		var modifiers = new List<ValueModifier>();															//list of all modifiers, INCLUDING base stat (ie unit's stat would jhsut be an addvaluemodifier with that stat)
		var info = new Info<Unit, Unit, List<ValueModifier>>(attacker, target, modifiers);
		this.PostEvent(eventName, info);																	//posts the event, power script auto adds the modifier for the base stat
		modifiers.Sort(Compare);
		
        // Debug.Log(modifiers.Count + " num modifers");
        //applies all the modifiers to the value
		float value = startValue;
		for (int i = 0; i < modifiers.Count; ++i){
			// Debug.Log(modifiers[i] + " add amt " + (modifiers[i] as AddValueModifier).toAdd);			
			value = modifiers[i].Modify(startValue, value);

		}
		
		// Debug.Log("middle modified value " + value);
        //floors value as an int and clamps within damage range
		int retValue = Mathf.FloorToInt(value);
		retValue = Mathf.Clamp(retValue, minDamage, maxDamage);
		// Debug.Log("final modified value " + retValue);
		return retValue;
	}
	
	void OnGetBaseCritRate (object sender, object args){
		// Debug.Log("getting crtit??");
		// MonoBehaviour obj = sender as MonoBehaviour;
		// Debug.Log("check crit rate is my effect " + obj + " | " + obj.transform.parent + " | " + transform);
		if (IsMyEffect(sender)){
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, GetComponentInParent<Stats>()[StatTypes.CR]) );
			// Debug.Log("on get base crit rate " + GetComponentInParent<Stats>()[StatTypes.CR]);
		}
	}

	void OnGetBaseCritDMG (object sender, object args){
		if (IsMyEffect(sender)){
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, GetComponentInParent<Stats>()[StatTypes.CD]) );
			// Debug.Log("on get base crit dmg " + GetComponentInParent<Stats>()[StatTypes.CD]);
		}
	}
	
	//checks if the sender of the event is on the same ability, ie other abilities and unit stats won't get called
	protected bool IsMyEffect (object sender){
		MonoBehaviour obj = sender as MonoBehaviour;
		// Debug.Log("check is my effect " + obj + " | " + obj.transform.parent + " | " + transform);
		// return obj != null && obj.transform.parent == transform.parent;
		return obj != null && obj.transform == transform;
	}

    //returns the modifier that should trigger first
	int Compare (ValueModifier x, ValueModifier y){
		return x.sortOrder.CompareTo(y.sortOrder);
	}
}