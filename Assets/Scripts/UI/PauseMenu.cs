using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false;
    void Start(){
        pauseMenu.SetActive(false);
    }
    public void PauseGame(){
        pauseMenu.SetActive(true);
        isPaused = true;
    }
    public void ResumeGame(){
        pauseMenu.SetActive(false);
        isPaused = false;
    }
    public void MainMenu(){
        SceneManager.LoadScene("TestMainMenu");
    }
}
