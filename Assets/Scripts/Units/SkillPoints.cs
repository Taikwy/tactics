using UnityEngine;
using System.Collections;

public class SkillPoints : MonoBehaviour 
{
	public int SK{
		get { return stats[StatTypes.SK]; }
		set { stats[StatTypes.SK] = value; }
	}
	
	public int MSK{
		get { return stats[StatTypes.MSK]; }
		set { stats[StatTypes.MSK] = value; }
	}
	int MinSK = 0;
	Stats stats;
	
	void Awake (){
		stats = GetComponent<Stats>();
	}
	
	void OnEnable (){
		this.AddObserver(OnSKWillChange, Stats.WillChangeEvent(StatTypes.SK), stats);
		this.AddObserver(OnMSKDidChange, Stats.DidChangeEvent(StatTypes.MSK), stats);
	}
	
	void OnDisable (){
		this.RemoveObserver(OnSKWillChange, Stats.WillChangeEvent(StatTypes.SK), stats);
		this.RemoveObserver(OnMSKDidChange, Stats.DidChangeEvent(StatTypes.MSK), stats);
	}
	
	void OnSKWillChange (object sender, object args){
        Debug.Log("clamping skill points");
		ValueChangeException vce = args as ValueChangeException;
		vce.AddModifier(new ClampValueModifier(int.MaxValue, MinSK, stats[StatTypes.MHP]));
	}
	
	void OnMSKDidChange (object sender, object args){
		int oldMSK = (int)args;
        //increases current health by how much max hp adds
		if (MSK > oldMSK)
			SK += MSK - oldMSK;
		else
			SK = Mathf.Clamp(SK, MinSK, MSK);
	}
}
