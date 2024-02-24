using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : Movement {
    protected override bool ExpandSearch (Tile from, Tile to){
         if(to.isWalkable || to.isFlyable){
            if(to.content != null){
                if(to.content.GetComponent<Unit>()){
                    // Debug.Log("tile is occupied by a unit, can fly over");
                    return base.ExpandSearch(from, to);
                }
            }
            else
                return base.ExpandSearch(from, to);
        }
        return false;
    }
}