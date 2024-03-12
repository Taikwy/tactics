using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectLabel : MonoBehaviour
{
    bool intialized = false;
    public TMP_Text effectLabel;
    public float floatSpeed, fadeTime;
    float fadeAmount;
    Vector2 movePos;

    public void Initialize(string labelText, float floatspeed, float fadeTime){
        effectLabel.text = labelText;
        int.TryParse(labelText, out int result);
        if(result > 0){
            effectLabel.color = Color.green;
        }
        else if(result < 0){
            effectLabel.color = Color.red;
        }
        else{
            if(labelText == "MISS!")
                effectLabel.color = Color.gray;
            else if(labelText == "PURIFYING!")
                effectLabel.color = Color.white;
            else if(labelText == "NO VALID TARGETS")
                effectLabel.color = Color.red;
            else
                effectLabel.color = Color.yellow;
        }
        // effectLabel.color = Color.white;
        // print("alpha: " + effectLabel.color.a);
        // print("position: " + gameObject.transform.position);

        this.floatSpeed = floatspeed;
        fadeAmount = 1/fadeTime;
        intialized = true;
    }

    void Update(){
        if(!intialized)
            return;
        if(effectLabel.color.a <= 0){
            // print("alpha at 0, destroying label " + effectLabel.text);
            Destroy(gameObject);

        }
        // print("updating");

        
        movePos = (Vector2)transform.position + new Vector2(0, 1f);
        transform.position = Vector2.MoveTowards(transform.position, movePos, floatSpeed*Time.deltaTime);

        Color newColor = effectLabel.color;
        newColor.a -= fadeAmount*Time.deltaTime;
        effectLabel.color = newColor;
    }

}
