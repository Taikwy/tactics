using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecifyAbilityArea : AbilityArea 
{
    public int range;
    public override List<Tile> ShowTargetedTiles (Board board){
        List<Tile> tiles = new List<Tile>(targets);
        // tiles.Add(board.GetTile(pos));
        return tiles;
    }

    
}