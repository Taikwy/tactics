using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//UNTESTED, JUST COPIED THE CODE FROM ORTHOGONAL ABILITY RANGE.
public class OrthogonalZone : EffectZone
{
    public override List<Tile> ShowTilesInZone (Board board, Point pos){

        Point startPos = pos;
        Point endPos;
        List<Tile> tiles = new List<Tile>();

        Point[] endPoints = {
            new Point(startPos.x, startPos.y + range),
            new Point(startPos.x, startPos.y - range),
            new Point(startPos.x + range, startPos.y),
            new Point(startPos.x - range, startPos.y)
        };
        foreach(Point endPoint in endPoints){
            startPos = pos;
            endPos = endPoint;
            while (startPos != endPos){
                if (startPos.x < endPos.x) startPos.x++;
                else if (startPos.x > endPos.x) startPos.x--;
                if (startPos.y < endPos.y) startPos.y++;
                else if (startPos.y > endPos.y) startPos.y--;
                    Tile t = board.GetTile(startPos);
                if (t != null)
                    tiles.Add(t);
            }
        }
        
        return tiles;
    }    
}
