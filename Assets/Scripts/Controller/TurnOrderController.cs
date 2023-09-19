using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderController : MonoBehaviour 
{


    const int baseActionGauge = 10000;
    const int moveCost = 2000;
    const int actionCost = 3000;


    const int turnActivation = 1000;
    const int turnCost = 500;

    public const string RoundBeganEvent = "TurnOrderController.roundBegan";
	public const string TurnCheckEvent = "TurnOrderController.turnCheck";
	public const string TurnBeganEvent = "TurnOrderController.turnBegan";
	public const string TurnCompletedEvent = "TurnOrderController.turnCompleted";
	public const string RoundEndedEvent = "TurnOrderController.roundEnded";

    public IEnumerator Round (){
        BattleController battleController = GetComponent<BattleController>();
        while (true){
			this.PostEvent(RoundBeganEvent);
            List<Unit> units = new List<Unit>( battleController.units );
            SortUnitsTurnOrder(units);

            //if the first unit value is not 0 yet
            if(!CanAct(units[0])){
                int decrementAV = units[0].GetComponent<Stats>()[StatTypes.AV];
                //decrement unit action value by the first unit's remaining AV
                for (int i = 0; i < units.Count; ++i) {
                    Stats s = units[i].GetComponent<Stats>();
                    s[StatTypes.AV] -= decrementAV;
                }
                SortUnitsTurnOrder(units);
            }


            //Loops thru all 
            for (int i = units.Count - 1; i >= 0; --i){
                //If the current units AV is below 0, it can act and will change to its turn
                if (CanAct(units[i])){
                    battleController.turn.Change(units[i]);
					units[i].PostEvent(TurnBeganEvent);
                    yield return units[i];

                    //Calculation for setting the new AV
                    Stats statsScript = units[i].GetComponent<Stats>();
                    int actionGauge = baseActionGauge;
                    if (battleController.turn.hasUnitMoved)
                        actionGauge += moveCost;
                    if (battleController.turn.hasUnitActed)
                        actionGauge += battleController.turn.actionCost;
                    
                    float AV = actionGauge / Mathf.Clamp(statsScript[StatTypes.SP], 50, 200) ;

                    //Sets the current unit its new AV
                    statsScript.SetValue(StatTypes.AV, (int)Mathf.Ceil(AV), false);

					units[i].PostEvent(TurnCompletedEvent);
                }
            }
        }
    }
    

    //Sorts unit by remaining Action Value
    public void SortUnitsTurnOrder(List<Unit> units){
        units.Sort( (a,b) => GetAV(a).CompareTo(GetAV(b)) );
    }

    public void RecalculateAV(Unit unit){
        Stats statsScript = unit.GetComponent<Stats>();
        int percentRemaining = statsScript[StatTypes.AV] * statsScript[StatTypes.SP];
        //need to get the original speed value used for the old AV
        float newAV = percentRemaining / Mathf.Clamp(statsScript[StatTypes.SP], 50, 200);
        statsScript.SetValue(StatTypes.AV, (int)Mathf.Ceil(newAV), false);
    }

    bool CanAct (Unit target){
        BaseException exc = new BaseException( GetAV(target) <= 0 );
        target.PostEvent( TurnCheckEvent, exc );
        return exc.toggle;
    }
    float GetAV (Unit target){
        return target.GetComponent<Stats>()[StatTypes.AV];
    }

    // bool CanTakeTurn (Unit target){
    //     BaseException exc = new BaseException( GetCounter(target) >= turnActivation );
    //     target.PostEvent( TurnCheckEvent, exc );
    //     return exc.toggle;
    // }
    
    int GetCounter (Unit target){
        return target.GetComponent<Stats>()[StatTypes.TurnCounter];
    }
}