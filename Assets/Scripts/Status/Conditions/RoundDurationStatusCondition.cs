using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundDurationStatusCondition : DurationStatusCondition
{
    protected void OnEnable (){
        this.AddObserver(OnNewRound, TurnOrderController.RoundBeganEvent);
    }
    protected void OnDisable (){
        this.RemoveObserver(OnNewRound, TurnOrderController.RoundBeganEvent);
    }
    protected void OnNewRound (object sender, object args){
        duration--;
        if (duration <= 0)
            Remove();
    }

}
