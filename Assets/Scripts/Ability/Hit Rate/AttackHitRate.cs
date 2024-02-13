using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitRate : HitRate 
{
    public override float CalculateHitRate (Tile target)
    {
        if(target.content.GetComponent<Unit>() == null || guaranteed)
            return 100;
        Stats AStats = GetComponentInParent<Unit>().statsScript;
        Stats DStats = target.content.GetComponent<Unit>().statsScript;

        float baseHitRate;
        float finalHitRate;
        float abilityHitRate = 0;
        float defenderTerrainBonus = 0;
        
        baseHitRate = abilityHitRate + defenderTerrainBonus + (AStats[StatTypes.SP]/(AStats[StatTypes.SP] + DStats[StatTypes.SP]) - 0.5f);

        finalHitRate = baseHitRate;

        // Debug.Log("attacker stats " + AStats[StatTypes.SK] + " " + AStats[StatTypes.LU] + " " + AStats[StatTypes.AC]);
        // Debug.Log("defender stats " + TStats[StatTypes.SP] + " " + TStats[StatTypes.LU] + " " + TStats[StatTypes.MV]);
        // Debug.Log("attacking hit rate " + attackingHitRate +  " evasion " + evasion + " final hit rate " + finalHitRate);
        return finalHitRate;
    }

}
