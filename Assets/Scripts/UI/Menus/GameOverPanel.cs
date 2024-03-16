using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public GameObject endGameMenu;
    public static bool isPaused = false;

    public void ShowPanel(){
        GetComponentInParent<BattleController>().audioManager.PlayMusic(GetComponentInParent<BattleController>().gameOverMusic);
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
        GetComponentInParent<BattleController>().audioManager.StopMusic();
        SceneManager.LoadScene("BattleTest");
    }
    public void MainMenu(){
        GetComponentInParent<BattleController>().audioManager.StopMusic();
        SceneManager.LoadScene("MainMenu");
    }

    

}
