using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitRate : MonoBehaviour 
{
	/// <summary>
	/// Includes a toggleable MatchException argument which defaults to false.
	/// </summary>
	public const string AutomaticHitCheckNotification = "HitRate.AutomaticHitCheckNotification";

	/// <summary>
	/// Includes a toggleable MatchException argument which defaults to false.
	/// </summary>
	public const string AutomaticMissCheckNotification = "HitRate.AutomaticMissCheckNotification";

	/// <summary>
	/// Includes an Info argument with three parameters: Attacker (Unit), Defender (Unit), 
	/// and Defender's calculated Evade / Resistance (int).  Status effects which modify Hit Rate
	/// should modify the arg2 parameter.
	/// </summary>
	public const string StatusCheckNotification = "HitRate.StatusCheckNotification";

	public virtual bool IsAngleBased { get { return false; }}
	protected Unit attacker;
    public bool guaranteed = false;
    public float abilityHitRate;

	protected virtual void Start ()
	{
		attacker = GetComponentInParent<Unit>();
	}
	/// <summary>
	/// Returns a value in the range of 0 t0 100 as a percent chance of
	/// an ability succeeding to hit
	/// </summary>
	public abstract float CalculateHitRate (Tile target);
	
    //im assuming this replaces the thingy in perform ability state
	public virtual bool RollForHit (Tile target)
	{
		if(guaranteed)
			return true;
		// float roll = Random.Range(0, 1f);
		float roll = Random.Range(0, 1f);
		float chance = CalculateHitRate(target)/100.0f;
        Debug.Log("rolled for hit " + roll + " " + chance);
		return roll <= chance;
	}
	// protected virtual bool AutomaticHit (Unit target)
	// {
	// 	MatchException exc = new MatchException(attacker, target);
	// 	this.PostEvent(AutomaticHitCheckNotification, exc);
	// 	return exc.toggle;
	// }

	// protected virtual bool AutomaticMiss (Unit target)
	// {
	// 	MatchException exc = new MatchException(attacker, target);
	// 	this.PostEvent(AutomaticMissCheckNotification, exc);
	// 	return exc.toggle;
	// }

	protected virtual int AdjustForStatusEffects (Unit target, int rate)
	{
		Info<Unit, Unit, int> args = new Info<Unit, Unit, int>(attacker, target, rate);
		this.PostEvent(StatusCheckNotification, args);
		return args.arg2;
	}

}