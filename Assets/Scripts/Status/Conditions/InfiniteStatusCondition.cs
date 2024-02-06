using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteStatusCondition : StatusCondition 
{
    protected virtual void OnEnable (){
        this.AddObserver(OnNewTurn, TurnOrderController.RoundBeganEvent);
    }
    protected virtual void OnDisable (){
        this.RemoveObserver(OnNewTurn, TurnOrderController.RoundBeganEvent);
    }
    protected virtual void OnNewTurn (object sender, object args){

    }
}