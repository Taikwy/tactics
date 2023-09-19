using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationStatusCondition : StatusCondition 
{
    public int duration = 3;
    void OnEnable (){
        this.AddObserver(OnNewTurn, TurnOrderController.RoundBeganEvent);
    }
    void OnDisable (){
        this.RemoveObserver(OnNewTurn, TurnOrderController.RoundBeganEvent);
    }
    void OnNewTurn (object sender, object args){
        if (duration <= 0)
            Remove();
        
        duration--;
    }
}