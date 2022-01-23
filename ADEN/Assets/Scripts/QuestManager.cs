using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class QuestManager : MonoBehaviour {
    private static QuestManager _instance;

    public static QuestManager instance {
        get { return _instance; }
    }

    private void Awake() {
        if (_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else{
            _instance = this;
        }
    }

    //[HideInInspector]
    public List<Quest> quests;

    public MothershipResources shipResources;
    public QuestPage questPage;
    private int currentIndex;
    public Quest currentQuest;
    public bool nextQuest = false;


    private void Start() {
        currentQuest = quests[0];
        currentIndex = 0;
        currentQuest.isActive = true;
        questPage.currentQuest = currentQuest;
    }

    private void Update() {
        if (currentQuest.goal.IsQuestComplete()){
            currentQuest.isActive = false;
            if (currentIndex + 1 <= quests.Count - 1){
                currentQuest = quests[currentIndex + 1];
                currentQuest.isActive = true;
                currentIndex++;
                questPage.currentQuest = currentQuest;
            } else{
                questPage.currentQuest = null;
            }
        }


        //DEBUG
        if (nextQuest){
            nextQuest = false;
            currentQuest.isActive = false;
            if (currentIndex + 1 <= quests.Count - 1){
                currentQuest = quests[currentIndex + 1];
                currentQuest.isActive = true;
                currentIndex++;
                questPage.currentQuest = currentQuest;
            } else{
                questPage.currentQuest = null;
            }
        }
    }
}