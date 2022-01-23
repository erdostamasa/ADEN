using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    public List<GameObject> pages;
    public GameObject currentPage;
    public int currentIndex;

    public GameObject prevButton;
    public GameObject nextButton;


    private void Start() {
        currentIndex = 0;
        currentPage = pages[currentIndex];
        currentPage.SetActive(true);
    }

    public void NextPage() {
        if (currentIndex < pages.Count - 1){
            currentPage.SetActive(false);
            currentPage = pages[currentIndex + 1];
            currentIndex++;
            currentPage.SetActive(true);
            if (currentIndex == pages.Count - 1){
                nextButton.SetActive(false);
            } else{
                nextButton.SetActive(true);
            }
            
            
            if (currentIndex == 0){
                prevButton.SetActive(false);
            } else{
                prevButton.SetActive(true);
            }
        }
    }

    public void PreviousPage() {
        if (currentIndex > 0){
            currentPage.SetActive(false);
            currentPage = pages[currentIndex - 1];
            currentIndex--;
            currentPage.SetActive(true);
            
            if (currentIndex == pages.Count - 1){
                nextButton.SetActive(false);
            } else{
                nextButton.SetActive(true);
            }


            
            if (currentIndex == 0){
                prevButton.SetActive(false);
            } else{
                prevButton.SetActive(true);
            }
        }
    }
}