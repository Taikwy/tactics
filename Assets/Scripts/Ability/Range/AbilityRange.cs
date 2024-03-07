using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbilityRange : MonoBehaviour 
{
    public int range;
    [HideInInspector] public List<Tile> tilesInRange;
    [HideInInspector] public List<Tile> targetsInRange;
    // public int vertical = int.MaxValue;
	public virtual bool positionOriented { get { return true; }}
    public virtual bool directionOriented { get { return false; }}
    protected Unit unit { get { return GetComponentInParent<Unit>(); }}
    public abstract List<Tile> GetTilesInRange (Board board);
    // public abstract List<Tile> GetTargetsInRange (Board board);

    //filters out any tiles that are NOT ground tiles
    public virtual List<Tile> FilterTargetable (List<Tile> tiles){
        
        List<Tile> targetableTiles = new List<Tile>();
        for (int i = tiles.Count - 1; i >= 0; --i){
            if (tiles[i].tileType == Tile.TILETYPE.GROUND || tiles[i].tileType == Tile.TILETYPE.PIT){
                targetableTiles.Add(tiles[i]);
            }
        }
        return targetableTiles;
    }
}