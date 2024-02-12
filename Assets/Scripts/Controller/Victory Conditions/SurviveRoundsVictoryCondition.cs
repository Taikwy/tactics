using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurviveRoundsVictoryCondition : BaseVictoryCondition 
{
	public int roundsToSurvive;
	
	protected override void CheckForGameOver ()
	{
		base.CheckForGameOver ();
		if (Victor == Alliances.None && bc.turnOrderController.currentRound >= roundsToSurvive){
			Victor = Alliances.Ally;
            // Debug.Log(bc.turnOrderController.currentRound + " " + roundsToSurvive);
			Debug.Log("ALLIES WIN!");
		}
	}
}
