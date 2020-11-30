using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HangarScreen : MonoBehaviour {

    public GameObject booster;
    public float boosterMetalCost;
    public float boosterIceCost;

    public GameObject lander;
    public float landerMetalCost;
    public float landerIceCost;

    public GameObject scanner;
    public float scannerMetalCost;
    public float scannerIceCost;

    public GameObject hauler;
    public float haulerMetalCost;
    public float haulerIceCost;

    public GameObject gui;

    public GameObject errorMessage;

    public GameObject mothership;
    public MothershipDocking hangar;
    public Transform spawnPoint;

    private MothershipResources ship;

    private void Start() {
        ship = mothership.GetComponent<MothershipResources>();
    }

    public void SpawnBooster() {
        if (hangar.storedBooster > 0){
            Spawn(booster, 0, 0);
            hangar.storedBooster--;
        } else{
            Spawn(booster, boosterMetalCost, boosterIceCost);
        }
    }

    public void SpawnLander() {
        if (hangar.storedLander > 0){
            Spawn(lander, 0, 0);
            hangar.storedLander--;
        } else{
            Spawn(lander, landerMetalCost, landerIceCost);
        }
    }

    public void SpawnHauler() {
        if (hangar.storedHauler > 0){
            Spawn(hauler, 0, 0);
            hangar.storedHauler--;
        } else{
            Spawn(hauler, haulerMetalCost, haulerIceCost);
        }
    }

    public void SpawnScanner() {
        if (hangar.storedScanner > 0){
            Spawn(scanner, 0, 0);
            hangar.storedScanner--;
        } else{
            Spawn(scanner, scannerMetalCost, scannerIceCost);
        }
    }

    private void Spawn(GameObject prefab, float metalCost, float iceCost) {
        if (ship.iron < metalCost || ship.ice < iceCost){
            errorMessage.SetActive(true);
        } else{
            ship.iron -= metalCost;
            ship.ice -= iceCost;
            GameObject spawned = Instantiate(prefab, spawnPoint.position, mothership.transform.rotation);
            spawned.GetComponent<Rigidbody2D>().velocity = hangar.velocity;
            gui.SetActive(true);
            gameObject.SetActive(false);
            mothership.GetComponentInChildren<MothershipDocking>().Undock();
        }
    }
}