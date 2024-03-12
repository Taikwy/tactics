using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAbilityArea : AbilityArea 
{
    // public override List<Tile> ShowTargetedTiles (Board board){
    //     AbilityRange ar = GetComponent<AbilityRange>();
    //     return ar.GetTilesInRange(board);
    // }
    
    public override List<Tile> GetTilesInArea (Board board, Point pos){
		AbilityRange ar = GetComponent<AbilityRange>();
        targets = ar.FilterTargetable(ar.GetTilesInRange(board));
		return targets;
	}
}