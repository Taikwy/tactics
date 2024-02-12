using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattleState : BattleState 
{
	public override void Enter (){
		base.Enter ();
        SceneManager.LoadScene("TestMainMenu");
	}
}
