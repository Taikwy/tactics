using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LineAbilityRange : AbilityRange 
{
    public override bool directionOriented { get { return true; }}
    public override List<Tile> GetTilesInRange (Board board){
        Point startPos = unit.tile.position;
        Point endPos;
        tilesInRange = new List<Tile>();
        Debug.Log("range " + range);
        Point[] endPoints = {
            new Point(startPos.x, startPos.y + range),
            new Point(startPos.x, startPos.y - range),
            new Point(startPos.x + range, startPos.y),
            new Point(startPos.x - range, startPos.y)
        };
        foreach(Point endPoint in endPoints){
            startPos = unit.tile.position;
            endPos = endPoint;
            while (startPos != endPos){
            if (startPos.x < endPos.x) startPos.x++;
            else if (startPos.x > endPos.x) startPos.x--;
            if (startPos.y < endPos.y) startPos.y++;
            else if (startPos.y > endPos.y) startPos.y--;
                Tile t = board.GetTile(startPos);
            if (t != null)
                tilesInRange.Add(t);
            }
        }
        
        return tilesInRange;
    }
    // public override List<Tile> GetTargetsInRange (Board board)
    // {
    //     Point startPos = unit.tile.position;
    //     Point endPos;
    //     targetsInRange = new List<Tile>();
    //     switch (unit.dir)
    //     {
    //         case Directions.North:
    //             endPos = new Point(startPos.x, startPos.y + range);
    //             break;
    //         case Directions.East:
    //             endPos = new Point(startPos.x + range, startPos.y);
    //             break;
    //         case Directions.South:
    //             endPos = new Point(startPos.x, startPos.y - range);
    //             break;
    //         default: // West
    //             endPos = new Point(startPos.x - range, startPos.y);
    //             break;
    //     }
    //     while (startPos != endPos){
    //         if (startPos.x < endPos.x) startPos.x++;
    //         else if (startPos.x > endPos.x) startPos.x--;
    //         if (startPos.y < endPos.y) startPos.y++;
    //         else if (startPos.y > endPos.y) startPos.y--;
    //             Tile t = board.GetTile(startPos);
    //         if (t != null)
    //             targetsInRange.Add(t);
    //     }
    
    //     return targetsInRange;
    // }
}
