using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHitRate : HitRate
{
    //no calculaton, hit rate is just built in
    public override float CalculateHitRate (Tile target)
    {
        if(guaranteed)
            return 100;
        return abilityHitRate;
    }
}
