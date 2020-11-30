using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buildable : MonoBehaviour {
    public float metalResources = 0f;
    public TextMeshProUGUI display;
    public TextMeshProUGUI counterDisplay;
    public int buildingCount = 0;
    public int neededBuildingCount = 1;
    public bool completed = false;


    public GameObject targetUIprefab;
    private bool spawnedTarget = false;
    private Transform targetTransform;
    
    
    private void Update() {
        
        
        if (!completed){
            if (QuestManager.instance.currentQuest.goal.goalType != GoalType.BuildScanningPost){
                display.gameObject.SetActive(false);
                counterDisplay.gameObject.SetActive(false);
            } else{
                
                display.text = "metal: " + Math.Round(metalResources, 2, MidpointRounding.ToEven);
                counterDisplay.text = "buildings: " + buildingCount + "/" + neededBuildingCount;
                
                display.gameObject.SetActive(true);
                counterDisplay.gameObject.SetActive(true);

                if (!spawnedTarget){
                    targetTransform = Instantiate(targetUIprefab, transform.position, Quaternion.identity).transform;
                    spawnedTarget = true;
                } else{
                    targetTransform.position = transform.position + new Vector3(0, 0, -5);
                }
                
                if (buildingCount >= neededBuildingCount){
                    QuestManager.instance.currentQuest.goal.currentAmount++;
                    completed = true;
                    Destroy(targetTransform.gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        CustomTag ct = other.gameObject.GetComponent<CustomTag>();
        if (ct == null) return;

        if (ct.HasTag("iron")){
            metalResources += other.gameObject.GetComponent<Rigidbody2D>().mass;
            Destroy(other.gameObject);
        }
    }

    /*
    private void OnCollisionExit2D(Collision2D other) {
        throw new NotImplementedException();
    }

    private void OnCoolEnter2D(Collider2D other) {
        CustomTag ct = other.GetComponent<CustomTag>();
        if (ct == null) return;

        if (ct.HasTag("iron")){
            metalResources += other.GetComponent<Rigidbody2D>().mass;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        CustomTag ct = other.GetComponent<CustomTag>();
        if (ct == null) return;

        if (ct.HasTag("iron")){
            metalResources -= other.GetComponent<Rigidbody2D>().mass;
            if (metalResources < 0.1f){
                metalResources = 0f;
            }
        }
    }*/
}