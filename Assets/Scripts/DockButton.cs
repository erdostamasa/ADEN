using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DockButton : MonoBehaviour {
    public MothershipDocking ship;

    private Button btn;

    private void Start() {
        btn = GetComponent<Button>();
    }

    void Update() {
        btn.interactable = ship.canDock;
    }
}
