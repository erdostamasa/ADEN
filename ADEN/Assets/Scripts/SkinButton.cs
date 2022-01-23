using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour {
    

    // Update is called once per frame
    void Update() {
        GetComponent<Image>().sprite = GameMaster.Instance.currentCapsule.GetComponent<SkinChanger>().currentSkin;
    }
}
