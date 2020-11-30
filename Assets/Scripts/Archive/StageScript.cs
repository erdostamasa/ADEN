using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StageScript : MonoBehaviour {
    public ThrusterScript main;
    public ThrusterScript left;
    public ThrusterScript right;

    public float mainPower;
    public float sidePower;

    public float fuel = 10f;
    public float consumption = 1f;

    public bool isActive;
    GameObject mainStage;

    void Start() {
        isActive = false;
        mainStage = transform.parent.gameObject;
    }

    private void FixedUpdate() {
        if (isActive){
            if (fuel > 0){
                if (Input.GetKey(KeyCode.W)){
                    main.Fire(mainPower);
                    fuel -= consumption / 2 * Time.fixedDeltaTime;
                }

                if (Input.GetKey(KeyCode.A)){
                    right.Fire(sidePower);
                    fuel -= consumption / 2 * Time.fixedDeltaTime;
                }

                if (Input.GetKey(KeyCode.D)){
                    left.Fire(sidePower);
                    fuel -= consumption * Time.fixedDeltaTime;
                }
            } else{
                isActive = false;
                mainStage.GetComponent<PrimitiveRocketController>().DropStage();
            }
        }

        /*
        if (isActive) {
            foreach(ParticleSystem ps in PSs) {
                ps.Play();
            }
            if (fuel > 0) {
                mainStage.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * thrust, transform.position, ForceMode2D.Force);
                fuel -= consumption;
            } else if (fuel <= 0) {
                foreach (ParticleSystem ps in PSs) {
                    ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                }
                isActive = false;
                mainStage.GetComponent<RocketController>().DropStage();
            }
        }*/
    }
}