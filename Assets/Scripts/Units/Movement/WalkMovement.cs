using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : Movement
{
    protected override bool ExpandSearch (Tile from, Tile to){
        if(to.isWalkable){
            if(to.content != null){
                if(to.content.GetComponent<Unit>()){
                    if(to.content.GetComponent<Unit>().ALLIANCE == GetComponent<Unit>().ALLIANCE){
                        // Debug.Log("tile is occupied by a unit of a same alliance");
                        return base.ExpandSearch(from, to);
                    }
                }
            }
            else
                return base.ExpandSearch(from, to);
        }
        return false;
    }
}
