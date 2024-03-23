using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattleState : BattleState 
{
	public override void Enter (){
		base.Enter ();
		print("entering end battle state");
		guiController.ShowGameOver();
        // SceneManager.LoadScene("TestMainMenu");
	}
}
