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
	protected override void GetAttack (object sender, object args){
		if(IsAttacker(sender, args)){
			Debug.Log("multiplying attack! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new MultValueModifier(0, incrementOrMultiply) );
		}
	}
	protected override void GetDefense (object sender, object args){
		if(IsDefender(sender, args)){
			Debug.Log("multiplying defense! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new MultValueModifier(0, incrementOrMultiply) );
		}
	}
	protected override void GetCritRate (object sender, object args){
		if(IsAttacker(sender, args)){
			Debug.Log("multiplying critrate! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new MultValueModifier(0, incrementOrMultiply) );
		}
	}
	protected override void GetCritDMG (object sender, object args){
		if(IsAttacker(sender, args)){
			Debug.Log("multiplying critdmg! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new MultValueModifier(0, incrementOrMultiply) );
		}
	}
}
