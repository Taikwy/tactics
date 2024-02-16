using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMultiplyStatusEffect : StatusEffect
{
    Unit owner;
	Stats myStats;
    public int multiplyAmount;

	void OnEnable (){
		if (myStats)
			this.AddObserver( OnCounterWillChange, Stats.WillChangeEvent(StatTypes.AV), myStats );
	}

	void OnDisable (){
		this.RemoveObserver( OnCounterWillChange, Stats.WillChangeEvent(StatTypes.AV), myStats );
	}

	void OnCounterWillChange (object sender, object args){
		ValueChangeException exc = args as ValueChangeException;
		MultValueModifier m = new MultValueModifier(0, multiplyAmount);
		exc.AddModifier(m);
	}
}
