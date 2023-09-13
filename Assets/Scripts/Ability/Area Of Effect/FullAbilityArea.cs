using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAbilityArea : AbilityArea 
{
    public override List<Tile> GetTargetedTiles (Board board, Point pos){
        AbilityRange ar = GetComponent<AbilityRange>();
        return ar.GetTilesInRange(board);
    }
}