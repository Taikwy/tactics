using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent( typeof( Button ) )]
public class ReactiveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public AbilityMenuEntry abilityMenuEntry;
    private TextMeshProUGUI txt;
    private Button btn;

    public Color normalColor;
    public Color highlightedColor;
    public Color pressedColor;
    public Color disabledColor;

    public enum ButtonStatus{
        Normal,
        Disabled,
        Highlighted,
        Pressed
    }

    void Start()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
        btn = gameObject.GetComponent<Button>();
    }

    private ButtonStatus lastButtonStatus = ButtonStatus.Normal;
    private bool isHighlightDesired = false;
    private bool isPressedDesired = false;

    public void Reset () {
        isHighlightDesired = false;
        isPressedDesired = false;
    }

    void Update()
    {
        ButtonStatus desiredButtonStatus = ButtonStatus.Normal;
        if ( !btn.interactable )
            desiredButtonStatus = ButtonStatus.Disabled;
        else{
            if ( isHighlightDesired )
                desiredButtonStatus = ButtonStatus.Highlighted;
            if ( isPressedDesired )
                desiredButtonStatus = ButtonStatus.Pressed;
        }

        // if ( desiredButtonStatus != lastButtonStatus ){
        //     lastButtonStatus = desiredButtonStatus;
        //     switch ( lastButtonStatus ){
        //         case ButtonStatus.Normal:
        //             txt.color = normalColor;
        //             break;
        //         case ButtonStatus.Disabled:
        //             txt.color = disabledColor;
        //             break;
        //         case ButtonStatus.Pressed:
        //             txt.color = pressedColor;
        //             break;
        //         case ButtonStatus.Highlighted:
        //             txt.color = highlightedColor;
        //             break;
        //     }
        // }
    }

    public void OnPointerEnter( PointerEventData eventData ){
        isHighlightDesired = true;
        abilityMenuEntry.Hover();
    }

    public void OnPointerDown( PointerEventData eventData ){
        isPressedDesired = true;
    }

    public void OnPointerUp( PointerEventData eventData ){
        isPressedDesired = false;
    }

    public void OnPointerExit( PointerEventData eventData ){
        isHighlightDesired = false;
        // abilityMenuEntry.Unhover();
    }


}