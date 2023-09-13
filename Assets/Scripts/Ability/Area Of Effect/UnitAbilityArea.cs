using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityArea : AbilityArea 
{
    public override List<Tile> GetTargetedTiles (Board board, Point pos){
        List<Tile> retValue = new List<Tile>();
        Tile tile = board.GetTile(pos);
        if (tile != null)
            retValue.Add(tile);
        return retValue;
    }
}