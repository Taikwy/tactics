using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformStateUI : MonoBehaviour
{
    [Header("perfrom state timing stuff")]
    public float abilityDisplayDelay;
    public float performDelay;
    public float focusStartDelay, focusEndDelay;
    public float effectDisplayDelay;
    [Header("effect label stuff")]
    public GameObject effectLabelPrefab;
    public GameObject effectLabelContainer;
    public float effectFloatSpeed, effectFadeSpeed;
    public Vector2 unitEffectLabelOffset;

    public void DisplayEffect (Tile target, string label){
        print("displaying effect");
        Vector2 targetPos = (Vector2)target.transform.position + unitEffectLabelOffset;
        Unit unit = target.content.GetComponent<Unit>();
        GameObject effectLabel = Instantiate(effectLabelPrefab, targetPos, Quaternion.identity, unit.canvasObj);
        effectLabel.GetComponent<EffectLabel>().Initialize(label, effectFloatSpeed, effectFadeSpeed);
        // print(effect[targetIndex] + " | pos " + targetPos);
    }

    // IEnumerator DisplayEffects (Tile target){
    //     int targetIndex = turn.targets.IndexOf(target);
    //     // print("displaying effect " + targetIndex + " ______________________");
    //     for(int effectIndex = 0; effectIndex < effects[targetIndex].Count; effectIndex++){
    //         // Vector2 labelOffset = new Vector2(Random.Range(-.2f,.2f), Random.Range(.55f,.6f));
    //         Vector2 targetPos = (Vector2)target.transform.position + unitEffectLabelOffset;
    //         Unit unit = target.content.GetComponent<Unit>();
    //         GameObject effectLabel = Instantiate(effectLabelPrefab, targetPos, Quaternion.identity, unit.canvasObj);
    //         effectLabel.GetComponent<EffectLabel>().Initialize(effects[targetIndex][effectIndex], effectFloatSpeed, effectFadeSpeed);
    //         // print(effects[targetIndex][effectIndex] + " | pos " + targetPos);
    //         yield return new WaitForSeconds(.2f);
    //     }
    //     yield return null;
    // }
}
