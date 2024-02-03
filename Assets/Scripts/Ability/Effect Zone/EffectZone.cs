using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectZone : MonoBehaviour
{
    public int range;
    public abstract List<Tile> ShowTilesInZone (Board board, Point pos);
}
