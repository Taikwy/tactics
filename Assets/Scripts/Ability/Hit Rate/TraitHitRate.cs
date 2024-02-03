using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitHitRate : HitRate
{
    //returns hit rate as percentage - 50%
    public override float CalculateHitRate (Tile target)
    {
        if(target.content.GetComponent<Unit>() == null || guaranteed)
            return 100;
        Stats AStats = GetComponentInParent<Unit>().statsScript;
        Stats DStats = target.content.GetComponent<Unit>().statsScript;
        
        float baseHitRate;
        float finalHitRate;

        float AS = AStats[StatTypes.SP];
        float DS = DStats[StatTypes.SP];

        //update terrain bonus when i actually have terrain. ill prob have to check the itle the defender is currently occupying to grab the tile script and then get the bonus from there
        float defenderTerrainBonus = 0;
        float speed = (AS/(AS + DS)) - 0.5f;
        baseHitRate = abilityHitRate + defenderTerrainBonus + speed;

        finalHitRate = baseHitRate;

        // Debug.Log("ability hit rate " + abilityHitRate + " attacker stats " + AS + " defender stats " + DS + " speed " + speed);
        // Debug.Log("hitrate = " + finalHitRate);
        return finalHitRate;
    }
}
