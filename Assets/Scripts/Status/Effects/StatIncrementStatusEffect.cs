using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatIncrementStatusEffect : StatusEffect 
{
	Stats myStats;
	StatTypes type;
    public int incrementAmount;

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
		AddValueModifier m = new AddValueModifier(0, incrementAmount);
		exc.AddModifier(m);
	}
}
