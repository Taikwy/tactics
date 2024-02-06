using UnityEngine;
using System.Collections;
using System.IO;

public class FlatDamageStatusEffect : StatusEffect 
{
	Unit owner;
    public int damage;

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
		Debug.Log("NEW TURN FOR " + statusName + " (FLAT DAMAGE)");
		Stats s = GetComponentInParent<Stats>();
		int currentHP = s[StatTypes.HP] - damage;
		s.SetValue(StatTypes.HP, currentHP, false);
	}
}
