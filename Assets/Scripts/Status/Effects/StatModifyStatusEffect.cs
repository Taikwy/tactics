using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifyStatusEffect : StatusEffect 
{
	Unit owner;
	Stats myStats;
    public int modifyAmount;

	void OnEnable (){
		// owner = gameObject.transform.parent.GetComponent<Unit>();
		owner = GetComponentInParent<Unit>();
		// if (owner)
		// 	this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganEvent, owner);
		// Debug.Log("trying to find parent unit " + owner);
		if (myStats)
			this.AddObserver( OnCounterWillChange, Stats.WillChangeEvent(StatTypes.AV), myStats );
	}

	void OnDisable (){
		// this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganEvent, owner);
		this.RemoveObserver( OnCounterWillChange, Stats.WillChangeEvent(StatTypes.AV), myStats );
	}

	void OnCounterWillChange (object sender, object args){
		ValueChangeException exc = args as ValueChangeException;
		MultDeltaModifier m = new MultDeltaModifier(0, 2);
		exc.AddModifier(m);
	}
}
