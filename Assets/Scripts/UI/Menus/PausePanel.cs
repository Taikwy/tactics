using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused = false;

    public void ShowPanel(){
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenu.SetActive(true);
    }
    public void HidePanel(){
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void MainMenu(){
        SceneManager.LoadScene("TestMainMenu");
    }
}
