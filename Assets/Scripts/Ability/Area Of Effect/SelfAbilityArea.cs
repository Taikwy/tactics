using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfAbilityArea : AbilityArea 
{
    // [HideInInspector] public bool canTargetSelf = true;
    public override List<Tile> ShowTargetedTiles (Board board){
        AbilityRange ar = GetComponent<AbilityRange>();
        return ar.GetTilesInRange(board);
    }
    // public override List<Tile> GetTargetedTiles (Board board, Point pos){
    //     List<Tile> retValue = new List<Tile>();
    //     Tile tile = board.GetTile(pos);
    //     if (tile != null)
    //         retValue.Add(tile);
    //     return retValue;
    // }
}