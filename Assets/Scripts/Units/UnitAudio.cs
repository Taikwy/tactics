using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAudio : MonoBehaviour
{
    public Unit unit;
    public Stats stats;
    public string hpIncreaseSound, hpDecreaseSound, deathSound, moveSound;
    AudioManager am{ get { return GetComponentInParent<BattleController>().audioManager; } }
    void OnEnable (){
        this.AddObserver(OnDeath, Unit.UnitDiedEvent);
		this.AddObserver(OnChangeHealth, Stats.DidChangeEvent(StatTypes.HP));
		// this.AddObserver(OnLoseHealth, Stats.DidChangeEvent(StatTypes.HP));
	}

	void OnDisable (){
		this.RemoveObserver(OnDeath, Unit.UnitDiedEvent);
		this.RemoveObserver(OnChangeHealth, Stats.DidChangeEvent(StatTypes.HP));
		// this.RemoveObserver(OnLoseHealth, Stats.DidChangeEvent(StatTypes.HP));
	}

    public void OnGainHealth(object sender, object args){
    }
    public void OnChangeHealth(object sender, object args){
        Unit unit = (sender as Stats).GetComponent<Unit>();
        if((sender as Stats) == stats){
            int oldVal = (int)args;
            if(stats[StatTypes.HP] > oldVal)
                am.PlaySFX(hpIncreaseSound);
            else if(stats[StatTypes.HP] < oldVal)
                am.PlaySFX(hpDecreaseSound);
        }
    }
    public void OnLoseHealth(object sender, object args){
        // am.Play(hpDecreaseSound);
    }
    public void OnDeath(object sender, object args){
        if((sender as Unit) == unit)
            am.PlaySFX(deathSound);
    }    
    bool IsThisUnit(Unit sender){
        if(sender == unit)
            return true;
        return false;
    }
}
