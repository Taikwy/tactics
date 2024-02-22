using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class StatusLabel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public UnityEngine.Events.UnityAction highlightFunc, unhighlightFunc;

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
