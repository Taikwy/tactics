using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifyStatusEffect : StatusEffect 
{
	Unit owner;
	Stats myStats;
    public StatTypes statType;
    public int incrementOrMultiply;

	void OnEnable (){
		owner = GetComponentInParent<Unit>();
		// if (owner)
		// 	this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganEvent, owner);
		myStats = GetComponentInParent<Stats>();
		if (myStats)
			this.AddObserver( OnStatWillChange, Stats.WillChangeEvent(StatTypes.AV), myStats );
	}

	void OnDisable (){
		// this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganEvent, owner);
		this.RemoveObserver( OnStatWillChange, Stats.WillChangeEvent(StatTypes.AV), myStats );
	}

	protected virtual void OnStatWillChange (object sender, object args){}
}
