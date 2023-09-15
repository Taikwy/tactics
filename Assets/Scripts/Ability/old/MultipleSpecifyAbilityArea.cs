using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSpecifyAbilityArea : AbilityArea 
{
    public int horizontal;
    Tile tile;
    public override List<Tile> ShowTargetedTiles (Board board){
        List<Tile> tiles = new List<Tile>(targets);
        // tiles.Add(board.GetTile(pos));
        // Debug.Log(targets.Count);
        // Debug.Log(tiles.Count);
        return tiles;
    }
    // public override List<Tile> GetTargetedTiles (Board board, Point pos){
    //     tile = board.GetTile(pos);
    //     return board.Search(tile, ExpandSearch);
    // }
    // bool ExpandSearch (Tile from, Tile to){
    //     return (from.distance + 1) <= horizontal;
    // }
}