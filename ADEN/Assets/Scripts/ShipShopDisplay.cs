using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipShopDisplay : MonoBehaviour {
    public HangarScreen hangar;
    public MothershipDocking ship;

    public TextMeshProUGUI boosterMetal;
    public TextMeshProUGUI boosterIce;
    public TextMeshProUGUI boosterStored;

    public TextMeshProUGUI haulerMetal;
    public TextMeshProUGUI haulerIce;
    public TextMeshProUGUI haulerStored;

    public TextMeshProUGUI scannerMetal;
    public TextMeshProUGUI scannerIce;
    public TextMeshProUGUI scannerStored;

    public TextMeshProUGUI landerMetal;
    public TextMeshProUGUI landerIce;
    public TextMeshProUGUI landerStored;

    private void Start() {
        boosterMetal.text = hangar.boosterMetalCost.ToString();
        boosterIce.text = hangar.boosterIceCost.ToString();
        boosterStored.text = "Stored: " + ship.storedBooster;

        haulerMetal.text = hangar.haulerMetalCost.ToString();
        haulerIce.text = hangar.haulerIceCost.ToString();
        haulerStored.text = "Stored: " + ship.storedHauler;

        scannerMetal.text = hangar.scannerMetalCost.ToString();
        scannerIce.text = hangar.scannerIceCost.ToString();
        scannerStored.text = "Stored: " + ship.storedScanner;

        landerMetal.text = hangar.landerMetalCost.ToString();
        landerIce.text = hangar.landerIceCost.ToString();
        landerStored.text = "Stored: " + ship.storedLander;
    }

    private void Update() {
        boosterStored.text = "Stored: " + ship.storedBooster;
        haulerStored.text = "Stored: " + ship.storedHauler;
        scannerStored.text = "Stored: " + ship.storedScanner;
        landerStored.text = "Stored: " + ship.storedLander;
    }
}