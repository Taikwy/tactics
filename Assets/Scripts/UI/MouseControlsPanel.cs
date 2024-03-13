using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseControlsPanel : MonoBehaviour
{
    public GameObject panelBG;
    [SerializeField]  TMP_Text LMBLabel, RMBLabel;
    
    public void Display (string LMB, string RMB){

        LMBLabel.text = "LMB:";
        LMBLabel.text = string.Format("LMB:{0}", LMB);
        RMBLabel.text = "RMB:";
        RMBLabel.text = string.Format("RMB:{0}", RMB);
    }
    public void ShowPanel(){
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        panelBG.SetActive(false);
    }
}
