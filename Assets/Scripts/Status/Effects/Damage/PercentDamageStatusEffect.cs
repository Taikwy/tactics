using UnityEngine;
using System.Collections;
using System.IO;

public class PercentDamageStatusEffect : DamageStatusEffect 
{
	protected override void OnNewTurn (object sender, object args){
		Debug.Log("NEW TURN FOR " + statusName + " (PERCENT DAMAGE) " + (flatOrPercent/100f) + " raw dmg taken");
		Stats s = GetComponentInParent<Stats>();
		int percentDMG = (int)(s[StatTypes.HP]*(flatOrPercent/100f));
		int currentHP = s[StatTypes.HP] - Mathf.Max(percentDMG, 1);
		s.SetValue(StatTypes.HP, currentHP, false);

		this.PostEvent(EffectAppliedEvent, percentDMG);
	}
}
