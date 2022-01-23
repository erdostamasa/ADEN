using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour {
    public List<Sprite> skins;
    public int currentIndex = 0;
    public Sprite currentSkin;

    /*
    public Sprite cap1;
    public Sprite cap2;
    public Sprite cap3;
    public Sprite cap4;
*/
    public SpriteRenderer minimap;
    public SpriteRenderer worldmap;

    private SpriteRenderer capsule;

    private void Start() {
        capsule = GetComponent<SpriteRenderer>();
        currentSkin = skins[currentIndex];
        capsule.sprite = skins[currentIndex];
        minimap.sprite = skins[currentIndex];
        worldmap.sprite = skins[currentIndex];
    }

    public void NextSkin() {
        if (currentIndex < skins.Count - 1){
            currentIndex++;
        } else{
            currentIndex = 0;
        }

        currentSkin = skins[currentIndex];
        capsule.sprite = skins[currentIndex];
        minimap.sprite = skins[currentIndex];
        worldmap.sprite = skins[currentIndex];
    }

/*
    public void ChangeSking(SKIN skin) {
        if (skin == SKIN.CAP1){
            capsule.sprite = cap1;
            minimap.sprite = cap1;
            worldmap.sprite = cap1;
        } else if (skin == SKIN.CAP2){
            capsule.sprite = cap2;
            minimap.sprite = cap2;
            worldmap.sprite = cap2;
        } else if (skin == SKIN.CAP3){
            capsule.sprite = cap3;
            minimap.sprite = cap3;
            worldmap.sprite = cap3;
        } else if (skin == SKIN.CAP4){
            capsule.sprite = cap4;
            minimap.sprite = cap4;
            worldmap.sprite = cap4;
        }
    }*/
}

/*
public enum SKIN {
    CAP1,
    CAP2,
    CAP3,
    CAP4,
}*/