using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : Movement
{
    protected override bool ExpandSearch (Tile from, Tile to)
    {
        if(to.isWalkable){
            if(to.content != null){
                if(to.content.GetComponent<Unit>()){
                    if(to.content.GetComponent<Unit>().ALLIANCE == GetComponent<Unit>().ALLIANCE){
                        Debug.Log("tile is occupied by a unit of a same alliance");
                        return base.ExpandSearch(from, to);
                    }
                }
            }
            else
                return base.ExpandSearch(from, to);
        }
        return false;
        
        //if tile is occupied by NOT a unit or isn't walkable
        if (!to.isWalkable || (to.content != null)){
            Debug.Log("tile is occupied or unwalkable");
            return false;
        }
        // if(to.content != null){
        //     if()
        // }
        GameObject content = to.content;
        if(content.GetComponent<Unit>().ALLIANCE != GetComponent<Unit>().ALLIANCE){
            Debug.Log("tile is occupied by a unit of a different alliance");
            return false;
        }
        // Skip if the tile is occupied or isn't walkable
        // if (to.content != null || !to.isWalkable)
        //     return false;
        return base.ExpandSearch(from, to);
    }

    // public override IEnumerator Traverse (Tile tile)
    // {
    //     unit.Place(tile);
    //     // Build a list of way points from the unit's 
    //     // starting tile to the destination tile
    //     List<Tile> targets = new List<Tile>();
    //     while (tile != null){
    //         targets.Insert(0, tile);
    //         tile = tile.prev;
    //     }
    //     // Move to each way point in succession
    //     for (int i = 1; i < targets.Count; ++i)
    //     {
    //         // Tile from = targets[i-1];
    //         Tile to = targets[i];
    //         // if (unit.dir != dir)
    //         //     yield return StartCoroutine(Turn(dir));
    //         // if (from.height == to.height)
    //             yield return StartCoroutine(Walk(to));
    //         // else
    //         //     yield return StartCoroutine(Jump(to));
    //         // Debug.Log("moving");
    //     }
    //     yield return null;
    // }

    // IEnumerator Walk (Tile target){
    //     // Debug.Log(transform.position + " " + target.center);
    //     while((Vector2)transform.position != target.center){
    //         transform.position  = Vector2.MoveTowards(transform.position, target.center, .05f);

    //         yield return null;
    //     }


    //     // Tweener tweener = transform.MoveTo(target.center, 0.5f, EasingEquations.Linear);
    //     // while (tweener != null)
    //     //     yield return null;
    // }

    // IEnumerator Jump (Tile to){
    //     Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);
    //     Tweener t2 = jumper.MoveToLocal(new Vector3(0, Tile.stepHeight * 2f, 0), tweener.easingControl.duration / 2f, EasingEquations.EaseOutQuad);
    //     t2.easingControl.loopCount = 1;
    //     t2.easingControl.loopType = EasingControl.LoopType.PingPong;
    //     while (tweener != null)
    //         yield return null;
    // }
}
