using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour 
{
    [SerializeField] PausePanel pauseMenu; 
    [SerializeField] GameOverPanel GameOverMenu;              //selected and targeted unit
    [HideInInspector] public bool showingPause, showingGameOver = false;
    
    void Start (){
        HidePause();
        HideGameOver();
    }
    public void ShowPause (){
        //will not pause if currently game over
        if(showingGameOver)
            return;
        showingPause = true;
        pauseMenu.ShowPanel();
    }
    public void HidePause (){
        showingPause = false;
        pauseMenu.HidePanel();
    }

    public void ShowGameOver (){
        HidePause();
        showingGameOver = true;
        GameOverMenu.ShowPanel();
    }
    public void HideGameOver (){
        showingGameOver = false;
        GameOverMenu.HidePanel();
    }
}