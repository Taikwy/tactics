using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndStatusCondition : DurationStatusCondition 
{
    protected void OnEnable (){
        this.AddObserver(OnTurnEnd, TurnOrderController.TurnCompletedEvent);
    }
    protected void OnDisable (){
        this.RemoveObserver(OnTurnEnd, TurnOrderController.TurnCompletedEvent);
    }
    protected void OnTurnEnd (object sender, object args){
        //makes it so its updating when its the new turn of the inflicted unit and not just any unit
        if(!sender.Equals(transform.parent.GetComponent<Unit>()))
            return;
        // print("turn ended, decrementing duration");
        duration--;
        if (duration <= 0)
            Remove();
    }
}