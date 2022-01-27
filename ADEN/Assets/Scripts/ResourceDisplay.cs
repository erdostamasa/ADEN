using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour {
    public TextMeshProUGUI metal;
    public TextMeshProUGUI water;

    public MothershipResources ship;

    private void Update() {
        metal.text = Math.Round(ship.iron, 1, MidpointRounding.ToEven).ToString();
        water.text = Math.Round(ship.ice, 1, MidpointRounding.ToEven).ToString();
    }
}
