using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TMP_Text turns;
    public List<Sprite> statusIcons;
    [HideInInspector] public UnityEngine.Events.UnityAction highlightFunc, unhighlightFunc;

    public void SetImage(string effectName){
        effectName = effectName.ToUpper();
        switch(effectName){
            default:
                icon.sprite = statusIcons[0];
                break;
            case "BREAK":
                icon.sprite  = statusIcons[1];
                break;
            case "CRUSH":
                icon.sprite  = statusIcons[2];
                break;
            case "BURN":
                icon.sprite  = statusIcons[3];
                break;
            case "TOXIC":
                icon.sprite  = statusIcons[4];
                break;
            case "SHOCK":
                icon.sprite  = statusIcons[5];
                break;
            case "COLD":
                icon.sprite  = statusIcons[6];
                break;
            case "STRENGTHEN":
                icon.sprite  = statusIcons[7];
                break;
            case "FORTIFY":
                icon.sprite  = statusIcons[8];
                break;
            case "HASTE":
                icon.sprite  = statusIcons[9];
                break;
            case "RESTORE":
                icon.sprite  = statusIcons[10];
                break;
            case "HALT":
                icon.sprite  = statusIcons[11];
                break;
        }
    }

    public void OnPointerEnter( PointerEventData eventData ){
        Debug.Log("hovering " +gameObject.name);
        if(highlightFunc != null)
            highlightFunc();

    }
    public void OnPointerExit( PointerEventData eventData ){
        // Debug.Log("unhovering " +gameObject.name);
        if(unhighlightFunc != null)
            unhighlightFunc();
    }
}
