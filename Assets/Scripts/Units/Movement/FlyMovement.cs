using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : Movement 
{
    protected override bool ExpandSearch (Tile from, Tile to){
         if(to.isWalkable || to.isFlyable){
            if(to.content != null){
                if(to.content.GetComponent<Unit>()){
                    Debug.Log("tile is occupied by a unit, can fly over");
                    return base.ExpandSearch(from, to);
                }
            }
            else
                return base.ExpandSearch(from, to);
        }
        return false;

        // Skip if the tile is occupied
        if (to.content != null || !to.isFlyable)
            return false;
        return base.ExpandSearch(from, to);
    }

    // public override IEnumerator Traverse (Tile tile)
    // {
    //     unit.Place(tile);
    //     //Builds a list backwards forwards from start to target tile
    //     List<Tile> targets = new List<Tile>();
    //     while (tile != null){
    //         targets.Insert(0, tile);
    //         tile = tile.prev;
    //     }

    //     // Move to each way point in succession
    //     for (int i = 1; i < targets.Count; ++i)
    //     {
    //         Tile to = targets[i];
    //         yield return StartCoroutine(Fly(to));
    //         yield return new WaitForSeconds(.025f);
    //     }
    //     yield return null;
    // }

    // IEnumerator Fly (Tile target){
    //     while((Vector2)transform.position != target.center){
    //         transform.position  = Vector2.MoveTowards(transform.position, target.center, .05f);

    //         yield return null;
    //     }
    // }
}