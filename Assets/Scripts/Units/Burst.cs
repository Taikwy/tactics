using UnityEngine;
using System.Collections;

public class Burst : MonoBehaviour 
{
	public int BP{
		get { return stats[StatTypes.BP]; }
		set { stats[StatTypes.BP] = value; }
	}
	
	public int MBP{
		get { return stats[StatTypes.MBP]; }
		set { stats[StatTypes.MBP] = value; }
	}
	
	int MinBP = 0;
	int turnBP = 4;
	Stats stats;
	
	void Awake (){
		stats = GetComponent<Stats>();
	}
	
	void OnEnable (){
		this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganEvent);

		this.AddObserver(OnBPWillChange, Stats.WillChangeEvent(StatTypes.BP), stats);
		this.AddObserver(OnMBPDidChange, Stats.DidChangeEvent(StatTypes.MBP), stats);
	}
	
	void OnDisable (){
		this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganEvent);

		this.RemoveObserver(OnBPWillChange, Stats.WillChangeEvent(StatTypes.BP), stats);
		this.RemoveObserver(OnMBPDidChange, Stats.DidChangeEvent(StatTypes.MBP), stats);
	}
	void TurnStartBP(){}
	void OnBPWillChange (object sender, object args){
		ValueChangeException vce = args as ValueChangeException;
		vce.AddModifier(new ClampValueModifier(int.MaxValue, MinBP, stats[StatTypes.MBP]));
	}
	
	void OnMBPDidChange (object sender, object args){
		int oldMBP = (int)args;
        //increases current health by how much max hp adds
		if (MBP > oldMBP)
			BP += MBP - oldMBP;
		else
			BP = Mathf.Clamp(BP, MinBP, MBP);
	}

	void OnNewTurn(object sender, object args){
		// Debug.Log("new turn, incrementing burst " + turnBP);
		BP += turnBP;
	}
	void OnAVDidChange(object sender, object args){}


}
