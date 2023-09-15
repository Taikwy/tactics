using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantAbilityRange : AbilityRange 
{
    public override List<Tile> GetTilesInRange (Board board){
        List<Tile> tiles = board.Search(unit.tile, ExpandSearch);
        tiles.Remove(unit.tile);
        return tiles;
    }
    bool ExpandSearch (Tile from, Tile to){
        return (from.distance + 1) <= range;
    }

    // public override List<Tile> GetTargetsInRange (Board board){
    //     return board.Search(unit.tile, ExpandSearch);
    // }
}