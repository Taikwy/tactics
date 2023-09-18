using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Point
{
    public int x, y;

    public Point(int x = 0, int y = 0){
        this.x = x;
        this.y = y;
    }

    //Operators
    public static Point operator +(Point a, Point b){
        return new Point(a.x + b.x, a.y + b.y);
    }

    public static Point operator -(Point a, Point b){
        return new Point(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(Point a, Point b){
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Point a, Point b){
        return !(a==b);
    }

    //Hash stuff?
    public override bool Equals(object obj){
        if(obj is Point){
            Point p = (Point)obj;
            return x == p.x && y == p.y;
        }
        return false;
    }

    public bool Equals(Point p){
        return x == p.x && y == p.y;   
    }

    public override int GetHashCode(){
        return x ^ y;
    }

    //For debugging
    public override string ToString(){
        return string.Format ("({0},{1})", x, y);
    }
}
