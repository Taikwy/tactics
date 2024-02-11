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
        owner.turnOrderController = owner.gameObject.AddComponent<TurnOrderController>();
        owner.round = owner.turnOrderController.Round();
        
        board.Load( levelData );
        Point p = new Point((int)levelData.tilePositions[0].x, (int)levelData.tilePositions[0].y);
        SelectTile(p);
        // SpawnTestUnits(); // This is new
        SpawnFactory();
		AddVictoryCondition();
        
        yield return null;

		// owner.ChangeState<CutSceneState>();
        owner.ChangeState<SelectUnitState>();
    }

    //TEst the factory stuff
    void SpawnFactory(){
        string[] unitRecipes = new string[]{
            "Paladin",
            "Lancer",
            "Alchemist",
            // "Shaman",

            "Slime",
            "Mushroom",
            "Kitsune",
            // "Snake"
        };
        Point[] spawnLocations = new Point[]{
            new Point(4,4),
            new Point(5,3),
            new Point(6,3),
            // new Point(6,3),

            new Point(6,6),
            new Point(4,7),
            new Point(5,8),
            // new Point(6,8),
        };

        GameObject unitContainer = new GameObject("Units");
		unitContainer.transform.SetParent(owner.transform);

        for (int i = 0; i < unitRecipes.Length; ++i)
        {
            GameObject instance = UnitFactory.Create(unitRecipes[i], 1);
            instance.transform.SetParent(unitContainer.transform);

            Unit unitScript = instance.GetComponent<Unit>();
            unitScript.Init(board.GetTile(spawnLocations[i]));
            turnOrderController.CalculateAV(unitScript);
            // Debug.Log(unitScript.name + " AV " + unitScript.gameObject.GetComponent<Stats>()[StatTypes.AV]);

            units.Add(unitScript);
        }
        string result = "Units: ";
        foreach (var item in units){ result += item.ToString() + ", "; }
        // Debug.Log(result);

        owner.timeline.PopulateTimeline(units);
        
        // SelectTile(units[0].tile.position);                  //this is prob unneeded, since i already select a tile later in select unit state. ig this is jsut for the intiial tile indicator "before" i make a unit take action?
    }

    void AddVictoryCondition (){
        string result = "Victory Units: ";
        foreach (var item in units){ result += item.ToString() + ", "; }
        // Debug.Log(result);


		DefeatTargetVictoryCondition victoryCondition = owner.gameObject.AddComponent<DefeatTargetVictoryCondition>();
		Unit enemy = units[ 5 ];
		victoryCondition.target = enemy;
		Health health = enemy.GetComponent<Health>();
		health.MinHP = 10;
	}
}
