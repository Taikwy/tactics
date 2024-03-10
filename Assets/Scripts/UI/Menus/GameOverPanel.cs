using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public GameObject endGameMenu;
    public static bool isPaused = false;

    public void ShowPanel(){
        Time.timeScale = 0f;
        isPaused = true;
        endGameMenu.SetActive(true);
    }
    public void HidePanel(){
        Time.timeScale = 1f;
        isPaused = false;
        endGameMenu.SetActive(false);
    }
    
    public void PlayAgain(){
        SceneManager.LoadScene("BattleTest");
    }
    public void MainMenu(){
        SceneManager.LoadScene("TestMainMenu");
    }

    

}
