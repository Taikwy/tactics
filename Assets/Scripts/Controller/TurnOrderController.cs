using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnOrderController : MonoBehaviour 
{
    //total baseactiongauge is 8000, adjusted for moving. 
    const int baseActionGauge = 8000;
    const int moveCost = 1500;
    const int actionCost = 1500;
    // const int focusCost = 1000;
    const int roundAVCost = 100;
    int AVCounter = 100;
    public int currentRound = 0;

    const int turnActivation = 1000;
    const int turnCost = 500;
    const int minSPEED = 50;
    const int maxSPEED = 200;

    public const string RoundBeganEvent = "TurnOrderController.roundBegan";
	public const string TurnCheckEvent = "TurnOrderController.turnCheck";
	public const string TurnBeganEvent = "TurnOrderController.turnBegan";
	public const string TurnCompletedEvent = "TurnOrderController.turnCompleted";
	public const string RoundEndedEvent = "TurnOrderController.roundEnded";

	public const string AVChangedEvent = "TurnOrderController.AVChanged";
	public const string SpeedChangedEvent = "TurnOrderController.speedChanged";
	public const string GetSpeedEvent = "BaseAbilityEffect.GetSpeedEvent";
    // public List<Unit> units;
    
    void OnEnable(){
		this.AddObserver(OnSpeedChange, SpeedChangedEvent);
		this.AddObserver(OnGetSpeed, GetSpeedEvent);
    }void OnDisable(){ 
		this.RemoveObserver(OnSpeedChange, SpeedChangedEvent);
		this.RemoveObserver(OnGetSpeed, GetSpeedEvent);
    }

    public IEnumerator Round (){
        BattleController battleController = GetComponent<BattleController>();
        while (true){
            //if AV is at 100, then a new round has started
            if(AVCounter == 100){
                currentRound++;
                Debug.Log("new round " + currentRound + " ========================================"); 
                this.PostEvent(RoundBeganEvent);
            }

            // units = new List<Unit>( battleController.units );
            List<Unit> units = new List<Unit>( battleController.units );
            SortUnitsTurnOrder(units);
            
            //if the first unit value is not 0 yet, decrements all units by the first unit's remaining AV so the first unit can take a turn and al other units move up the timeline
            if(!CanAct(units[0])){
                int decrementAV = units[0].GetComponent<Stats>()[StatTypes.AV];
                //decrement unit action value by the first unit's remaining AV. works bc the first unit is the soonest to act
                for (int i = 0; i < units.Count; ++i) {
                    Stats s = units[i].GetComponent<Stats>();
                    s[StatTypes.AV] -= decrementAV;
                }
                //in case a unit's speed or anything was affected. sorts everything so units with the least AV remaining are first
                SortUnitsTurnOrder(units);

                //decremets av counter
                AVCounter -= decrementAV;
                // Debug.Log(AVCounter);
            }           


            //Loops thru all units that can act
            for (int i = 0; i < units.Count; ++i){
                if(!units[i])
                    continue;
                //If the current units AV is below 0, it can act and will change to its turn
                if (CanAct(units[i])){
                    battleController.turn.Change(units[i]);
					units[i].PostEvent(TurnBeganEvent);
                    // Debug.Log("new turn");
                    yield return units[i];
                    //makes sure the unit is still alive. if it died just now, the unit will be null and we simply cycle to the next unit to take a turn
                    if(units[i]){
                        //Adjusts actiongauge for calculation based on unit actiosn during the turn
                        Stats statsScript = units[i].GetComponent<Stats>();
                        int adjustedActionGauge = baseActionGauge;
                        if (battleController.turn.hasUnitMoved)
                            adjustedActionGauge += moveCost;
                        if (battleController.turn.hasUnitActed)
                            adjustedActionGauge += actionCost;
                        // else if(battleController.turn.hasUnitFocused)
                        //     adjustedActionGauge += focusCost;
                            
                            // actionGauge += battleController.turn.actionCost;
                        
                        
                        CalculateAV(units[i], adjustedActionGauge);
                        // float AV = adjustedActionGauge / Mathf.Clamp(statsScript[StatTypes.SP], minSPEED, maxSPEED) ;
                        // //Sets the current unit its new AV
                        // statsScript.SetValue(StatTypes.AV, (int)Mathf.Ceil(AV), false);
                        // Debug.Log(statsScript.gameObject.name + "'s AV = "+ AV);

                        units[i].PostEvent(TurnCompletedEvent);
                    }
                    else{
                        Debug.LogError("oops, unit[i] is nul");
                    }
                }
            }
            //if av has gone below 0, means that 100Av has passed and the round has ended. AFTER units have acted, since 100AV on turn 1 still lets u act once before a new turn
            if(AVCounter <= 0){
                Debug.Log("round " + currentRound + " ended ___________________________________________________________");
                this.PostEvent(RoundEndedEvent);
                AVCounter = 100;
            }

            
        }
    }

    //Sorts unit by remaining Action Value
    public void SortUnitsTurnOrder(List<Unit> units){
        units.Sort( (a,b) => GetAV(a).CompareTo(GetAV(b)) );

        // string result = "sorted units : ";
        // foreach (var item in units){ result += item.ToString() + ", "; }
        // Debug.Log(result);
    }

    public void SetupUnitsAV(List<Unit> units){
        // Debug.Log("setting up units AV");
        foreach(Unit unit in units){
            CalculateAV(unit);
        }
    }
    public void CalculateAV(Unit unit, int actionGauge = baseActionGauge){
        Stats statsScript = unit.GetComponent<Stats>();
        
        float newAV = actionGauge / GetSpeed(unit);
        // float AV = actionGauge / Mathf.Clamp(statsScript[StatTypes.SP], minSPEED, maxSPEED) ;

        statsScript.SetValue(StatTypes.AV, (int)Mathf.Ceil(newAV), false);
        // Debug.Log("calculating " + statsScript.gameObject.name + "'s AV = "+ AV);
        
		unit.PostEvent(AVChangedEvent);

        print("CALCULATING NEW AV " + unit + " | " + newAV);
    }

    //current version's equation is oldAV / ( adjSP / baseSP )
    public void RecalculateAV(Unit unit){
        // float percentRemaining = GetAV(unit) * GetSpeed(unit) / baseActionGauge;
        int adjSpeed = GetSpeed(unit);
        float percentRemaining = (float)adjSpeed / unit.GetComponent<Stats>()[StatTypes.SP];
        int adjustedAV = (int)( GetAV(unit) / percentRemaining );


        // Debug.Log("RECALCULATING AV " + GetAV(unit) + " | " + unit.GetComponent<Stats>()[StatTypes.SP]);
        unit.GetComponent<Stats>().SetValue(StatTypes.AV, adjustedAV, false);
        // int adjustedActionGauge = (int)Mathf.Ceil(percentRemaining*baseActionGauge);
        // CalculateAV(unit, adjustedActionGauge);
        // Debug.Log("NEW av " + adjustedAV + " | "+  adjSpeed + " | " + percentRemaining + "% | " );

        // Stats statsScript = unit.GetComponent<Stats>();
        // int percentRemaining = statsScript[StatTypes.AV] * statsScript[StatTypes.SP];
        // //need to get the original speed value used for the old AV
        // float newAV = percentRemaining / Mathf.Clamp(statsScript[StatTypes.SP], minSPEED, maxSPEED);
        // statsScript.SetValue(StatTypes.AV, (int)Mathf.Ceil(newAV), false);
		unit.PostEvent(AVChangedEvent);
    }

    bool CanAct (Unit target){
        BaseException exc = new BaseException( GetAV(target) <= 0 );
        target.PostEvent( TurnCheckEvent, exc );
        return exc.toggle;
    }
    int GetAV (Unit target){
        return target.GetComponent<Stats>()[StatTypes.AV];
    }
    void OnGetSpeed(object sender, object args){
        MonoBehaviour obj = sender as MonoBehaviour;
        var info = args as Info<Unit, Unit, List<ValueModifier>>;
        info.arg2.Add( new AddValueModifier(0, obj.GetComponent<Stats>()[StatTypes.SP]) );
        // Debug.Log("ON GET SPEED: " + obj);
    }

    int GetSpeed(Unit unit){
        // Debug.Log(unit + "getting unit's speed " + unit.GetComponent<Stats>()[StatTypes.SP]);
		var modifiers = new List<ValueModifier>();															//list of all modifiers, INCLUDING base stat (ie unit's stat would jhsut be an addvaluemodifier with that stat)
		var info = new Info<Unit, Unit, List<ValueModifier>>(unit, null, modifiers);
		unit.PostEvent(GetSpeedEvent, info);																	//posts the event, power script auto adds the modifier for the base stat
		modifiers.Sort(Compare);
		
        //applies all the modifiers to the value
		float value = 0;
		for (int i = 0; i < modifiers.Count; ++i)
			value = modifiers[i].Modify(0, value);
		
        //floors value as an int and clamps within damage range
		int retValue = Mathf.FloorToInt(value);
		retValue = Mathf.Clamp(retValue, minSPEED, maxSPEED);
        // Debug.Log("adjusted speed " + retValue);
		return retValue;
    }
    //returns the modifier that should trigger first
	int Compare (ValueModifier x, ValueModifier y){
		return x.sortOrder.CompareTo(y.sortOrder);
	}

    void OnSpeedChange(object sender, object args){
        MonoBehaviour obj = sender as MonoBehaviour;
        Debug.Log("speed and AV changed! " + obj + " | " + obj.transform.parent + " | " + transform);
        RecalculateAV(obj.GetComponentInParent<Unit>());

        GetComponent<BattleController>().timeline.UpdateTimeline(null, null);
    }
}