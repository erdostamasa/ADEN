using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
            if (gameIsPaused){
                Resume();
            } else{
                Pause();
            }
        }
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}