using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour 
{
	// public const string EffectInflictedEvent = "StatusEffect.EffectInflictedEvent";
	public const string EffectAppliedEvent = "StatusEffect.EffectAppliedEvent";
    public string statusName;
    [TextArea(5,20)]public string statusEffectDescription;
}
