using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDurationStatusCondition : StatusCondition 
{
    public int duration = 3;
    void OnEnable (){
        // this.AddObserver(OnNewTurn, TurnOrderController.RoundBeganNotification);
    }
    void OnDisable (){
        // this.RemoveObserver(OnNewTurn, TurnOrderController.RoundBeganNotification);
    }
    void OnNewTurn (object sender, object args){
        duration--;
        if (duration <= 0)
            Remove();
    }
}