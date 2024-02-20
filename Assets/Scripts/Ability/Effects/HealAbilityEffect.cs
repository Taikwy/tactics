using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilityEffect : BaseAbilityEffect 
{
    public int baseHealAmount = 0;
	public int defensePercentModifier = -1;						//percentage out of 100. scales the unit's defense stat
	public int attackPercentModifier = -1;						//percentage out of 100. scales the unit's attack stat

	void Start(){
		abilityEffectName = "HEAL";
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
		float attack = GetStatForCombat(attacker, defender, GetAttackEvent, 0);
		float defense = GetStatForCombat(attacker, defender, GetDefenseEvent, 0);

		//terrain and weapon bonus logic needs to be added, should use events
		// int terrainBonus = 0;
		int weaponBonus = 0;

        float healAmount = 0;
        if(defensePercentModifier >= 0){
		    healAmount = defensePercentModifier/100.0f * defense + baseHealAmount + weaponBonus;
        }else if(attackPercentModifier >= 0){
		    healAmount = attackPercentModifier/100.0f * attack + baseHealAmount + weaponBonus;
        }else{
            Debug.LogError("No percent modifier specified for healing effect " + gameObject);
        }
        healAmount = Mathf.Max(healAmount, 1);
		healAmount = Mathf.Clamp(healAmount, 0, maxHeal); 
		
		Debug.Log("predicting heal ability effect  HEAL [ " + "percent[" + defensePercentModifier + "% OR " + attackPercentModifier + "%] * (attack[" + attack + "] OR defense[" + defense + "]  + terrain0[]) + weapon[0] ] = " + healAmount + " dmg");
		return (int)healAmount;
	}
	
	//will need to change this to work with things other than units
	protected override int OnApply (Tile target){
		// Debug.Log("applying damage effect");
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

		// Start with the predicted damage value
		int predictedHeal = Predict(target);

		//apply crit rate stuff
		// Debug.Log("looking for crits");
		float critRate = GetStatForCombat(attacker, defender, GetCritRateEvent, 0);
		float critDMG =  GetStatForCombat(attacker, defender, GetCritDMGEvent, 0);
		if(Random.Range(0, 101) <= critRate){
			didCrit = true;
			int critDamage = (int) Mathf.Ceil( (critDMG/100f) * predictedHeal);
			predictedHeal += critDamage;

			// predictedDMG *= (int)((1f + critDMG/100f)*predictedDMG);
			Debug.Log("CRIT!!! " + critRate + " | " + critDMG + " | crit damage: " + critDamage);
		}
		else
			didCrit = false;

		// Clamp the damage to min and max damage
		predictedHeal = Mathf.Clamp(predictedHeal, minHeal, maxHeal);
        Debug.Log(defender.name + " healed by " + predictedHeal);

		// Apply the damage to the target
		Stats s = defender.GetComponent<Stats>();
		s[StatTypes.HP] += predictedHeal;
		return predictedHeal;
	}
	protected override int OnSubApply (Tile target, bool crit){
		// Debug.Log("applying SUB damage effect");
		Unit attacker = GetComponentInParent<Unit>();
		Unit defender = target.content.GetComponent<Unit>();

		// Start with the predicted damage value
		int predictedHeal = Predict(target);

		//if this is a sub effect, it will crit depending on whether its parent effect critted
		// Debug.Log("looking for crits");
		float critDMG =  GetStatForCombat(attacker, defender, GetCritDMGEvent, 0);
		if(crit){
			int critDamage = (int) Mathf.Ceil( (critDMG/100f) * predictedHeal);
			predictedHeal += critDamage;

			// predictedDMG *= (int)((1f + critDMG/100f)*predictedDMG);
			Debug.Log("PARENT DID CRIT!!! " + crit + " | " + critDMG + " | crit damage: " + critDamage);
		}

		// Clamp the damage to min and max damage
		predictedHeal = Mathf.Clamp(predictedHeal, minHeal, maxHeal);

		// Apply the damage to the target
		Stats s = defender.GetComponent<Stats>();
		s[StatTypes.HP] += predictedHeal;
		return predictedHeal;
	}

	void OnGetBaseAttack (object sender, object args){
		if (IsMyEffect(sender)){
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, GetComponentInParent<Stats>()[StatTypes.AT]) );
		}
	}
	void OnGetBaseDefense (object sender, object args){
		if (IsMyEffect(sender)){
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, GetComponentInParent<Stats>()[StatTypes.DF]) );
		}
	}

	protected override void OnPrimaryHit(object sender, object args){}
	protected override void OnPrimaryMiss(object sender, object args){}
	
}