using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour {
    public MothershipDocking ship;
    public GameObject hangarScreen;
    public GameObject errorScreen;

    public void ButtonPress() {
        if (ship.docked){
            if (!hangarScreen.activeSelf){
                hangarScreen.SetActive(true);
                errorScreen.SetActive(false);
            }
        }
    }
}