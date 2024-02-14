using UnityEngine;
using System.Collections;

public class DeadStatusEffect : StatusEffect
{
	Unit owner;
	Stats stats;
	
	void Awake (){
		owner = GetComponentInParent<Unit>();
		stats = owner.GetComponent<Stats>();
	}
	
	void OnEnable (){
		owner.transform.localScale = new Vector3(0.75f, 0.25f, 1f);
		this.AddObserver(OnTurnCheck, TurnOrderController.TurnCheckEvent, owner);
		this.AddObserver(OnAVWillChange, Stats.WillChangeEvent(StatTypes.AV), stats); 
	}
	
	void OnDisable (){
		owner.transform.localScale = Vector3.one;
		this.RemoveObserver(OnTurnCheck, TurnOrderController.TurnCheckEvent, owner);
		this.RemoveObserver(OnAVWillChange, Stats.WillChangeEvent(StatTypes.AV), stats);
	}
	
	void OnTurnCheck (object sender, object args){
		Debug.Log("on turn check");
		// Dont allow a dead unit to take turns
		// BaseException exc = args as BaseException;
		// if (exc.defaultToggle == true)
		// 	exc.FlipToggle();
	}
	
	void OnAVWillChange (object sender, object args){
		Debug.Log("on AV counter");
		// Dont allow a dead unit to increment the turn order counter
		// ValueChangeException exc = args as ValueChangeException;
		// if (exc.toValue > exc.fromValue)
		// 	exc.FlipToggle();
	}
}