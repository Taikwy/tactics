using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq.Expressions;

public class CommandInfoPanel : MonoBehaviour 
{
    public GameObject panelBG;
    public TMP_Text descLabel;
    
    //takes in ability gameobject
    public void Display (string desc){
        descLabel.text = desc;
    }

    public void ShowPanel(){
        // Debug.Log("showing ability panel");
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        panelBG.SetActive(false);
    }
}