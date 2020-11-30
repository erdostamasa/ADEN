using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsRotater : MonoBehaviour {
    private RectTransform rect;

    public float rotationSpeed;
    
    void Start() {
        rect = GetComponent<RectTransform>();
    }

    void Update() {
        rect.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}