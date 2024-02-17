using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMultiplyStatusEffect : StatModifyStatusEffect
{
	protected override void OnStatWillChange  (object sender, object args){
		Debug.Log("APPLYING " + statType + " CHANGE " + incrementOrMultiply + " MULTIPLY");
		ValueChangeException exc = args as ValueChangeException;
		MultValueModifier m = new MultValueModifier(0, incrementOrMultiply);
		exc.AddModifier(m);
	}
}
