using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseAbilityPercentModifier : MonoBehaviour
{
	public int percentModifer = 0;
	protected int GetBaseAttack (){
        return GetComponentInParent<Stats>()[StatTypes.AT];
    }
	protected int GetBaseDefense (Unit target){
        return target.GetComponent<Stats>()[StatTypes.DF];
    }
	protected int GetPercent (){
		return percentModifer;
	}

	void OnEnable (){
		this.AddObserver(OnGetBaseAttack, BaseAbilityEffect.GetAttackEvent);
		this.AddObserver(OnGetBaseDefense, BaseAbilityEffect.GetDefenseEvent);
		this.AddObserver(OnGetPercent, BaseAbilityEffect.GetPowerEvent);
	}

	void OnDisable (){
		this.RemoveObserver(OnGetBaseAttack, BaseAbilityEffect.GetAttackEvent);
		this.RemoveObserver(OnGetBaseDefense, BaseAbilityEffect.GetDefenseEvent);
		this.RemoveObserver(OnGetPercent, BaseAbilityEffect.GetPowerEvent);
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

	void OnGetPercent (object sender, object args){
		if (IsMyEffect(sender)){
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, GetPercent()) );
		}
	}

    //makes sure the sender of the event is the same ability object as this power script 
	bool IsMyEffect (object sender){
		MonoBehaviour obj = sender as MonoBehaviour;
		return (obj != null && obj.transform.parent == transform);
	}
}