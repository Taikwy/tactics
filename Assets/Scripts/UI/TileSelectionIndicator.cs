using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSelectionIndicator : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator anim;
    public RuntimeAnimatorController selectAnim;
    public RuntimeAnimatorController targetAnim;
    // void Awake(){
    //     anim.clip = selectAnim;
    // }

    public void ChangeSelect(){
        // print("change tile select to select");
        // anim.contr
        // anim.clip = selectAnim;
        anim.runtimeAnimatorController = selectAnim;
    }
    public void ChangeTarget(){
        // print("change tile select to target");
        anim.runtimeAnimatorController = targetAnim;
        // anim.clip = targetAnim;
    }
}
