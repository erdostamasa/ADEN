using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKeyListener : MonoBehaviour {

    public GameObject map;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.M)){
            if (map.activeSelf){
                map.SetActive(false);
            } else{
                map.SetActive(true);
            }
        }
        
        
        
        
    }
}