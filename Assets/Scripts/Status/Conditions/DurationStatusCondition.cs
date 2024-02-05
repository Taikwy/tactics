using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationStatusCondition : StatusCondition 
{
    public int duration = 0;
    protected virtual void OnEnable (){
        this.AddObserver(OnNewTurn, TurnOrderController.RoundBeganEvent);
    }
    protected virtual void OnDisable (){
        this.RemoveObserver(OnNewTurn, TurnOrderController.RoundBeganEvent);
    }
    protected virtual void OnNewTurn (object sender, object args){
        duration--;
        if (duration <= 0)
            Remove();
    }
}