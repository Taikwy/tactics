using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn 
{
    public Unit actingUnit;
    //For calculating new AV and turn stuff
    public bool hasUnitMoved;
    public bool hasUnitActed;
    public int actionCost;
    public bool lockMove;
    Tile startTile;
    Directions startDir;
    // public GameObject ability;
    public Ability selectedAbility;
    public List<Tile> targets;
    
    public void Change (Unit current){
        actingUnit = current;
        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;
        startTile = actingUnit.tile;
        // startDir = actingUnit.dir;
    }
    public void UndoMove (){
        hasUnitMoved = false;
        actingUnit.Place(startTile);
        // actingUnit.dir = startDir;
        actingUnit.Match();
    }
}
