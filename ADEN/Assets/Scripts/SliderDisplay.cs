using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//using UnityEngine.UIElements;

public class SliderDisplay : MonoBehaviour {
    public GameObject sliderObject;

    private Slider slider;
    private void Start() {
        slider = sliderObject.GetComponent<Slider>();
    }

    private void Update() {
        GetComponent<TextMeshProUGUI>().text = Math.Round(slider.value, 2, MidpointRounding.AwayFromZero).ToString();
    }
}
