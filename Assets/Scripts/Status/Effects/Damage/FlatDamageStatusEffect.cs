using UnityEngine;
using System.Collections;
using System.IO;

public class FlatDamageStatusEffect : DamageStatusEffect 
{
	protected override void OnNewTurn (object sender, object args){
		Debug.Log("NEW TURN FOR " + statusName + " (FLAT DAMAGE) " + flatOrPercent + " dmg taken");
		Stats s = GetComponentInParent<Stats>();
		int currentHP = s[StatTypes.HP] - flatOrPercent;
		s.SetValue(StatTypes.HP, currentHP, false);
	}
}
