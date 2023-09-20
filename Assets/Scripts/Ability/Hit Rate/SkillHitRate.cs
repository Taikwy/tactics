using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHitRate : HitRate 
{
    public float skillMultiplier = 5;
    public float luckMultiplier = .5f;
    public float accuracyMultiplier = 1;
    public float skillBonus = 0;
    public override float CalculateHitRate (Tile target)
    {
        if(target.content.GetComponent<Unit>() == null || guaranteed)
            return 100;
        Stats AStats = GetComponentInParent<Unit>().statsScript;
        Stats TStats = target.content.GetComponent<Unit>().statsScript;
        float finalHitRate;
        float attackingHitRate;
        float evasion;

        attackingHitRate = AStats[StatTypes.SK]*skillMultiplier + AStats[StatTypes.LU]*luckMultiplier + AStats[StatTypes.AC]*accuracyMultiplier + skillBonus;
        evasion = Mathf.Sqrt(TStats[StatTypes.SP]) + TStats[StatTypes.LU] + TStats[StatTypes.MV];
        finalHitRate = (attackingHitRate - evasion) / attackingHitRate;


        Debug.Log("attacker stats " + AStats[StatTypes.SK] + " " + AStats[StatTypes.LU] + " " + AStats[StatTypes.AC]);
        Debug.Log("defender stats " + TStats[StatTypes.SP] + " " + TStats[StatTypes.LU] + " " + TStats[StatTypes.MV]);
        Debug.Log("attacking hit rate " + attackingHitRate +  " evasion " + evasion + " final hit rate " + finalHitRate);
        
        return finalHitRate;
    }
}
