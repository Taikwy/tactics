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
            s[StatTypes.BP] -= cost;
        }
        else
            Debug.LogError("Ability contains burst cost but is not burst type");
    }
}