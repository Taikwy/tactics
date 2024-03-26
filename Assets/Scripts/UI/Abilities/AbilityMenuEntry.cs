using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public AudioManager am;
    GameUIController guiController;
    // TMP_Text label;
    TextMeshProUGUI txt;
    public Image icon;
    public Color disabledColor;
    [HideInInspector] public Button button;
    [HideInInspector] public GameObject abilityObject;
    [HideInInspector] public string commandLabel;

    [HideInInspector]public UnityEngine.Events.UnityAction highlightFunc, unhighlightFunc;

    // [SerializeField] SpriteRenderer bullet;
    // [SerializeField] Sprite normalSprite;
    // [SerializeField] Sprite selectedSprite;
    // [SerializeField] Sprite disabledSprite;

    // public Color normalColor;
    // public Color highlightedColor;
    // public Color pressedColor;
    // public Color disabledColor;

    public enum ButtonStatus{
        Normal,
        Disabled,
        Highlighted,
        Pressed
    }

    private ButtonStatus lastButtonStatus = ButtonStatus.Normal;
    private bool isHighlightDesired = false;
    private bool isPressedDesired = false;

    public string hoverEnabledSound, hoverDisabledSound, clickEnabledSound, clickDisabledSound;

    void Awake (){
        // guiController = GetComponent<BattleController>().guiController;
        txt = GetComponentInChildren<TextMeshProUGUI>();
        button = gameObject.GetComponent<Button>();
        // label = gameObject.GetComponentInChildren<TMP_Text>();
    }
    public void Setup(){
        am = (FindObjectOfType(typeof(BattleController)) as BattleController).audioManager;
        guiController = (FindObjectOfType(typeof(BattleController)) as BattleController).guiController;

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
            // desiredButtonStatus = ButtonStatus.Disabled;
            icon.color = disabledColor;
        }
        else{
            icon.color = Color.white;
            // if ( isHighlightDesired )
            //     desiredButtonStatus = ButtonStatus.Highlighted;
            // if ( isPressedDesired )
            //     desiredButtonStatus = ButtonStatus.Pressed;
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

    // public string Title {
    //     get { return gameObject.name; }
    //     set { gameObject.name = value; }
    //     // get { return label.text; }
    //     // set { label.text = value; }
    // }

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
        button.onClick.RemoveAllListeners();
        icon.sprite = null;

        highlightFunc = null;
        unhighlightFunc = null;
    }


    public void OnPointerEnter( PointerEventData eventData ){
         if(guiController.isPaused)
            return;
        isHighlightDesired = true;
        if(highlightFunc != null){
            highlightFunc();
        }
        if(button.interactable)
            am.PlaySFX(hoverEnabledSound);
        else
            am.PlaySFX(hoverDisabledSound);

    }
    public void OnPointerExit( PointerEventData eventData ){
         if(guiController.isPaused)
            return;
        isHighlightDesired = false;
        if(unhighlightFunc != null)
            unhighlightFunc();
    }

    public void OnPointerDown( PointerEventData eventData ){
        isPressedDesired = true;
        if(button.interactable)
            am.PlaySFX(clickEnabledSound);
        else
            am.PlaySFX(clickDisabledSound);
    }

    public void OnPointerUp( PointerEventData eventData ){
        isPressedDesired = false;
    }

}
