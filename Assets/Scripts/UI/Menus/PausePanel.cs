using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused = false;

    public void ShowPanel(){
        GetComponentInParent<BattleController>().audioManager.PlayMusic(GetComponentInParent<BattleController>().pauseMusic);
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenu.SetActive(true);
    }
    public void HidePanel(){
        GetComponentInParent<BattleController>().audioManager.PlayMusic(GetComponentInParent<BattleController>().battleMusic);
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void MainMenu(){
        GetComponentInParent<BattleController>().audioManager.StopMusic();
        SceneManager.LoadScene("MainMenu");
    }
}
