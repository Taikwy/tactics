using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityArea : AbilityArea 
{
    public override List<Tile> ShowTargetedTiles (Board board){
        List<Tile> tiles = new List<Tile>(targets);
        return tiles;
    }
}