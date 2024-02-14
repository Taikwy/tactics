using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifyStatusEffect : StatusEffect 
{
	Stats myStats;
    public int incrementAmount;

	void OnEnable (){
		myStats = GetComponentInParent<Stats>();
		if (myStats)
			this.AddObserver( OnAVWillChange, Stats.WillChangeEvent(StatTypes.AV), myStats );
	}

	void OnDisable (){
		// this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganEvent, owner);
		this.RemoveObserver( OnAVWillChange, Stats.WillChangeEvent(StatTypes.AV), myStats );
	}

	void OnAVWillChange (object sender, object args){
		ValueChangeException exc = args as ValueChangeException;
		AddValueModifier m = new AddValueModifier(0, incrementAmount);
		exc.AddModifier(m);
	}
}
