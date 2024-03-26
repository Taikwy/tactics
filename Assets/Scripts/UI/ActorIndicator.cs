using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorIndicator : MonoBehaviour
{
    public SpriteRenderer sr;
    // public Animator anim;
    // public RuntimeAnimatorController selectAnim;
    // public RuntimeAnimatorController targetAnim;

    public float transparentAlpha = .5f;
    Vector2 centerPos;
    Vector2 offset;
    public float moveSpeed;
    public float moveDistance;
    Vector2 movePos, topPos, botPos;
    bool movingUp;
    [HideInInspector] public BattleController bc{ get {return GetComponentInParent<BattleController>(); }}

    public void Reset(Vector2 offset){
        transform.localPosition = offset;
        this.offset = offset;
        // Debug.LogError(transform.localPosition + " | " + offset);
        // centerPos = transform.localPosition;
        // topPos = centerPos + new Vector2(0, moveDistance);
        // botPos = centerPos - new Vector2(0, moveDistance);
    }
    public void MakeTransparent(bool transparent){
        if(transparent){
            Color temp = sr.color;
            temp.a = transparentAlpha;
            sr.color = temp;
        }
        else{
            Color temp = sr.color;
            temp.a = 1;
            sr.color = temp;
        }
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
				sr.color = bc.defaultColor;
				break;
			case Alliances.Ally:
				sr.color = bc.playerColor;
				break;
			case Alliances.Enemy:
				sr.color = bc.enemyColor;
				break;
			case Alliances.Neutral:
				sr.color = bc.neutralColor;
				break;
		}
        sr.color = bc.board.selectValid;
    }

    
    
    // public void ChangeSelect(){
    //     anim.runtimeAnimatorController = selectAnim;
    // }
    // public void ChangeTarget(){
    //     anim.runtimeAnimatorController = targetAnim;
    // }
}
