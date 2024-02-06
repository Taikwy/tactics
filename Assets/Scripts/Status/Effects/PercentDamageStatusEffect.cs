using UnityEngine;
using System.Collections;
using System.IO;

public class PercentDamageStatusEffect : StatusEffect 
{
	Unit owner;
    public float percent;

	void OnEnable ()
	{
		owner = gameObject.transform.parent.GetComponent<Unit>();
		// owner = GetComponentInParent<Unit>();
		if (owner)
			this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganEvent, owner);
		// Debug.Log("trying to find parent unit " + owner);
	}

	void OnDisable ()
	{
		this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganEvent, owner);
	}

	void OnNewTurn (object sender, object args){
		Debug.Log("NEW TURN FOR " + statusName + " (PERCENT DAMAGE)");
		Stats s = GetComponentInParent<Stats>();
		int currentHP = s[StatTypes.HP] - (int)(s[StatTypes.HP]*percent);
		s.SetValue(StatTypes.HP, currentHP, false);
	}
}
