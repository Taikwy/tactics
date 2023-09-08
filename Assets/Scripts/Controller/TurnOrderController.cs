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


    public IEnumerator Round (){
        BattleController battleController = GetComponent<BattleController>();
        while (true){
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
                //If one of the units AV is below 0, it can act and will change to its turn
                if (CanAct(units[i])){
                    battleController.turn.Change(units[i]);
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

    public IEnumerator OldRound (){
        BattleController bc = GetComponent<BattleController>();;
        while (true){
            // this.PostNotification(RoundBeganNotification);
            List<Unit> units = new List<Unit>( bc.units );
            for (int i = 0; i < units.Count; ++i) {
                Stats s = units[i].GetComponent<Stats>();
                s[StatTypes.TurnCounter] += s[StatTypes.SP];
            }

            units.Sort( (a,b) => GetCounter(a).CompareTo(GetCounter(b)) );

            for (int i = units.Count - 1; i >= 0; --i){
                if (CanTakeTurn(units[i])){
                    bc.turn.Change(units[i]);
                    yield return units[i];

                    int cost = turnCost;
                    if (bc.turn.hasUnitMoved)
                        cost += moveCost;
                    if (bc.turn.hasUnitActed)
                        cost += actionCost;
                    Stats s = units[i].GetComponent<Stats>();
                    s.SetValue(StatTypes.TurnCounter, s[StatTypes.TurnCounter] - cost, false);
                    // units[i].PostNotification(TurnCompletedNotification);
                }
            }
            
            // this.PostNotification(RoundEndedNotification);
        }
    }
    bool CanAct (Unit target){
        BaseException exc = new BaseException( GetAV(target) <= 0 );
        return exc.toggle;
    }
    float GetAV (Unit target){
        return target.GetComponent<Stats>()[StatTypes.AV];
    }

    bool CanTakeTurn (Unit target){
        BaseException exc = new BaseException( GetCounter(target) >= turnActivation );
        // target.PostNotification( TurnCheckNotification, exc );
        return exc.toggle;
    }
    
    int GetCounter (Unit target){
        return target.GetComponent<Stats>()[StatTypes.TurnCounter];
    }
}