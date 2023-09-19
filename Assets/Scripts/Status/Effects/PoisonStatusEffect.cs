using UnityEngine;
using System.Collections;

public class PoisonStatusEffect : StatusEffect 
{
	Unit owner;

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

	void OnNewTurn (object sender, object args)
	{
		Debug.Log("NEW TURN FOR POSION!!!");
		Stats s = GetComponentInParent<Stats>();
		int currentHP = s[StatTypes.HP] - 1;
		s.SetValue(StatTypes.HP, currentHP, false);


		// Stats s = GetComponentInParent<Stats>();
		// int currentHP = s[StatTypes.HP];
		// int maxHP = s[StatTypes.MHP];
		// int reduce = Mathf.Min(currentHP, Mathf.FloorToInt(maxHP * 0.1f));
		// s.SetValue(StatTypes.HP, (currentHP - reduce), false);
	}
}
