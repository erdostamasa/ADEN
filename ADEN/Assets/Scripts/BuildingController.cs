using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {
    public int collidingCount;
    
    void Start() {
        collidingCount = 0;
    }

    
    private void OnCollisionEnter2D(Collision2D other) {
        collidingCount++;
    }

    private void OnCollisionExit2D(Collision2D other) {
        collidingCount--;
    }
    
    /*
    private void OnTriggerEnter2D(Collider2D other) {
        collidingCount++;
    }

    private void OnTriggerExit2D(Collider2D other) {
        collidingCount--;
    }*/
}
