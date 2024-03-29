using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	public int HP{
		get { return stats[StatTypes.HP]; }
		set { stats[StatTypes.HP] = value; }
	}
	
	public int MHP{
		get { return stats[StatTypes.MHP]; }
		set { stats[StatTypes.MHP] = value; }
	}
	
	public int MinHP = 0;
	Stats stats;
	
	void Awake (){
		stats = GetComponent<Stats>();
	}
	
	void OnEnable (){
		this.AddObserver(OnHPWillChange, Stats.WillChangeEvent(StatTypes.HP), stats);
		this.AddObserver(OnMHPDidChange, Stats.DidChangeEvent(StatTypes.MHP), stats);
	}
	
	void OnDisable (){
		this.RemoveObserver(OnHPWillChange, Stats.WillChangeEvent(StatTypes.HP), stats);
		this.RemoveObserver(OnMHPDidChange, Stats.DidChangeEvent(StatTypes.MHP), stats);
	}
	
	void OnHPWillChange (object sender, object args){
		ValueChangeException vce = args as ValueChangeException;
		vce.AddModifier(new ClampValueModifier(int.MaxValue, MinHP, stats[StatTypes.MHP]));
	}
	
	void OnMHPDidChange (object sender, object args){
		int oldMHP = (int)args;
        //increases current health by how much max hp adds
		if (MHP > oldMHP)
			HP += MHP - oldMHP;
		else
			HP = Mathf.Clamp(HP, MinHP, MHP);
	}
}
