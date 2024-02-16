using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseVictoryCondition : MonoBehaviour
{
	public Alliances Victor{
		get { return victor; } 
		protected set { victor = value; }
	}
	Alliances victor = Alliances.None;
	
	protected BattleController bc;
	
	protected virtual void Awake (){
		bc = GetComponent<BattleController>();
	}	
	protected virtual void OnEnable (){
		this.AddObserver(OnTurnDidCompleteNotification, TurnOrderController.RoundEndedEvent);
		this.AddObserver(OnHPDidChangeNotification, Stats.DidChangeEvent(StatTypes.HP));
	}	
	protected virtual void OnDisable (){
		this.RemoveObserver(OnTurnDidCompleteNotification, TurnOrderController.RoundEndedEvent);
		this.RemoveObserver(OnHPDidChangeNotification, Stats.DidChangeEvent(StatTypes.HP));
	}
	protected virtual void OnHPDidChangeNotification (object sender, object args){
		CheckForGameOver();
	}
	protected virtual void OnTurnDidCompleteNotification (object sender, object args){
		CheckForGameOver();
	}
	
	protected virtual void CheckForGameOver (){
		if (PartyDefeated(Alliances.Ally)){
			Victor = Alliances.Enemy;
			Debug.Log("PARTY DEFEATED!! :(");
		}
	}
	
	protected virtual bool PartyDefeated (Alliances type){
		for (int i = 0; i < bc.units.Count; ++i){
			Debug.Log("units numb " + bc.units.Count);
			Alliance a = bc.units[i].GetComponent<Alliance>();
			if (a == null)
				continue;
			
			if (a.type == type && !IsDefeated(bc.units[i]))
				return false;
		}
		return true;
	}
	
	protected virtual bool IsDefeated (Unit unit){
		Health health = unit.GetComponent<Health>();
		if (health){
			Debug.Log(unit.name + " health " + health.MinHP + " - " + health.HP);
			return health.MinHP == health.HP;
		}
		
		Stats stats = unit.GetComponent<Stats>();
		return stats[StatTypes.HP] == 0;
	}
}