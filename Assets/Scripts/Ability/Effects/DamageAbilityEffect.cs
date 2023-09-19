using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbilityEffect : BaseAbilityEffect 
{
    #region Public
	public override int Predict (Tile target)
	{
		Debug.Log("predicting damage ability effect");
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

		// Get the attackers base attack stat considering
		// mission items, support check, status check, and equipment, etc
		int attack = GetStat(attacker, defender, GetAttackEvent, 0);

		// Get the targets base defense stat considering
		// mission items, support check, status check, and equipment, etc
		int defense = GetStat(attacker, defender, GetDefenseEvent, 0);

		// Calculate base damage
		int damage = attack - (defense / 2);
		damage = Mathf.Max(damage, 1);

		// Get the abilities power stat considering possible variations
		int power = GetStat(attacker, defender, GetPowerEvent, 0);

		// Apply power bonus
		damage = power * damage / 100;
		damage = Mathf.Max(damage, 1);

		// Tweak the damage based on a variety of other checks like
		// Elemental damage, Critical Hits, Damage multipliers, etc.
		damage = GetStat(attacker, defender, TweakDamageEvent, damage);

		// Clamp the damage to a range
		damage = Mathf.Clamp(damage, minDamage, maxDamage);
		return -damage;
	}
	
	//will need to change this to work with things other than units
	protected override int OnApply (Tile target){
		Debug.Log("applying damage effect");
		Unit defender = target.content.GetComponent<Unit>();
		// Start with the predicted damage value
		int value = Predict(target);

		// Clamp the damage to min and max damage
		value = Mathf.Clamp(value, minDamage, maxDamage);

		// Apply the damage to the target
		Stats s = defender.GetComponent<Stats>();
		s[StatTypes.HP] += value;
		return value;
	}
	#endregion
}