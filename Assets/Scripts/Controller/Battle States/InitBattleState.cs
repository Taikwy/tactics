using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        SpawnTestUnits(); // This is new
        
        yield return null;
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();

        owner.ChangeState<SelectUnitState>(); // This is changed
    }

    void SpawnTestUnits ()
    {
        // OldSpawnUnits();
        // return;

        // string[] jobs = new string[]{"Rogue", "Warrior", "Wizard"};
        string[] jobs = new string[]{"Rogue", "Warriro", "yuhyuh"};
        for (int i = 0; i < jobs.Length; ++i){
            // Debug.Log("spawning " + jobs[i]);
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

    void OldSpawnUnits(){
        string[] jobs = new string[]{"Rogue", "Warrior", "Wizard"};
        for (int i = 0; i < jobs.Length; ++i){
            GameObject instance = Instantiate(owner.playerPrefab) as GameObject;
            Stats s = instance.AddComponent<Stats>();
            // s[StatTypes.LVL] = 1;
            GameObject jobPrefab = Resources.Load<GameObject>( "Jobs/" + jobs[i] );
            GameObject jobInstance = Instantiate(jobPrefab) as GameObject;
            jobInstance.transform.SetParent(instance.transform);
            Job job = jobInstance.GetComponent<Job>();
            job.Employ();
            job.LoadDefaultStats();
            Point p = new Point((int)levelData.tilePositions[i].x, (int)levelData.tilePositions[i].y);
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(board.GetTile(p));
            unit.Match();
            instance.AddComponent<WalkMovement>();
            units.Add(unit);

            unit.unitName = jobInstance.name;
            //    Rank rank = instance.AddComponent<Rank>();
            //    rank.Init (10);
        }
    }
}
