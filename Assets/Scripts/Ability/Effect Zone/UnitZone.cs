using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitZone : EffectZone
{
    public override List<Tile> ShowTilesInZone (Board board, Point pos){
        List<Tile> tiles = new List<Tile>();
        tiles.Add(board.GetTile(pos));
        return tiles;
    }
}
