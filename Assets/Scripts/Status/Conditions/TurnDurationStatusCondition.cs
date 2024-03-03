using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDurationStatusCondition : DurationStatusCondition 
{
    protected void OnEnable (){
        this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganEvent);
    }
    protected void OnDisable (){
        this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganEvent);
    }
    protected void OnNewTurn (object sender, object args){
        //makes it so its updating when its the new turn of the inflicted unit and not just any unit
        if(!sender.Equals(transform.parent.GetComponent<Unit>()))
            return;
        duration--;
        if (duration <= 0)
            Remove();
    }
}