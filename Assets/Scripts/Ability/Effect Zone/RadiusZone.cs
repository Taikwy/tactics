using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusZone : EffectZone
{
    public override List<Tile> ShowTilesInZone (Board board, Point pos){
        Tile tile = board.GetTile(pos);
        return board.Search(tile, ExpandSearch);
    }

    bool ExpandSearch (Tile from, Tile to)
    {
        return (from.distance + 1) <= range;
    }
}
