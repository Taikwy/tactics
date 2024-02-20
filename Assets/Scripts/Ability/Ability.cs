using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour 
{
    public const string CanPerformCheck = "Ability.CanPerformCheck";
    public const string FailedNotification = "Ability.FailedNotification";
    public const string DidPerformNotification = "Ability.DidPerformNotification";
    public AbilityTypes type;
	public List<GameObject> primaryEffects = new List<GameObject>();
	public GameObject primaryEffect, primarySubEffect;
	public GameObject secondaryEffect, secondarySubEffect;
	[Space(1)]
    public EffectZone primaryEffectZone, primarySubEffectZone;
    public EffectZone secondaryEffectZone, secondarySubEffectZone;

	void Awake(){
		// Debug.Log("settingh primary effect");
		primaryEffect = primaryEffects[0];
	}
	public void SetOwner(){
		Unit owner = GetComponentInParent<Unit>();
		// Debug.Log("SETTING UP ABILITY " + owner);
		foreach(GameObject effect in primaryEffects){
			effect.GetComponent<BaseAbilityEffect>().owner = owner;
			foreach(BaseAbilityEffect subEffect in effect.GetComponent<BaseAbilityEffect>().subEffects){
				subEffect.owner = owner;
			}
		}
	}
	
    //replaced the logic i originally had in confirmtargetstate
    //may need to update this logic to accomdate more cmplex abilities with multiple component stuff
	public bool IsTarget (Tile tile){
		Transform obj = transform;
		for (int i = 0; i < obj.childCount; ++i){
			AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
			if (targeter.IsTarget(tile))
				return true;
		}
		return false;
	}

    //checks if the ability can be performed
	public bool CanPerform (){
		// Debug.Log("checkin if can perform");
        //the event here is posted in the skill points script, checks whether the unit has enough skill points for the given ability
		BaseException exc = new BaseException(true);
		this.PostEvent(CanPerformCheck, exc);
		return exc.toggle;
	}

	public void Perform (List<Tile> targets){
		if (!CanPerform()){
			this.PostEvent(FailedNotification);
			return;
		}
		
		for (int i = 0; i < targets.Count; ++i)
			Perform(targets[i]);

		this.PostEvent(DidPerformNotification);
	}

	void Perform (Tile target){
        //loops thru the ability effects
		// Debug.Log(transform.childCount + " children");
		// for (int i = 0; i < transform.childCount; ++i){
		// 	Debug.Log("perform looping " + i + " " + transform.childCount + " ");
		// 	Transform child = transform.GetChild(i);
		// 	BaseAbilityEffect effect = child.GetComponent<BaseAbilityEffect>();
		// 	effect.Apply(target);
		// }
		

		foreach(GameObject primaryEffect in primaryEffects){
			BaseAbilityEffect effect = primaryEffect.GetComponent<BaseAbilityEffect>();
			effect.Apply(target);
		}

		// BaseAbilityEffect effect = primaryEffect.GetComponent<BaseAbilityEffect>();
		// effect.Apply(target);

	}
}