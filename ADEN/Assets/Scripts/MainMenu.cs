using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void StartGame() {
        SceneManager.LoadScene("0Prologue");
    }

    public void StartTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    public void ExitGame() {
        Application.Quit();
    }
    
}