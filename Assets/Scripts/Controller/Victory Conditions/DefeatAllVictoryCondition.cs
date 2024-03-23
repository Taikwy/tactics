using UnityEngine;
using System.Collections;

public class DefeatAllVictoryCondition : BaseVictoryCondition 
{
	protected override void CheckForGameOver ()
	{
		base.CheckForGameOver();
		if (Victor == Alliances.None && PartyDefeated(Alliances.Enemy)){
			// Debug.Log("==================");
			// Debug.Log(" fucking hello? " + PartyDefeated(Alliances.Enemy));
			Victor = Alliances.Ally;
			// Debug.Log("all enemies defeated, ALLIES WIN!!!");
			Debug.Log("ALLIES WIN!");
		}
	}
}
