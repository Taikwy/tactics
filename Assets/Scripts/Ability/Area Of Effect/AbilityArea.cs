using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityArea : MonoBehaviour
{
    [HideInInspector] public bool canTargetSelf = false;
    [HideInInspector] public List<Tile> targets;
    // [HideInInspector] public abstract List<Tile> ShowTargetedTiles (Board board);
    [HideInInspector] public abstract List<Tile> GetTilesInArea (Board board, Point pos);
    // public abstract List<Tile> GetTargetedTiles (Board board, Point pos);
}