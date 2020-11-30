using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ThrusterScript : MonoBehaviour {
    public GameObject rootObject;
    public Rigidbody2D rootRigidbody;

    public ParticleSystem.EmissionModule em;
    public AudioSource audioSource;
    public bool isFiring;


    private float power;
    private void Awake() {
        rootObject = transform.root.gameObject;
        rootRigidbody = rootObject.GetComponent<Rigidbody2D>();

        em = GetComponent<ParticleSystem>().emission;
        audioSource = GetComponent<AudioSource>();
        isFiring = false;
    }

    private void Update() { }


    private void FixedUpdate() {
        rootRigidbody = transform.root.gameObject.GetComponent<Rigidbody2D>();

        if (isFiring){
            isFiring = false;
            //Set particles on
            em.rateOverTime = 40;
            //Play sound
            audioSource.volume = 0.1f;
            //Calculate and apply force in the direction of red axis
            Vector2 force = transform.right * (-1 * power);
            rootRigidbody.AddForceAtPosition(force, transform.position);
        } else{
            em.rateOverTime = 0;
            audioSource.volume -= 2.5f * Time.fixedDeltaTime;
        }
    }

    public void Fire(float firingPower) {
        power = firingPower;
        isFiring = true;
    }
}