using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideButton : MonoBehaviour {
    public GameObject target;
    private Button btn;

    private void Start() {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ShowHide);
    }

    private void ShowHide() {
        if (target.activeSelf){
            target.SetActive(false);
        } else{
            target.SetActive(true);
        }
    }

}