using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void StartGame() {
        SceneManager.LoadScene("Story");
    }

    public void StartTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    public void ExitGame() {
        Application.Quit();
    }
    
}