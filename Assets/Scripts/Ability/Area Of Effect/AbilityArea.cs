using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityArea : MonoBehaviour
{
    [HideInInspector] public bool canTargetSelf = false;
    public int numTargets = 1;
    [HideInInspector] public List<Tile> targets;
    [HideInInspector] public abstract List<Tile> ShowTargetedTiles (Board board);
    // public abstract List<Tile> GetTargetedTiles (Board board, Point pos);
}