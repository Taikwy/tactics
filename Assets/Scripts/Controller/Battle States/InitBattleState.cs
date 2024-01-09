using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class InitBattleState : BattleState 
{
    public override void Enter ()
    {
        base.Enter ();
        StartCoroutine(Init());
    }
    
    IEnumerator Init (){
        board.Load( levelData );
        Point p = new Point((int)levelData.tilePositions[0].x, (int)levelData.tilePositions[0].y);
        SelectTile(p);
        // SpawnTestUnits(); // This is new
        SpawnFactory();
        
        yield return null;
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();

        owner.ChangeState<SelectUnitState>(); // This is changed
    }

    //TEst the factory stuff
    void SpawnFactory(){
        string[] unitRecipes = new string[]{
            "Paladin",
            "Lancer",
            "Alchemist",
            "Shaman",

            "Slime",
            "Mushroom",
            // "Snake",
            "Kitsune",
            "Snake"
        };
        Point[] spawnLocations = new Point[]{
            new Point(2,3),
            new Point(3,3),
            new Point(5,3),
            new Point(6,3),

            new Point(6,6),
            new Point(4,7),
            new Point(5,8),
            new Point(6,8),
        };
        for (int i = 0; i < unitRecipes.Length; ++i)
        {
            GameObject instance = UnitFactory.Create(unitRecipes[i], 1);
            Unit unitScript = instance.GetComponent<Unit>();
            unitScript.Init(board.GetTile(spawnLocations[i]));

            units.Add(unitScript);
        }
        
        
        SelectTile(units[0].tile.position);
    }

    void SpawnTestUnits ()    //curerently unused, i think this was the old version of spawnfactory?
    {
        string[] unitRecipes = new string[]{
            // "Paladin",
            // "Wizard",
            // "Slime",
            // "Mushroom",
            "Snake"
        };
        //List of all locations on map
        List<Tile> locations = new List<Tile>(board.tiles.Values);
        for (int i = 0; i < unitRecipes.Length; ++i)
        {
            int level = 0;
            GameObject instance = UnitFactory.Create(unitRecipes[i], level);

            //Finds random point to spawn the unit
            int random = Random.Range(0, locations.Count);
            Tile randomTile = locations[random];
            locations.RemoveAt(random);

            Unit unitScript = instance.GetComponent<Unit>();
            unitScript.Init(randomTile);

            units.Add(unitScript);
        }
        
        OldSpawnUnits();
        SelectTile(units[0].tile.position);
    }

    void OldSpawnUnits(){
        string[] jobs = new string[]{"Rogue", "Warriro"};
        
        for (int i = 0; i < jobs.Length; ++i){
            GameObject unit = Instantiate(owner.playerPrefab);
            unit.name = jobs[i];
            

            Point p = new Point((int)levelData.tilePositions[i].x + 2, (int)levelData.tilePositions[i].y + 1);
            Unit unitScript = unit.GetComponent<Unit>();
            unitScript.Init(board.GetTile(p));

            // unit.AddComponent<WalkMovement>();
            units.Add(unitScript);

            //    Rank rank = instance.AddComponent<Rank>();
            //    rank.Init (10);
        }
        
    }
}
