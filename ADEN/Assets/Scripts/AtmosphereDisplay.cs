using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtmosphereDisplay : MonoBehaviour {
    public TextMeshProUGUI counter;
    public GameMaster gameMaster;

    [SerializeField] List<GameObject> visuals;

    void Update() {
        List<PlanetScript> planets = gameMaster.currentCapsule.GetComponent<GravityApplier>().planets;
        PlanetScript planet = null;
        if (planets.Count > 0){
            planet = planets[0];
        }

        if (planet != null){
            counter.text = Math.Round(gameMaster.currentCapsule.GetComponent<GravityApplier>().drag*5, 2, MidpointRounding.AwayFromZero).ToString();
            foreach (GameObject visual in visuals) {
                visual.SetActive(true);
            }
        } else{
            //counter.text = "NONE";
            foreach (GameObject visual in visuals) {
                visual.SetActive(false);
            }
        }
    }
}