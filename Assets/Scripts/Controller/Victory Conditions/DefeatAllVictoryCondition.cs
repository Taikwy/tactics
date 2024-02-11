using UnityEngine;
using System.Collections;

public class DefeatAllVictoryCondition : BaseVictoryCondition 
{
	protected override void CheckForGameOver ()
	{
		base.CheckForGameOver();
		if (Victor == Alliances.None && PartyDefeated(Alliances.Enemy))
			Victor = Alliances.Ally;
	}
}
