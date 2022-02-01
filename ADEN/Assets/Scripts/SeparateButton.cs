using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeparateButton : MonoBehaviour {
    private Button btn;
    
    private void Start() {
        btn = GetComponent<Button>();
    }

    private void Update() {
        if (GameMaster.Instance.currentCapsule == null) return;
        if (GameMaster.Instance.currentCapsule.GetComponent<CapsuleController>().connected){
            btn.interactable = true;
        } else{
            btn.interactable = false;
        }
    }
}
