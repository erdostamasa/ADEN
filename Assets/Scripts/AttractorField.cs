using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AttractorField : MonoBehaviour {
    public GameObject field;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && transform.parent != null){
            if (field.activeSelf){
                field.SetActive(false);
            } else{
                field.SetActive(true);
            }
        }
    }
}