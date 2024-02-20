using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySkillCost : MonoBehaviour 
{
    public int amount;
    Ability owner;
    void Awake (){
        owner = GetComponent<Ability>();
    }
    void OnEnable (){
        this.AddObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.AddObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }
    void OnDisable (){
        this.RemoveObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.RemoveObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }
    void OnCanPerformCheck (object sender, object args){
        Stats s = GetComponentInParent<Stats>();
        Debug.Log("can perform check");
        if (s[StatTypes.SK] < amount) {
            BaseException exc = (BaseException)args;
            exc.FlipToggle();
        }
    }
    void OnDidPerformNotification (object sender, object args) {
        Stats s = GetComponentInParent<Stats>();
        s[StatTypes.SK] -= amount;
    }
}