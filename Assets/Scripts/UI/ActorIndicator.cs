using System.Collections;
using System.Collections.Generic;
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

    public void ChangeColor(Unit unit){
        switch(unit.allianceScript.type){
			default:
				sr.color = Color.white;
				break;
			case Alliances.Ally:
				sr.color = new Color32 (0,255,248,255);
				break;
			case Alliances.Enemy:
				sr.color = new Color32 (255,0,124,255);
				break;
			case Alliances.Neutral:
				sr.color = Color.green;
				break;
		}
    }
    
    
    // public void ChangeSelect(){
    //     anim.runtimeAnimatorController = selectAnim;
    // }
    // public void ChangeTarget(){
    //     anim.runtimeAnimatorController = targetAnim;
    // }
}
