using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionsExtensions
{
    public static Directions GetDirection (this Tile t1, Tile t2){
        if (t1.position.y < t2.position.y)
            return Directions.North;
        if (t1.position.x < t2.position.x)
            return Directions.East;
        if (t1.position.y > t2.position.y)
            return Directions.South;
        return Directions.West;
    }
    public static Vector3 ToEuler (this Directions d){
        return new Vector3(0, (int)d * 90, 0);
    }

    public static Directions GetDirection (this Point p){
        if (p.y > 0)
            return Directions.North;
        if (p.x > 0)
            return Directions.East;
        if (p.y < 0)
            return Directions.South;
        return Directions.West;
    }
}
