using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySkillCost : MonoBehaviour 
{
    [Tooltip("ONLY APPLIES FOR SKILL AND WEAPON ABILITIES")]public int skillCost;
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
        // Debug.Log("can perform check");
        if (s[StatTypes.SK] < skillCost) {
            BaseException exc = (BaseException)args;
            exc.FlipToggle();
        }
    }
    void OnDidPerformNotification (object sender, object args) {
        Stats s = GetComponentInParent<Stats>();
        // Debug.Log("ability was performed, checking type " + owner.type);
        if(owner.type == AbilityTypes.BASIC){
            // Debug.Log("basic attack so gaining 1 skill point");
            s[StatTypes.SK]++;
        }
        else
            s[StatTypes.SK] -= skillCost;
    }
}