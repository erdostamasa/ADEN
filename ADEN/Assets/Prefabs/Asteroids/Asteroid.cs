using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour {
    public float minSpeed;
    public float maxSpeed;
    private Camera mainCam;

    void Start() {
        float speed = Random.Range(minSpeed, maxSpeed);
        GetComponent<Rigidbody2D>().AddTorque(speed * GetComponent<Rigidbody2D>().mass);
        mainCam = Camera.main;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.relativeVelocity.magnitude > 3f){
            Vector2 positionOnScreen = mainCam.WorldToViewportPoint(transform.position);
            if (positionOnScreen.x >= -1 && positionOnScreen.x <= 2 && positionOnScreen.y >= -1 &&
                positionOnScreen.y <= 2){
                AudioManager.Instance.PlayAsteroidSound(transform.position, 1f);
            }
        }
    }
}