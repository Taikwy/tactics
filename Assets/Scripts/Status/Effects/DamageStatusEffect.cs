using UnityEngine;
using System.Collections;
using System.IO;

public class DamageStatusEffect : StatusEffect 
{
	Unit owner;
    public int flatOrPercent;

	void OnEnable (){
		owner = GetComponentInParent<Unit>();
		if (owner)
			this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganEvent, owner);
	}

	void OnDisable (){
		this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganEvent, owner);
	}

	protected virtual void OnNewTurn (object sender, object args){}
}
