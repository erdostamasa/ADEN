using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterSelect : MonoBehaviour
{
    public void LoadChapter(int i) {
        SceneManager.LoadScene(i);
    }

}
