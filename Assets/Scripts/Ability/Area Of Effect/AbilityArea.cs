using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityArea : MonoBehaviour
{
    public int numTargets = 1;
    public List<Tile> targets;
    public virtual bool multipleTargets { get { return false; }}
    public abstract List<Tile> GetTargetedTiles (Board board, Point pos);
}