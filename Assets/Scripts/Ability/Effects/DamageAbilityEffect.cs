using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbilityEffect : BaseAbilityEffect 
{
	public int attackPercentModifier = 0;						//percentage out of 100. scales the unit's attack stat

	void Start(){
		abilityEffectName = "DAMAGE";
	}
	void OnEnable (){
		base.OnEnable();
		this.AddObserver(OnGetBaseAttack, GetAttackEvent);
		this.AddObserver(OnGetBaseDefense, GetDefenseEvent);
	}void OnDisable (){
		base.OnEnable();
		this.RemoveObserver(OnGetBaseAttack, GetAttackEvent);
		this.RemoveObserver(OnGetBaseDefense, GetDefenseEvent);
	}

	public override int Predict (Tile target){
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

		// Get the attacker and defender's stats considering equipment and statuses
		int attack = GetStatForCombat(attacker, defender, GetAttackEvent, 0);
		int defense = GetStatForCombat(attacker, defender, GetDefenseEvent, 0);

		//terrain and weapon bonus logic needs to be added, should use events
		int terrainBonus = 0;
		int weaponBonus = 0;

		//base damage calc
		float attackingDamage = attackPercentModifier/100.0f * (attack + terrainBonus) + weaponBonus;
		float damage = attackingDamage - defense;
		damage = Mathf.Max(damage, 1);

		// Clamp the damage to a range, just for edge cases
		damage = Mathf.Clamp(damage, 0, maxDamage); 
		
		Debug.Log("predicting damage ability effect  dmg[ " + "percent[" + attackPercentModifier + "] * (attack[" + attack + "] + terrain0[]) + weapon[0]"+ " ] - def["+ defense + "] = " + damage + " dmg");
		return -(int)damage;
	}
	
	//will need to change this to work with things other than units
	protected override int OnApply (Tile target){
		// Debug.Log("applying damage effect");
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

		// Start with the predicted damage value
		int predictedDMG = Predict(target);

		//apply crit rate stuff
		// Debug.Log("looking for crits");
		float critRate = GetStatForCombat(attacker, defender, GetCritRateEvent, 0);
		float critDMG =  GetStatForCombat(attacker, defender, GetCritDMGEvent, 0);
		if(Random.Range(0, 101) <= critRate){
			didCrit = true;
			int critDamage = (int) Mathf.Ceil( (critDMG/100f) * predictedDMG);
			predictedDMG += critDamage;

			// predictedDMG *= (int)((1f + critDMG/100f)*predictedDMG);
			Debug.Log("CRIT!!! " + critRate + " | " + critDMG + " | crit damage: " + critDamage);
		}
		else
			didCrit = false;

		// Clamp the damage to min and max damage
		predictedDMG = Mathf.Clamp(predictedDMG, minDamage, maxDamage);

		// Apply the damage to the target
		Stats s = defender.GetComponent<Stats>();
		s[StatTypes.HP] += predictedDMG;
		return predictedDMG;
	}
	protected override int OnSubApply (Tile target, bool crit){
		// Debug.Log("applying SUB damage effect");
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

		// Start with the predicted damage value
		int predictedDMG = Predict(target);

		//if this is a sub effect, it will crit depending on whether its parent effect critted
		// Debug.Log("looking for crits");
		float critDMG =  GetStatForCombat(attacker, defender, GetCritDMGEvent, 0);
		if(crit){
			int critDamage = (int) Mathf.Ceil( (critDMG/100f) * predictedDMG);
			predictedDMG += critDamage;

			// predictedDMG *= (int)((1f + critDMG/100f)*predictedDMG);
			Debug.Log("PARENT DID CRIT!!! " + crit + " | " + critDMG + " | crit damage: " + critDamage);
		}

		// Clamp the damage to min and max damage
		predictedDMG = Mathf.Clamp(predictedDMG, minDamage, maxDamage);

		// Apply the damage to the target
		Stats s = defender.GetComponent<Stats>();
		s[StatTypes.HP] += predictedDMG;
		return predictedDMG;
	}

	void OnGetBaseAttack (object sender, object args){
		if (IsMyEffect(sender)){
			// MonoBehaviour obj = sender as MonoBehaviour;
			// Debug.Log("check attack is my effect " + obj + " | " + obj.transform.parent + " | " + transform);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, GetComponentInParent<Stats>()[StatTypes.AT]) );
			// Debug.Log("on get base attack " + GetComponentInParent<Stats>()[StatTypes.AT]);
		}
	}
	void OnGetBaseDefense (object sender, object args){
		if (IsMyEffect(sender)){
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, info.arg1.GetComponent<Stats>()[StatTypes.DF]) );
			// Debug.Log("on get base defense " +  info.arg1.GetComponent<Stats>()[StatTypes.DF]);
		}
	}

	protected override void OnPrimaryHit(object sender, object args){}
	protected override void OnPrimaryMiss(object sender, object args){}
	
}