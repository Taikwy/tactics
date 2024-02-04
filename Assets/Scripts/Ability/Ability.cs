using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour 
{
    public const string CanPerformCheck = "Ability.CanPerformCheck";
    public const string FailedNotification = "Ability.FailedNotification";
    public const string DidPerformNotification = "Ability.DidPerformNotification";
    public AbilityTypes type;
	public GameObject primaryEffect, primarySubEffect;
	public GameObject secondaryEffect, secondarySubEffect;
    public EffectZone primaryEffectZone, primarySubEffectZone;
    public EffectZone secondaryEffectZone, secondarySubEffectZone;
	
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
        //the event here is posted in the magic cost related scripts. i dont have them yet, im gonna replace them with energy or whatever other resource later
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
		for (int i = 0; i < transform.childCount; ++i){
			Transform child = transform.GetChild(i);
			BaseAbilityEffect effect = child.GetComponent<BaseAbilityEffect>();
			effect.Apply(target);
		}
	}
}