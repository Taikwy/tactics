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
    Vector2 offset;
    public float moveSpeed;
    public float moveDistance;
    Vector2 movePos, topPos, botPos;
    bool movingUp;

    public void Reset(Vector2 offset){
        transform.localPosition = offset;
        this.offset = offset;
        // Debug.LogError(transform.localPosition + " | " + offset);
        // centerPos = transform.localPosition;
        // topPos = centerPos + new Vector2(0, moveDistance);
        // botPos = centerPos - new Vector2(0, moveDistance);
    }
    void Update(){
        // print(transform.localPosition.y + " | " + offset.y + " | " + movePos);
        if(transform.localPosition.y <= -moveDistance + offset.y)
            movingUp = true;
        else if(transform.localPosition.y >= moveDistance + offset.y)
            movingUp = false;
        if(movingUp)
            movePos = (Vector2)transform.localPosition + new Vector2(0, 1);
        else if(!movingUp)
            movePos =(Vector2)transform.localPosition +  new Vector2(0, -1);

        transform.localPosition = Vector2.MoveTowards(transform.localPosition, movePos, moveSpeed*Time.deltaTime);
        
    }
    
    
    // public void ChangeSelect(){
    //     anim.runtimeAnimatorController = selectAnim;
    // }
    // public void ChangeTarget(){
    //     anim.runtimeAnimatorController = targetAnim;
    // }
}
