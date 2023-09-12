using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAbilityState : BattleState 
{
    public override void Enter (){
        base.Enter ();
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        StartCoroutine(Animate());
    }
    
    IEnumerator Animate (){
        // TODO play animations, etc
        yield return null;
        // TODO apply ability effect, etc
        // TemporaryAttackExample();
        ApplyAbility();
        
        if (turn.hasUnitMoved){
            owner.ChangeState<SelectUnitState>();
        }
            // owner.ChangeState<EndFacingState>();
        else
            owner.ChangeState<CommandSelectionState>();
    }
    
    void TemporaryAttackExample ()
    {
        // Debug.Log("temporrary attacking");
        for (int i = 0; i < turn.targets.Count; ++i) {
            GameObject obj = turn.targets[i].content;
            Stats stats = obj != null ? obj.GetComponentInChildren<Stats>() : null;
            if (stats != null){
                stats[StatTypes.HP] -= 50;
                if (stats[StatTypes.HP] <= 0)
                    Debug.Log("Killed unit!", obj);
            }
        }
    }
    void ApplyAbility ()
    {
        BaseAbilityEffect[] effects = turn.selectedAbility.GetComponentsInChildren<BaseAbilityEffect>();
        bool[] effectHit = new bool[effects.Length];
        for (int i = 0; i < turn.targets.Count; ++i)
        {
            Tile target = turn.targets[i];
            for (int j = 0; j < effects.Length; ++j)
            {
                BaseAbilityEffect currentEffect = effects[j];
                AbilityEffectTarget targeter = currentEffect.GetComponent<AbilityEffectTarget>();
                if (targeter.IsTarget(target))
                {
                    if(currentEffect.isSubEffect){
                        //Checks if the previous, ie main effect hit successfully
                        if(!effectHit[j-1]){
                            //previous effect did not hit, skip the bonus effect
                            continue;
                        }
                    }
                    HitRate rate = currentEffect.GetComponent<HitRate>();
                    float chance = rate.CalculateHitRate(target);
                    if (Random.Range(0, 101) > chance)
                    {
                        // A Miss!
                        continue;
                    }
                    effectHit[j] = true;
                    currentEffect.Apply(target);
                }
            }
        }
    }
}