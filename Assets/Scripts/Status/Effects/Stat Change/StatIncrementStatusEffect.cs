using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatIncrementStatusEffect : StatModifyStatusEffect 
{
	protected override void OnGetAttack (object sender, object args){
		if(IsAttacker(sender, args) || IsActor(sender, args)){
			// Debug.Log("on get incrementing attack! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(1, incrementOrMultiply) );
		}
	}
	protected override void OnGetDefense (object sender, object args){
		if(IsDefender(sender, args) || IsActor(sender, args)){
			// Debug.Log("on get incrementing defense! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(1, incrementOrMultiply) );
		}
	}
	protected override void OnGetCritRate (object sender, object args){
		if(IsAttacker(sender, args) || IsActor(sender, args)){
			// Debug.Log("on get incrementing critrate! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(1, incrementOrMultiply) );
		}
	}
	protected override void OnGetCritDMG (object sender, object args){
		if(IsAttacker(sender, args) || IsActor(sender, args)){
			// Debug.Log("on get incrementing critdmg! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(1, incrementOrMultiply) );
		}
	}
	protected override void OnGetSpeed (object sender, object args){
		// Debug.Log(sender as Unit);
		if((sender as Unit) == GetComponentInParent<Unit>() || IsActor(sender, args)){
			// Debug.Log("on get incrementing speed! " + incrementOrMultiply);
			var info = args as Info<Unit, Unit, List<ValueModifier>>;
			info.arg2.Add( new AddValueModifier(1, incrementOrMultiply) );
		}
	}
}
