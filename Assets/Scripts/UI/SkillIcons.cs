using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcons : MonoBehaviour
{
    public Color fillColor, bgColor;
    public Image[] icons = new Image[5];
    public Image[] iconBGs = new Image[5];
    
    void Awake(){
        for(int i = 0; i < iconBGs.Length; i++){
            icons[i] = iconBGs[i].transform.GetChild(0).GetComponent<Image>();
        }
        DisableFills();
        // DisableBGs();
    }

    public void FillIcons(int numIcons){
        DisableFills();
        // int num = (int)Mathf.Clamp(numIcons, 0f, 5f);
        for(int i = 0; i < numIcons; i++){
            if(i >= iconBGs.Length)
                return;
            icons[i].color = fillColor;
            icons[i].enabled = true;
        }
    }
    public void FillBgs(int numBgs){
        DisableBGs();
        // int num = (int)Mathf.Clamp(numIcons, 0f, 5f);
        for(int i = 0; i < numBgs; i++){
            if(i >= iconBGs.Length)
                return;
            iconBGs[i].color = bgColor;
            iconBGs[i].enabled = true;
        }
    }

    void DisableBGs(){
        foreach(Image i in iconBGs){
            i.enabled = false;
        }
    }
    void DisableFills(){
        // print("disabling fills " + icons.Length);
        // print(icons);
        for(int i = 0; i < icons.Length; i++){
            // print(icons[i]);
            icons[i].enabled =false;
        }
        // foreach(Image i in icons){
        //     print(i);
        //     i.enabled = false;
        // }
    }
}
