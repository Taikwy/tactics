using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class SpecifyAbilityArea : AbilityArea 
// {
//     // public int range;
//     public int numTargets = 1;
    
//     public int splashRange;
// 	Tile tile;
//     public override List<Tile> ShowTargetedTiles (Board board){
//         List<Tile> tiles = new List<Tile>(targets);
//         // tiles.Add(board.GetTile(pos));
//         return tiles;
//     }

//     public override List<Tile> GetTilesInArea (Board board, Point pos)
// 	{
// 		tile = board.GetTile(pos);
// 		return board.Search(tile, ExpandSearch);
// 	}

// 	bool ExpandSearch (Tile from, Tile to)
// 	{
// 		return (from.distance + 1) <= splashRange;
// 	}

    
// }