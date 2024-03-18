using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnIndicator : MonoBehaviour
{
    public Image icon;
    public Image background;
    public Animator portraitAnimator;
    public Animator selectedAnimator;
    public Color defaultBGColor, defaultAVColor;
    public Image portraitBG;
    public TMP_Text counter;
    [HideInInspector] public Unit unitScript;
    [HideInInspector] public Stats statsScript;

    public void Select(){
        selectedAnimator.gameObject.SetActive(true);
    }
    public void Deselect(){
        selectedAnimator.gameObject.SetActive(false);
    }

}
