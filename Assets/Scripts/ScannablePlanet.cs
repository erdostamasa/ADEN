using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScannablePlanet : MonoBehaviour {
    public float scanProgress;
    public bool playerScanning = false;
    public QuestManager questManager;
    public float scanSpeed = 1f;
    public GameObject progressUI;
    public TextMeshProUGUI progressText;
    public bool scanComplete = false;
    public GameObject targetUIprefab;

    private bool spawnedTarget = false;
    private Transform targetTransform;
    

    private void Start() {
        scanProgress = 0f;
    }

    private void Update() {
        if (questManager.currentQuest.goal.goalType == GoalType.ScanPlanet && !scanComplete){
            //Display target is quest is active
            if (!spawnedTarget){
                targetTransform = Instantiate(targetUIprefab, transform.position, Quaternion.identity)
                    .GetComponent<Transform>();
                spawnedTarget = true;
            } else{
                targetTransform.position = transform.position + new Vector3(0, 0, -5);
            }
            
            if (playerScanning){
                scanProgress += scanSpeed * Time.deltaTime;
                progressText.text = "Scan progress: " + Math.Round(scanProgress, 2, MidpointRounding.AwayFromZero) +
                                    "%";
                if (scanProgress >= 100f){
                    questManager.currentQuest.goal.currentAmount += 1;
                    progressUI.SetActive(false);
                    scanComplete = true;
                    Destroy(targetTransform.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (questManager.currentQuest.goal.goalType == GoalType.ScanPlanet){
            if (other.gameObject.CompareTag("Player")){
                GameObject connected = other.GetComponent<CapsuleController>().connected;
                if (connected != null && connected.GetComponent<CustomTag>() != null &&
                    connected.GetComponent<CustomTag>().HasTag("scanner")){
                    playerScanning = true;
                    progressUI.SetActive(true);
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other) {
        if (questManager.currentQuest.goal.goalType == GoalType.ScanPlanet){
            if (other.gameObject.CompareTag("Player")){
                GameObject connected = other.GetComponent<CapsuleController>().connected;
                if (connected != null && connected.GetComponent<CustomTag>() != null &&
                    connected.GetComponent<CustomTag>().HasTag("scanner")){
                    playerScanning = false;
                    progressUI.SetActive(false);
                    scanProgress = 0f;
                }
            }
        }
    }
}