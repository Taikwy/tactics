using UnityEngine;
using System.Collections;

public class PlanOfAttack 
{
	public Ability ability;
	public Targets target;
	public Point moveLocation;
	public Point fireLocation;
	public Directions attackDirection;

	// public bool canPerformAbility;
	public bool validTargetsLeft;
	public enum SubAction{
		PASS,
		FOCUS,
	}
	public enum SubMovement{
		PASS,
		ALLY,
		FOE,
		RANDOM,

	}
	public SubAction subAction;
	public bool targets;
	public SubMovement subMovement;

}
