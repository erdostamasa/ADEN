using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtmosphereDisplay : MonoBehaviour {
    public TextMeshProUGUI counter;
    public GameMaster gameMaster;

    void Update() {
        List<PlanetScript> planets = gameMaster.currentCapsule.GetComponent<GravityApplier>().planets;
        PlanetScript planet = null;
        if (planets.Count > 0){
            planet = planets[0];
        }

        if (planet != null){
            counter.text = Math.Round(planet.currentDrag, 4, MidpointRounding.AwayFromZero).ToString();
        } else{
            counter.text = "NONE";
        }
    }
}