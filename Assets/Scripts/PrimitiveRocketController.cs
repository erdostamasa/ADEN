using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveRocketController : MonoBehaviour {
    public GameObject capsule;
    public List<GameObject> stages;
    public GameObject activeStage;

    private void Start() {
        if (stages.Count > 0){
            activeStage = stages[stages.Count - 1];
            activeStage.GetComponent<StageScript>().isActive = true;
        } else{
            DeployCapsule();
        }
    }

    public void DropStage() {
        if (stages.Count > 0){
            GameObject lastStage = stages[stages.Count - 1];
            lastStage.tag = "Untagged";
            lastStage.transform.parent = null;
            Rigidbody2D stageRb = lastStage.AddComponent<Rigidbody2D>();
            stageRb.useAutoMass = true;
            stageRb.gravityScale = 0;
            stageRb.drag = 0;
            stageRb.angularDrag = 0;
            lastStage.AddComponent<GravityApplier>();
            stages.Remove(lastStage);
            if (stages.Count > 0){
                activeStage = stages[stages.Count - 1];
                activeStage.GetComponent<StageScript>().isActive = true;
            } else{
                DeployCapsule();
                
            }
        }
    }

    private void DeployCapsule() {
        capsule.GetComponent<PrimitiveCapsule>().enabled = true;
    }
    
}