using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public GameObject endGameMenu;
    public static bool isPaused = false;

    public void ShowPanel(){
        isPaused = true;
        endGameMenu.SetActive(true);
    }
    public void HidePanel(){
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
