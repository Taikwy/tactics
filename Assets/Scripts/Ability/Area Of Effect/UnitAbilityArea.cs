using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityArea : AbilityArea 
{
    public int numTargets = 1;
    // public override List<Tile> ShowTargetedTiles (Board board){
    //     List<Tile> tiles = new List<Tile>(targets);
    //     return tiles;
    // }

    public override List<Tile> GetTilesInArea (Board board, Point pos){
		// List<Tile> retValue = new List<Tile>();
		Tile tile = board.GetTile(pos);
        //only adds a NEW and VALID tile, given that the list isn't already maxed
		if (tile != null && targets.Count < numTargets && !targets.Contains(tile))
			targets.Add(tile);
		return targets;
	}
}