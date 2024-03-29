using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class InitBattleState : BattleState 
{
    public override void Enter (){
        base.Enter ();
        StartCoroutine(Init());
    }
    
    IEnumerator Init (){
        owner.turnOrderController = owner.gameObject.AddComponent<TurnOrderController>();
        owner.round = owner.turnOrderController.Round();
        
        // board.Load( levelData );
        // print(board);
        // print(levelData);
        board.Set(boardData, levelData );
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
        // string[] unitRecipes = new string[]{
        //     "Paladin",
        //     "Samurai",
        //     "Barbarian",
        //     "Shaman",
        //     "Alchemist",
        //     // "Shaman",

        //     // "Slime",
        //     // "Mushroom",
        //     "Kitsune",
        //     "Faerie",
        //     "Gorgon",
        //     "Sphinx",
        //     "Qinglong"
        //     // "Snake"
        // };
        string[] unitRecipes = new string[]{
            "Faerie",
            "Samurai",
            "Faerie",
            "Faerie",
            "Faerie",
            // "Shaman",

            // "Slime",
            // "Mushroom",
            "Faerie",
            "Faerie",
            "Faerie",
            "Faerie",
            "Faerie"
            // "Snake"
        };
        Point[] spawnLocations = new Point[]{
            new Point(4,4),
            new Point(5,3),
            new Point(6,3),
            new Point(7,2),
            new Point(8,2),
            // new Point(6,3),

            new Point(4,7),
            new Point(5,7),
            new Point(6,8),
            new Point(7,8),
            new Point(8,5),
            // new Point(6,8),
        };

        //create containers for units based on aliance
        GameObject unitContainer = new GameObject("Units");
		unitContainer.transform.SetParent(owner.transform);
        GameObject allyContainer = new GameObject("Allies");
		allyContainer.transform.SetParent(unitContainer.transform);
        GameObject enemyContainer = new GameObject("Enemies");
		enemyContainer.transform.SetParent(unitContainer.transform);
        GameObject neutralContainer = new GameObject("Neutrals");
		neutralContainer.transform.SetParent(unitContainer.transform);

        SpawnUnits(owner.levelRecipe.allyUnitRecipes, owner.levelRecipe.allySpawnLocations, allyContainer);
        SpawnUnits(owner.levelRecipe.enemyUnitRecipes, owner.levelRecipe.enemySpawnLocations, enemyContainer);

        // for (int i = 0; i < unitRecipes.Length; ++i)
        // {
        //     // GameObject instance = UnitFactory.Create(unitRecipes[i], Random.Range(1, 11));
        //     GameObject instance = UnitFactory.Create(unitRecipes[i], 1);
        //     Unit unitScript = instance.GetComponent<Unit>();
        //     unitScript.Init(board.GetTile(spawnLocations[i]));
        //     // turnOrderController.CalculateAV(unitScript);

        //     switch(instance.GetComponent<Alliance>().type){
        //         default:
        //             instance.transform.SetParent(unitContainer.transform);
        //             Debug.Log("Unit has no alliance set");
        //             break;
        //         case Alliances.Ally:
        //             instance.transform.SetParent(allyContainer.transform);
        //             break;
        //         case Alliances.Enemy:
        //             instance.transform.SetParent(enemyContainer.transform);
        //             break;
        //         case Alliances.Neutral:
        //             instance.transform.SetParent(neutralContainer.transform);
        //             break;
        //     }
        //     units.Add(unitScript);
        // }
        // string result = "Units: ";
        // foreach (var item in units){ result += item.ToString() + ", "; }
        // Debug.Log(result);

        turnOrderController.SetupUnitsAV(units);
        owner.timeline.PopulateTimeline(units);
        
        // SelectTile(units[0].tile.position);                  //this is prob unneeded, since i already select a tile later in select unit state. ig this is jsut for the intiial tile indicator "before" i make a unit take action?
    }

    void SpawnUnits(List<string> unitRecipes, List<Vector2> spawnLocations, GameObject container){
        for (int i = 0; i < unitRecipes.Count; ++i)
        {
            GameObject instance = UnitFactory.Create(unitRecipes[i], 1);
            Unit unitScript = instance.GetComponent<Unit>();
            instance.transform.SetParent(container.transform);
            Point pos = new Point((int)spawnLocations[i].x, (int)spawnLocations[i].y);
            unitScript.Init(board.GetTile(pos));

            units.Add(unitScript);
        }
    }

    void AddVictoryCondition (){
        // string result = "Victory Units: ";
        // foreach (var item in units){ result += item.ToString() + ", "; }
        // Debug.Log(result);

		AddDefeatAll();
        // AddTarget();
        // AddSurvive();
	}
    void AddDefeatAll(){
        DefeatAllVictoryCondition victoryCondition = owner.gameObject.AddComponent<DefeatAllVictoryCondition>();
    }
    void AddTarget(){
        DefeatTargetVictoryCondition victoryCondition = owner.gameObject.AddComponent<DefeatTargetVictoryCondition>();
		Unit enemy = units[ 3 ];
		victoryCondition.target = enemy;
    }
    void AddSurvive(){
        SurviveRoundsVictoryCondition victoryCondition = owner.gameObject.AddComponent<SurviveRoundsVictoryCondition>();
		victoryCondition.roundsToSurvive = 5;
    }

}
