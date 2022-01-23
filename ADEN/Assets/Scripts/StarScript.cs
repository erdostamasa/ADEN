using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour {

    private float value;
    public float speed;
    private bool increasing;

    private GameObject rotationPivot;

    float originalRotation;
    void Start() {
        value = 0;
        increasing = true;

        /*
        rotationPivot = GameObject.FindGameObjectWithTag("MainCamera");
        originalRotation = GetComponent<Renderer>().material.GetFloat("_Rotation");*/
    }

    void Update() {

        if (increasing) {
            value += speed * Time.deltaTime;
        } else {
            value -= speed * Time.deltaTime;
        }

        if(value >= 0.2) {
            increasing = false;
        }else if(value <= 0) {
            increasing = true;
        }
        
        GetComponent<Renderer>().material.SetFloat("_BrightnessVariationScale", value);
        /*
        GetComponent<Renderer>().material.SetFloat("_Rotation", originalRotation + rotationPivot.transform.rotation.eulerAngles.z);*/

    }


}
