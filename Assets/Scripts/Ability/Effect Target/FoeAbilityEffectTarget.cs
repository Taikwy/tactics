using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeAbilityEffectTarget : AbilityEffectTarget 
{
    public override bool IsTarget (Tile tile){
        if (tile == null || tile.content == null || !IsFoe(tile))
            return false;
        
        Stats s = tile.content.GetComponent<Stats>();
        return s != null && s[StatTypes.HP] > 0;
    }
    bool IsFoe(Tile tile){
        Unit unit = tile.content.GetComponent<Unit>();
        if(unit){
            return GetComponentInParent<Unit>().allianceScript.IsMatch(unit.allianceScript, Targets.Foe);
        }
        return false;
    }
}