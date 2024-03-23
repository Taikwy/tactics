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
    public Color damageColor, debuffColor, buffColor;
    public List<Sprite> statusIcons;
    [HideInInspector] public Color statusColor;
    [HideInInspector] public UnityEngine.Events.UnityAction highlightFunc, unhighlightFunc;

    public void Setup(string effectName){
        effectName = effectName.ToUpper();
        switch(effectName){
            default:
                Debug.LogError("uncrecognized effect name");
                icon.sprite = statusIcons[0];
                statusColor = Color.white;
                break;
            case "BREAK":
                icon.sprite  = statusIcons[1];
                statusColor = debuffColor;
                break;
            case "CRUSH":
                icon.sprite  = statusIcons[2];
                statusColor = debuffColor;
                break;
            case "BURN":
                icon.sprite  = statusIcons[3];
                statusColor = damageColor;
                break;
            case "TOXIC":
                icon.sprite  = statusIcons[4];
                statusColor = damageColor;
                break;
            case "SHOCK":
                icon.sprite  = statusIcons[5];
                statusColor = damageColor;
                break;
            case "COLD":
                icon.sprite  = statusIcons[6];
                statusColor = debuffColor;
                break;
            case "STRENGTHEN":
                icon.sprite  = statusIcons[7];
                statusColor = buffColor;
                break;
            case "FORTIFY":
                icon.sprite  = statusIcons[8];
                statusColor = buffColor;
                break;
            case "HASTE":
                icon.sprite  = statusIcons[9];
                statusColor = buffColor;
                break;
            case "RESTORE":
                icon.sprite  = statusIcons[10];
                statusColor = buffColor;
                break;
            case "HALT":
                icon.sprite  = statusIcons[11];
                statusColor = debuffColor;
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
