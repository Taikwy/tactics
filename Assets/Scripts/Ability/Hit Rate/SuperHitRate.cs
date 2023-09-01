using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperHitRate : HitRate 
{
    public float skillMultiplier = 2;
    public float luckMultiplier = 1;
    public float accuracyMultiplier = 1;
    public float superBonus = 150;
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

        attackingHitRate = AStats[StatTypes.SK]*skillMultiplier + AStats[StatTypes.LU]*luckMultiplier + AStats[StatTypes.AC]*accuracyMultiplier + superBonus;
        evasion = Mathf.Sqrt(TStats[StatTypes.SP]) + TStats[StatTypes.LU] + TStats[StatTypes.MV];
        finalHitRate = (attackingHitRate - evasion) / attackingHitRate;

        return finalHitRate;
    }
}
