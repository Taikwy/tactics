using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class ActorIndicator : MonoBehaviour
{
    public SpriteRenderer sr;
    // public Animator anim;
    // public RuntimeAnimatorController selectAnim;
    // public RuntimeAnimatorController targetAnim;

    Vector2 centerPos;
    public float moveSpeed;
    public float moveDistance;
    Vector2 movePos, topPos, botPos;
    bool movingUp;

    public void Reset(){
        centerPos = transform.position;
        topPos = centerPos + new Vector2(0, moveDistance);
        botPos = centerPos - new Vector2(0, moveDistance);
    }
    void Update(){
        if((Vector2)transform.position == botPos)
            movingUp = true;
        else if((Vector2)transform.position == topPos)
            movingUp = false;
        if(movingUp)
            movePos = topPos;
        else if(!movingUp)
            movePos = botPos;

        transform.position = Vector2.MoveTowards(transform.position, movePos, moveSpeed*Time.deltaTime);
        
    }
    
    
    // public void ChangeSelect(){
    //     anim.runtimeAnimatorController = selectAnim;
    // }
    // public void ChangeTarget(){
    //     anim.runtimeAnimatorController = targetAnim;
    // }
}
