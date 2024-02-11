using UnityEngine;
using System.Collections;

public class AutoStatusController : MonoBehaviour 
{
	void OnEnable (){
		this.AddObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
	}
	
	void OnDisable (){
		this.RemoveObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
	}
	
	void OnHPDidChangeNotification (object sender, object args){
		Stats stats = sender as Stats;
		if (stats[StatTypes.HP] == 0){
			Status status = stats.GetComponentInChildren<Status>();
            StatComparisonCondition statComparisonCondition = status.Add<DeadStatusEffect, StatComparisonCondition>().GetComponent<StatComparisonCondition>();
            statComparisonCondition.Setup(StatTypes.HP, 0, statComparisonCondition.EqualTo);
            Debug.Log("hp has reached 0");
		}
	}
}