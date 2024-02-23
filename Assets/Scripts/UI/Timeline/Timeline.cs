using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.ComponentModel;

public class Timeline : MonoBehaviour
{
    //refs to the actual prefabs of the indicators
    public GameObject container;
    public GameObject indicatorPrefab;
    BattleController owner;
    public List<TurnIndicator> turnIndicators = new List<TurnIndicator>();

    void OnEnable (){        
        this.AddObserver(UpdateTimeline, TurnOrderController.TurnBeganEvent);
        this.AddObserver(UpdateTimeline, TurnOrderController.AVChangedEvent);
        this.AddObserver(UpdateTimeline, Stats.DidChangeEvent(StatTypes.AV));

        owner = GetComponentInParent<BattleController>();
	}

	void OnDisable (){
		this.RemoveObserver(UpdateTimeline, TurnOrderController.TurnBeganEvent);
        this.RemoveObserver(UpdateTimeline, TurnOrderController.AVChangedEvent);
        this.RemoveObserver(UpdateTimeline, Stats.DidChangeEvent(StatTypes.AV));
	}

    //initial stuff, sets things up like adding the round stuff
    public void PopulateTimeline(List<Unit> units){
        
        turnIndicators = new List<TurnIndicator>();
        foreach(Unit unit in units){
            turnIndicators.Add(CreateUnitIndicator(unit));
        }
        CreateRoundIndicator();

        //sort all the indicators
        UpdateTimeline(null, null);             //just fucking duct tape. no clue how to use events properly but hopefully this works 

        // string result = "Units after timeline: ";
        // foreach (var item in units){ result += item.ToString() + ", "; }
        // Debug.Log(result);
    }
    
    public TurnIndicator CreateUnitIndicator(Unit unit){
        GameObject obj = Instantiate(indicatorPrefab, container.transform);
        // obj.transform.SetAsFirstSibling();
		obj.name = unit.name;

        TurnIndicator indicator = obj.GetComponent<TurnIndicator>();
        indicator.statsScript = unit.statsScript;
        indicator.unitScript = unit;
        indicator.icon.sprite = unit.portrait;
        indicator.icon.color = unit.GetComponent<Unit>().portraitColor;
        // Debug.Log("setting timeline color as "+ indicator.icon.color);

        indicator.counter.text = GetAV(indicator).ToString();
        
        return indicator;
    }

    public void CreateRoundIndicator(){}
    //when units or turn or round has changed, update the timeline. handles cases and removes or adds and updates however it sees fit
    public void UpdateTimeline(object sender, object args){
        // Debug.Log("updating timeline");
        foreach(TurnIndicator indicator in turnIndicators){
            indicator.counter.text = GetAV(indicator).ToString();
        }
        SortTimeline();

        //updates sibling order of indicators based on AV
        foreach(TurnIndicator indicator in turnIndicators){
            // Debug.Log(indicator.counter.text);
            indicator.gameObject.transform.SetAsFirstSibling();
            // Debug.Log("timeline color is "+ indicator.icon.color);
            indicator.background.color = indicator.defaultBGColor;
            if(owner.turn.actingUnit && indicator.unitScript == owner.turn.actingUnit)
                indicator.background.color = Color.white;
        }
        
    }
    //sorts all indicators by ascending AV count, so smallest first
    public void SortTimeline(){
        List<TurnIndicator> tempIndicators = turnIndicators;
        // for(int i = 0; i < turnIndicators.Count; i++){
        //     for(int j = 0; j < turnIndicators.Count; j++){}
        // }
        turnIndicators.Sort( (a,b) => GetAV(b).CompareTo(GetAV(a)) );
        // turnIndicators.Sort( (a,b) => GetAV(a).CompareTo(GetAV(b)) );
        // turnIndicators.Reverse();

        // string result = "turnindicators : ";
        // foreach (var item in turnIndicators){ result += item.ToString() + ", "; }
        // Debug.Log(result);

        // Debug.Log("sorted");
    }
    //returns the raw AV from the stats script
    int GetAV(TurnIndicator indicator){
        return indicator.statsScript[StatTypes.AV];
    }
    //returns the actual display of the turn indicator
    int GetCounter(TurnIndicator indicator){
        return int.Parse(indicator.counter.text);
    }

    //empties timeline and removes all the indicators
    public void EmptyTimeline(){
        foreach(TurnIndicator indicator in turnIndicators){
            RemoveIndicator(indicator);
        } 
        turnIndicators.Clear();
    }
    // public void AddUnit(){}
    public void RemoveIndicator(TurnIndicator indicator){
        // Debug.Log("removing indicator");
        turnIndicators.Remove(indicator);
        Destroy(indicator.gameObject);
    }
    public void RemoveUnit(Unit unit){
        int index = -1;
        for(int i = 0; i < turnIndicators.Count; i++){
            if(turnIndicators[i].unitScript.Equals(unit)){
                index = i;
            }
        }
        if(index != -1)
            RemoveIndicator(turnIndicators[index]);

    }
}
