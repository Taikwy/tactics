using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatIncrementStatusEffect : StatModifyStatusEffect 
{
	protected override void OnGetAttack (object sender, object args){
		if(IsAttacker(sender, args)){
			Debug.Log("incrementing attack! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, incrementOrMultiply) );
		}
	}
	protected override void OnGetDefense (object sender, object args){
		if(IsDefender(sender, args)){
			Debug.Log("incrementing defense! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, incrementOrMultiply) );
		}
	}
	protected override void OnGetCritRate (object sender, object args){
		if(IsAttacker(sender, args)){
			Debug.Log("incrementing critrate! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, incrementOrMultiply) );
		}
	}
	protected override void OnGetCritDMG (object sender, object args){
		if(IsAttacker(sender, args)){
			Debug.Log("incrementing critdmg! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(0, incrementOrMultiply) );
		}
	}
}
