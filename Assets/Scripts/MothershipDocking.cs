using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MothershipDocking : MonoBehaviour {
    private RelativeJoint2D joint;
    public bool canDock = false;
    public bool docked = false;
    public GameObject player;

    public GameObject dockingScreen;
    public GameObject gui;

    private Vector2 oldpos;
    private Vector2 newpos;
    public Vector2 velocity;

    public int storedBooster = 0;
    public int storedHauler = 0;
    public int storedScanner = 0;
    public int storedLander = 0;


    public bool dockAtStart;

    public GameObject hangarLight;
    
    private void Start() {
        joint = GetComponent<RelativeJoint2D>();
        oldpos = transform.position;
        if (dockAtStart){
            Dock(false);
        }
    }


    private void FixedUpdate() {
        //Calculate velocity of kinematic Rigidbody2D
        newpos = transform.position;
        velocity = (newpos - oldpos) / Time.fixedDeltaTime;
        oldpos = newpos;
    }

    private void Update() {
        //Debug.Log(canDock);
        //Debug.Log("K: " + Input.GetKeyDown(KeyCode.K));
        if (canDock && Input.GetKeyDown(KeyCode.K)){
            //if (!player.GetComponent<CapsuleController>().isConnected){
                Dock(false);    
            //}
        } else if (Input.GetKeyDown(KeyCode.K) && docked){
            Undock();
        }
    }

    public void Dock(bool showShop) {
        if (player.GetComponent<CapsuleController>().isConnected){
            GameObject module = player.GetComponent<CapsuleController>().connected;
            CustomTag ct = module.GetComponent<CustomTag>();
            if (ct.HasTag("booster")){
                storedBooster++;
            }else if (ct.HasTag("hauler")){
                storedHauler++;
            }else if (ct.HasTag("scanner")){
                storedScanner++;
            }else if (ct.HasTag("lander")){
                storedLander++;
            }

            GameMaster.Instance.currentCapsule.GetComponent<CapsuleController>().DestroyConnected();
        }
        if (!canDock) return;
        AudioManager.Instance.PlayDockingSound();
        joint.connectedBody = player.GetComponent<Rigidbody2D>();
        float angOffset = joint.angularOffset;
        Vector2 linOffset = joint.linearOffset;
        joint.autoConfigureOffset = true;
        joint.enabled = true;
        joint.autoConfigureOffset = false;
        joint.angularOffset = angOffset;
        joint.linearOffset = linOffset;
        docked = true;
        canDock = false;
        player.GetComponent<CapsuleController>().enabled = false;
        player.GetComponent<SelectorScript>().enabled = false;
        player.GetComponent<GravityApplier>().enabled = false;
        player.GetComponent<BuildingPlacer>().enabled = false;
        player.GetComponent<HealthManager>().health = 100;
        player.transform.Find("HUD").gameObject.SetActive(false);
        if(showShop) dockingScreen.SetActive(true);
        hangarLight.SetActive(true);
        //gui.SetActive(false);
    }

    public void Undock() {
        if (!docked) return;
        joint.enabled = false;
        joint.connectedBody = null;
        docked = false;
        canDock = true;
        player.GetComponent<CapsuleController>().enabled = true;
        player.GetComponent<SelectorScript>().enabled = true;
        player.GetComponent<GravityApplier>().enabled = true;
        player.GetComponent<BuildingPlacer>().enabled = true;
        player.transform.Find("HUD").gameObject.SetActive(true);
        player.GetComponent<CapsuleController>().UpdateConnectPoints();
        dockingScreen.SetActive(false);
        gui.SetActive(true);
        AudioManager.Instance.Undock();
        hangarLight.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")){
           // if (!other.GetComponent<CapsuleController>().isConnected){
                canDock = true;
                player = other.gameObject;
            //} else{
            //    canDock = false;
            //}
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){
           // if (!other.GetComponent<CapsuleController>().isConnected){
                canDock = true;
                player = other.gameObject;
           // }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")){
            canDock = false;
        }
    }
}