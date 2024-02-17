using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMultiplyStatusEffect : StatusEffect
{
	Stats myStats;
	StatTypes type;
    public int multiplyAmount;

	void OnEnable (){
		myStats = GetComponentInParent<Stats>();
		if (myStats)
			this.AddObserver( OnStatWillChange, Stats.WillChangeEvent(type), myStats );
	}

	void OnDisable (){
		this.RemoveObserver( OnStatWillChange, Stats.WillChangeEvent(type), myStats );
	}

	void OnStatWillChange (object sender, object args){
		ValueChangeException exc = args as ValueChangeException;
		MultValueModifier m = new MultValueModifier(0, multiplyAmount);
		exc.AddModifier(m);
	}
}
