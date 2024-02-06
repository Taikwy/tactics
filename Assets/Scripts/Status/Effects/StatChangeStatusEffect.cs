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
		if (myStats)
			this.AddObserver( OnCounterWillChange, Stats.WillChangeNotification(statType), myStats );
	}

	void OnDisable ()
	{
		this.RemoveObserver( OnCounterWillChange, Stats.WillChangeNotification(statType), myStats );
	}

	void OnCounterWillChange (object sender, object args)
	{
		ValueChangeException exc = args as ValueChangeException;
		AddValueModifier a = new AddValueModifier(0, amountChanged);
		exc.AddModifier(a);
	}
}