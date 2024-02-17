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
	protected override void GetAttack (object sender, object args){
		if(IsAttacker(sender, args)){
			Debug.Log("incrementing attack! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, incrementOrMultiply) );
		}
	}
	protected override void GetDefense (object sender, object args){
		if(IsDefender(sender, args)){
			Debug.Log("incrementing defense! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, incrementOrMultiply) );
		}
	}
	protected override void GetCritRate (object sender, object args){
		if(IsAttacker(sender, args)){
			Debug.Log("incrementing critrate! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, incrementOrMultiply) );
		}
	}
	protected override void GetCritDMG (object sender, object args){
		if(IsAttacker(sender, args)){
			Debug.Log("incrementing critdmg! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, incrementOrMultiply) );
		}
	}
}
