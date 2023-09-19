using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitRate : HitRate 
{
    public bool guaranteed = false;
    public override float CalculateHitRate (Tile target)
    {
        if(target.content.GetComponent<Unit>() == null || guaranteed)
            return 100;
        Stats AStats = GetComponentInParent<Unit>().statsScript;
        Stats TStats = target.content.GetComponent<Unit>().statsScript;
        float finalHitRate;
        float attackingHitRate;
        float evasion;

        //Add this later depending on weapons and buffs and stuff
        float attackingBonus;
        float evasionBonus;

        attackingHitRate = AStats[StatTypes.SK] * 4 + AStats[StatTypes.LU] + AStats[StatTypes.AC];
        evasion = Mathf.Sqrt(TStats[StatTypes.SP]) + TStats[StatTypes.LU] + TStats[StatTypes.MV];
        finalHitRate = (attackingHitRate - evasion) / attackingHitRate;

        // Debug.Log("attacker stats " + AStats[StatTypes.SK] + " " + AStats[StatTypes.LU] + " " + AStats[StatTypes.AC]);
        // Debug.Log("defender stats " + TStats[StatTypes.SP] + " " + TStats[StatTypes.LU] + " " + TStats[StatTypes.MV]);
        // Debug.Log("attacking hit rate " + attackingHitRate +  " evasion " + evasion + " final hit rate " + finalHitRate);
        return finalHitRate;
    }
}
