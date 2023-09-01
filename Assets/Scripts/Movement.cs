using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public int range { get { return stats[StatTypes.MV]; }}
    // public int jumpHeight { get { return stats[StatTypes.JMP]; }}
    protected Stats stats;

    public int jumpHeight;
    protected Unit unit;
    protected Transform jumper;

    protected virtual void Awake ()
    {
        unit = GetComponent<Unit>();
        // jumper = transform.FindChild("Jumper");
    }

    protected virtual void Start ()
    {
        stats = GetComponent<Stats>();
    }
    public virtual List<Tile> GetTilesInRange (Board board){
        List<Tile> retValue = board.Search( unit.tile, ExpandSearch );
        FilterOccupied(retValue);
        return retValue;
    }

    protected virtual bool ExpandSearch (Tile from, Tile to){
        return (from.distance + 1) <= range;
    }

    protected virtual void Filter (List<Tile> tiles){
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
    }

    protected virtual void FilterOccupied (List<Tile> tiles){
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
    }

    public virtual IEnumerator Move(Tile target)
    {
        unit.Place(target);
        //Builds a list backwards forwards from start to target tile
        List<Tile> targets = new List<Tile>();
        while (target != null){
            targets.Insert(0, target);
            target = target.prev;
        }

        // Move to each way point in succession
        for (int i = 1; i < targets.Count; ++i)
        {
            Tile to = targets[i];
            yield return StartCoroutine(MoveTo(to));
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }

    IEnumerator MoveTo (Tile target){
        while((Vector2)transform.position != target.center){
            transform.position  = Vector2.MoveTowards(transform.position, target.center, .035f);
            yield return null;
        }
    }
    // public abstract IEnumerator Traverse (Tile tile);

    protected virtual IEnumerator Turn (Directions dir)
    {
        Debug.Log("tweening");
        // TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);
        
        // // When rotating between North and West, we must make an exception so it looks like the unit
        // // rotates the most efficient way (since 0 and 360 are treated the same)
        // if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
        //     t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        // else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0))
        //     t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        // unit.dir = dir;
        
        // while (t != null)
            yield return null;
    }
}
