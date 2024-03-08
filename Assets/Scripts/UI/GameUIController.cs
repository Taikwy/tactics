using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameUIController : MonoBehaviour 
{
    [SerializeField] PausePanel pauseMenu; 
    [SerializeField] GameOverPanel GameOverMenu;              //selected and targeted unit
    [SerializeField] GameObject disabledOverlay;              //selected and targeted unit
    [HideInInspector] public bool showingPause, showingGameOver, showingDisabled = false;
    [HideInInspector] public bool isPaused{
        get{
            return pauseMenu.isPaused;
        }
    }
    
    void Start (){
        HidePause();
        HideGameOver();
        HideDisabled();
    }
    public void ShowPause (){
        ShowDisabled();
        //will not pause if currently game over
        if(showingGameOver)
            return;
        showingPause = true;
        pauseMenu.ShowPanel();
    }
    public void HidePause (){
        showingPause = false;
        pauseMenu.HidePanel();
        HideDisabled();
    }

    public void ShowGameOver (){
        HidePause();
        ShowDisabled();
        showingGameOver = true;
        GameOverMenu.ShowPanel();
    }
    public void HideGameOver (){
        showingGameOver = false;
        GameOverMenu.HidePanel();
        HideDisabled();
    }

    public void ShowDisabled (){
        showingDisabled = true;
        disabledOverlay.SetActive(true);
    }
    public void HideDisabled (){
        showingDisabled = false;
        GameOverMenu.HidePanel();
        disabledOverlay.SetActive(false);
    }
}