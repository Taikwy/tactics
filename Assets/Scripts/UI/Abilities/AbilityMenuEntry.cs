using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    GameUIController guiController;
    [SerializeField] Image bullet;
    // [SerializeField] SpriteRenderer bullet;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite disabledSprite;
    [SerializeField] TMP_Text label;

    private TextMeshProUGUI txt;
    public Button button;

    public Color normalColor;
    public Color highlightedColor;
    public Color pressedColor;
    public Color disabledColor;

    [HideInInspector] public GameObject entry;

    [HideInInspector]public UnityEngine.Events.UnityAction highlightFunc, unhighlightFunc;
    

    public enum ButtonStatus{
        Normal,
        Disabled,
        Highlighted,
        Pressed
    }

    private ButtonStatus lastButtonStatus = ButtonStatus.Normal;
    private bool isHighlightDesired = false;
    private bool isPressedDesired = false;

    void Awake (){
        // guiController = GetComponent<BattleController>().guiController;
        guiController = (FindObjectOfType(typeof(BattleController)) as BattleController).guiController;
        txt = GetComponentInChildren<TextMeshProUGUI>();
        button = gameObject.GetComponent<Button>();
    }

    void Update()
    {
        ButtonStatus desiredButtonStatus = ButtonStatus.Normal;
        
        // button.interactable = !guiController.isPaused;
        // if(guiController.isPaused){
        //     print("gui controller is paused");
        //     // button.interactable = false;
        // }
        if ( !button.interactable){
            desiredButtonStatus = ButtonStatus.Disabled;
        }
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

    public string Title {
        get { return label.text; }
        set { label.text = value; }
    }

    [System.Flags]
    enum States {
        None = 0,
        Selected = 1 << 0,
        Locked = 1 << 1
    }

    public bool IsLocked{
        get { return (State & States.Locked) != States.None; }
        set{
            if (value)
                State |= States.Locked;
            else
                State &= ~States.Locked;
        }
    }

    public bool IsSelected{
        get { return (State & States.Selected) != States.None; }
        set{
            if (value)
                State |= States.Selected;
            else
                State &= ~States.Selected;
        }
    }

    //getter setter for entry state. automatically updates the text color when a new state has been set
    States State{ 
        get { return state; }
        set{
            if (state == value)
                return;
            state = value;
            
            // if (IsLocked) {
            //     // bullet.sprite = disabledSprite;
            //     label.color = Color.red;
            //     // outline.effectColor = new Color32(20, 36, 44, 255);
            // }
            // else if (IsSelected) {
            //     // bullet.sprite = selectedSprite;
            //     label.color = new Color32(249, 210, 118, 255);
            //     // outline.effectColor = new Color32(255, 160, 72, 255);
            // }
            // else {
            //     // bullet.sprite = normalSprite;
            //     label.color = Color.black;
            //     // outline.effectColor = new Color32(20, 36, 44, 255);
            // }
        }
    }
    States state;

    public void Reset () {
        State = States.None;
        isHighlightDesired = false;
        isPressedDesired = false;
        button.interactable = true;

        highlightFunc = null;
        unhighlightFunc = null;
    }


    public void OnPointerEnter( PointerEventData eventData ){
         if(guiController.isPaused)
            return;
        // Debug.Log("highlighting");
        isHighlightDesired = true;
            // Debug.Log("null func " + highlightFunc);
        if(highlightFunc != null){
            // Debug.Log("func " + highlightFunc);
            highlightFunc();
        }

    }
    public void OnPointerExit( PointerEventData eventData ){
         if(guiController.isPaused)
            return;
        // Debug.Log("unhighlighting");
        isHighlightDesired = false;
        if(unhighlightFunc != null)
            unhighlightFunc();
        // abilityMenuEntry.Unhover();
    }

    public void OnPointerDown( PointerEventData eventData ){
        isPressedDesired = true;
    }

    public void OnPointerUp( PointerEventData eventData ){
        isPressedDesired = false;
    }

}
