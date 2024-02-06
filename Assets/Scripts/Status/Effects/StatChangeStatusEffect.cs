using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeStatusEffect : StatusEffect 
{
	Stats myStats;
    public StatTypes statType;
    public int amountChanged;

	void OnEnable ()
	{
		myStats = GetComponentInParent<Stats>();
		// if (myStats)
		// 	this.AddObserver( OnCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), myStats );
	}

	void OnDisable ()
	{
		// this.RemoveObserver( OnCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), myStats );
	}

	void OnCounterWillChange (object sender, object args)
	{
		ValueChangeException exc = args as ValueChangeException;
		MultDeltaModifier m = new MultDeltaModifier(0, 2);
		exc.AddModifier(m);
	}
}