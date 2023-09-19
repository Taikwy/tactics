using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseAbilityPower : MonoBehaviour
{
	protected abstract int GetBaseAttack ();
	protected abstract int GetBaseDefense (Unit target);
	protected abstract int GetPower ();

	void OnEnable (){
		this.AddObserver(OnGetBaseAttack, BaseAbilityEffect.GetAttackEvent);
		this.AddObserver(OnGetBaseDefense, BaseAbilityEffect.GetDefenseEvent);
		this.AddObserver(OnGetPower, BaseAbilityEffect.GetPowerEvent);
	}

	void OnDisable (){
		this.RemoveObserver(OnGetBaseAttack, BaseAbilityEffect.GetAttackEvent);
		this.RemoveObserver(OnGetBaseDefense, BaseAbilityEffect.GetDefenseEvent);
		this.RemoveObserver(OnGetPower, BaseAbilityEffect.GetPowerEvent);
	}

	void OnGetBaseAttack (object sender, object args){
		if (IsMyEffect(sender)){
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, GetBaseAttack()) );
		}
	}

	void OnGetBaseDefense (object sender, object args){
		if (IsMyEffect(sender)){
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, GetBaseDefense(info.arg1)) );
		}
	}

	void OnGetPower (object sender, object args){
		if (IsMyEffect(sender)){
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, GetPower()) );
		}
	}

    //makes sure the sender of the event is the same ability object as this power script 
	bool IsMyEffect (object sender){
		MonoBehaviour obj = sender as MonoBehaviour;
		return (obj != null && obj.transform.parent == transform);
	}
}