using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityDisplayPanel : MonoBehaviour
{
    public GameObject panelBG;
    public TMP_Text nameLabel;
    
    //takes in ability gameobject
    public void Display (GameObject ability){
        nameLabel.text = string.Format("{0}", ability.name);
    }

    public void ShowPanel(){
        // Debug.Log("showing ability display panel");
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        panelBG.SetActive(false);
    }
}
