using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStatusEffect : StatusEffect 
{
	Unit owner;
	Stats myStats;
    public StatTypes statType;
    public float incrementOrMultiply;

	void OnEnable(){
        this.AddObserver(GetMove, BaseAbilityEffect.GetAttackEvent);
	}void OnDisable (){
        this.RemoveObserver(GetMove, BaseAbilityEffect.GetAttackEvent);
	}

	// void OnGetBaseDefense (object sender, object args){
	// 	if (IsMyEffect(sender)){
	// 		var info = args as Info<Unit, Unit, List<ValueModifier>>;
	// 		info.arg2.Add( new AddValueModifier(0, info.arg1.GetComponent<Stats>()[StatTypes.DF]) );
	// 		// Debug.Log("on get base defense " +  info.arg1.GetComponent<Stats>()[StatTypes.DF]);
	// 	}
	// }

	protected virtual void OnStatWillChange (object sender, object args){}

	protected virtual void GetMove (object sender, object args){}

	//checks if event's attacker is this unit
	protected bool IsMover (object sender, object args){
		BaseAbilityEffect obj = sender as BaseAbilityEffect;
		var info = args as Info<Unit, Unit, List<ValueModifier>>;
		Debug.Log("check is attacker | " + obj.owner + " | " + GetComponentInParent<Unit>() + "      | " + info.arg1);
		return obj != null && obj.owner == GetComponentInParent<Unit>();
	}
}
