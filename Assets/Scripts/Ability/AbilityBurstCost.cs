using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBurstCost : MonoBehaviour 
{
    public int cost;
    Ability owner;
    void Awake (){
        owner = GetComponent<Ability>();
        Stats s = GetComponentInParent<Stats>();
    }
    void OnEnable (){
        this.AddObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.AddObserver(OnDidPerformNotification, Ability.DidPerformEvent, owner);
    }
    void OnDisable (){
        this.RemoveObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.RemoveObserver(OnDidPerformNotification, Ability.DidPerformEvent, owner);
    }
    void OnCanPerformCheck (object sender, object args){
        Stats s = GetComponentInParent<Stats>();
        // Debug.Log("can perform burst check");
        if (s[StatTypes.BP] < cost) {
            BaseException exc = (BaseException)args;
            exc.FlipToggle();
        }
    }
    void OnDidPerformNotification (object sender, object args) {
        Stats s = GetComponentInParent<Stats>();
        if(owner.type == AbilityTypes.BURST){
            //if cost is less than 0 it means it just uses the max burst meter of a character
            if(cost < 0)
                s[StatTypes.BP] = 0;
            else
                s[StatTypes.BP] -= cost;
        }
        else
            Debug.LogError("Ability contains burst cost but is not burst type");
    }
}