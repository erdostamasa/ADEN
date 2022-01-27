using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour {
    public GameObject mothership;
    private MothershipResources ship;

    private void Start() {
        ship = mothership.GetComponent<MothershipResources>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ProcessAsteroid(other);
    }

    private void OnTriggerStay2D(Collider2D other) {
        ProcessAsteroid(other);
    }

    private void ProcessAsteroid(Collider2D other) {
        CustomTag ct = other.GetComponent<CustomTag>();
        if (ct != null && !ct.HasTag("grabbed")){
            float amount;
            if (ct.HasTag("ice")){
                AudioManager.Instance.PlayAsteroidCollected(other.transform.position, 1f);
                amount = other.GetComponent<Rigidbody2D>().mass;
                Destroy(other.gameObject);
                ship.ice += amount;
                if (QuestManager.instance.currentQuest.goal.goalType == GoalType.CollectIce){
                    //QuestManager.instance.currentQuest.goal.currentAmount = (int)ship.ice;
                    QuestManager.instance.currentQuest.goal.currentAmount += amount;
                }
                
            }else if (ct.HasTag("iron")){
                AudioManager.Instance.PlayAsteroidCollected(other.transform.position, 1f);
                amount = other.GetComponent<Rigidbody2D>().mass;
                Destroy(other.gameObject);
                ship.iron += amount;
                if (QuestManager.instance.currentQuest.goal.goalType == GoalType.CollectIron){
//                    QuestManager.instance.currentQuest.goal.currentAmount = (int) ship.iron;
                    QuestManager.instance.currentQuest.goal.currentAmount += amount;
                }
            }
        }
    }
    
}