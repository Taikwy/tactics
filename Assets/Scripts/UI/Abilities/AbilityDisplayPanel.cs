using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityDisplayPanel : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text nameLabel;
    
    //takes in ability gameobject
    public void Display (GameObject ability, bool focus = false){
        if(focus)
            nameLabel.text = string.Format("FOCUS");
        else if(ability)
            nameLabel.text = string.Format("{0}", ability.name);
    }

    public void ShowPanel(){
        // Debug.Log("showing ability display panel");
        panel.SetActive(true);
    }
    public void HidePanel(){
        // print("hiding ability panel");
        panel.SetActive(false);
    }
}
