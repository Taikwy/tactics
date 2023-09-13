using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSpecifyAbilityArea : AbilityArea 
{
    public int horizontal;
    public int vertical;
    Tile tile;
    public override bool multipleTargets { get { return true; }}
    public override List<Tile> GetTargetedTiles (Board board, Point pos){
        tile = board.GetTile(pos);
        return board.Search(tile, ExpandSearch);
    }
    bool ExpandSearch (Tile from, Tile to){
        return (from.distance + 1) <= horizontal;
    }
}