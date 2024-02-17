using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatIncrementStatusEffect : StatModifyStatusEffect 
{
	protected override void OnStatWillChange (object sender, object args){
		Debug.Log("APPLYING " + statType + " CHANGE " + incrementOrMultiply + " INCREMENT");
		ValueChangeException exc = args as ValueChangeException;
		AddValueModifier m = new AddValueModifier(0, incrementOrMultiply);
		exc.AddModifier(m);
	}
}
