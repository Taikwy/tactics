using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [HideInInspector] public Unit unit;
    // [HideInInspector] public GameObject outline;
    [HideInInspector] public Animator idleAnimator;
    [HideInInspector] public Animator outlineAnimator;
    [HideInInspector] public SpriteRenderer idleRenderer;
    [HideInInspector] public SpriteRenderer outlineRenderer;

    void Start(){
        // idleRenderer = unit.GetComponent<SpriteRenderer>();
    }
    
}
