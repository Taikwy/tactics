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
	Stats stats;
	
	int MinBP = 0;
	int deltaAV = 0;
	int turnBP = 4;
	int damagedBP = 3;
	public int focusBP = 5;
	
	void Awake (){
		stats = GetComponent<Stats>();
	}
	
	void OnEnable (){
		this.AddObserver(OnBPWillChange, Stats.WillChangeEvent(StatTypes.BP), stats);
		this.AddObserver(OnMBPDidChange, Stats.DidChangeEvent(StatTypes.MBP), stats);

		this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganEvent);
		this.AddObserver(OnAVDidChange, Stats.DidChangeEvent(StatTypes.AV), stats);
		this.AddObserver(OnHPDidChange, Stats.DidChangeEvent(StatTypes.HP), stats);
		this.AddObserver(OnAbilityHit, Ability.AbilityHitEvent);

	}
	
	void OnDisable (){
		this.RemoveObserver(OnBPWillChange, Stats.WillChangeEvent(StatTypes.BP), stats);
		this.RemoveObserver(OnMBPDidChange, Stats.DidChangeEvent(StatTypes.MBP), stats);

		this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganEvent);
		this.RemoveObserver(OnAVDidChange, Stats.DidChangeEvent(StatTypes.AV), stats);
		this.RemoveObserver(OnHPDidChange, Stats.DidChangeEvent(StatTypes.HP), stats);
		this.RemoveObserver(OnAbilityHit, Ability.AbilityHitEvent);

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
		Unit actor = sender as Unit;
		if(actor == gameObject.GetComponent<Unit>()){
			// Debug.Log("new turn, incrementing burst by " + turnBP + " sender: ");
			BP += turnBP;
		}
	}
	//gains 1 burst point for every 10 AV
	void OnAVDidChange(object sender, object args){
		MonoBehaviour obj = sender as MonoBehaviour;
		int oldAV = (int)args;
		if(obj!=null && obj.transform == gameObject.transform){
			// Debug.Log("sender " + obj+ " old val? " + oldAV+ " new val-" + stats[StatTypes.AV] + " old deltaA " + deltaAV);
			if(oldAV > stats[StatTypes.AV])
				deltaAV += oldAV - stats[StatTypes.AV];
			if(deltaAV >= 10){
				// Debug.Log("AV changed by " + deltaAV + ", incrementing burst by " + (deltaAV/10));
				BP += (deltaAV/10);
				deltaAV = deltaAV%10;
			}
		}
	}
	
	//25% of HP lost is bp gained, use division since its always single calculations and not a consistent thing like AV
	void OnHPDidChange (object sender, object args){
		MonoBehaviour obj = sender as MonoBehaviour;
		int oldHP = (int)args;
		if(obj!=null && obj.transform == gameObject.transform){
			int deltaHP = oldHP - stats[StatTypes.HP];
			//checks if the unit LOST HP
			if (deltaHP > 0){
				// Debug.Log(obj + " HP decreased, gaining burst");
				BP +=  damagedBP + deltaHP/4;
			}
		}
	}

	//when an ability's primary effect hits, u gain burst
	void OnAbilityHit(object sender, object args){
		MonoBehaviour obj = sender as MonoBehaviour;
		int burstGain = (int)args;
		// Debug.Log("ability sender " + obj.GetComponentInParent<Stats>().transform + " | unit gameobject " + gameObject.transform + " || " + (obj.GetComponentInParent<Stats>().transform == gameObject.transform));
		if(obj!=null && obj.GetComponentInParent<Stats>().transform == gameObject.transform){
			Debug.Log("gaining bp from ability hit " + burstGain);
			BP += burstGain;
		}
	}


}
