using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnStartStatusCondition : DurationStatusCondition 
{
    protected void OnEnable (){
        this.AddObserver(OnTurnStart, TurnOrderController.TurnBeganEvent);
    }
    protected void OnDisable (){
        this.RemoveObserver(OnTurnStart, TurnOrderController.TurnBeganEvent);
    }
    protected void OnTurnStart (object sender, object args){
        //makes it so its updating when its the new turn of the inflicted unit and not just any unit
        if(!sender.Equals(transform.parent.GetComponent<Unit>()))
            return;
        duration--;
        if (duration <= 0)
            Remove();
    }
}