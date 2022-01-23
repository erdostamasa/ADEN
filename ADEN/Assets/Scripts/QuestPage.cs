using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPage : MonoBehaviour {
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Quest currentQuest;
    public GameObject targetPrefab;

    private void Update() {
        if (currentQuest == null){
            title.text = "";
            description.text = "";
        } else{
            DisplayQuest(currentQuest);
            
            /*if (currentQuest.goal.goalType == GoalType.ScanPlanet){
                DisplayScanTarget();
            }else if (currentQuest.goal.goalType == GoalType.BuildScanningPost){
                DisplayBuildTargets();
            }*/
        }
    }

    private List<GameObject> buildTargets;
    private void DisplayBuildTargets() {
        if (buildTargets == null){
            buildTargets = new List<GameObject>();
            foreach (GameObject planet in currentQuest.goal.targetPlanets){
                buildTargets.Add(Instantiate(targetPrefab));
            }
        }
        
        GameObject remove = null;
        GameObject remove2 = null;
        for (int i = 0; i < buildTargets.Count; i++){
            Vector3 planetPosition = currentQuest.goal.targetPlanets[i].transform.position;

            buildTargets[i].transform.position = new Vector3(planetPosition.x, planetPosition.y, -10);

            if (currentQuest.goal.targetPlanets[i].GetComponent<Buildable>().completed){
                remove = currentQuest.goal.targetPlanets[i];
                remove2 = buildTargets[i];
            }
        }

        if (remove != null){
            currentQuest.goal.targetPlanets.Remove(remove);
            buildTargets.Remove(remove2);
            Destroy(remove2);
        }
        
    }
    
    private List<GameObject> minimapTargets;

    private void DisplayScanTarget() {
        if (minimapTargets == null){
            minimapTargets = new List<GameObject>();
            foreach (GameObject planet in currentQuest.goal.targetPlanets){
                minimapTargets.Add(Instantiate(targetPrefab));
            }
        }

        GameObject remove = null;
        GameObject remove2 = null;
        for (int i = 0; i < minimapTargets.Count; i++){
            Vector3 planetPosition = currentQuest.goal.targetPlanets[i].transform.position;

            minimapTargets[i].transform.position = new Vector3(planetPosition.x, planetPosition.y, -10);

            if (currentQuest.goal.targetPlanets[i].GetComponent<ScannablePlanet>().scanComplete){
                remove = currentQuest.goal.targetPlanets[i];
                remove2 = minimapTargets[i];
            }
        }

        if (remove != null){
            currentQuest.goal.targetPlanets.Remove(remove);
            minimapTargets.Remove(remove2);
            Destroy(remove2);
        }
    }

    private void DisplayQuest(Quest quest) {
        title.text = quest.goal.currentAmount + " / " + quest.goal.requiredAmount + " " + quest.title;
        description.text = quest.description;
    }
}