using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false;

    public void ShowPanel(){
        isPaused = true;
        pauseMenu.SetActive(true);
    }
    public void HidePanel(){
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void MainMenu(){
        SceneManager.LoadScene("TestMainMenu");
    }
}
