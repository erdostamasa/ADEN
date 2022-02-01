﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    private static GameMaster _instance;

    public static GameMaster Instance {
        get { return _instance; }
    }


    [SerializeField] GameObject explosionParticles;
    [SerializeField] float deathWaitDuration;

    public Slider mainPowerSlider;
    public Slider sidePowerSlider;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }
    }

    public bool dontspawn;

    public GameObject mothership;
    public Transform spawnPoint;
    public GameObject capsulePrefab;
    public GameObject currentCapsule;
    public Camera mainCam;
    public Camera minimapCam;

    public Slider healthBar;

    public GameObject deathGUI;
    public GameObject GUI;

    private void Start() {
        if (!dontspawn) {
            Respawn();
        }

        Application.targetFrameRate = 60;

        EventManager.instance.onPlayerDied += () => { StartCoroutine(DestroyCapsule()); };
        EventManager.instance.onHealthChanged += UpdateHealthBar;
        
        UpdateHealthBar();
    }

    private void Update() {
        // if (currentCapsule != null) {
        //     healthBar.value = currentCapsule.GetComponent<HealthManager>().health;
        //     if (currentCapsule.GetComponent<HealthManager>().health <= 0) {
        //         DestroyCapsule();
        //     }
        // }
    }

    void UpdateHealthBar() {
        healthBar.value = currentCapsule.GetComponent<HealthManager>().health;
    }


    public void NextSkin() {
        currentCapsule.GetComponent<SkinChanger>().NextSkin();
    }


    public void DisconnectCapsule() {
        currentCapsule.GetComponent<CapsuleController>().Disconnect();
    }

    public void Respawn() {
        currentCapsule = Instantiate(capsulePrefab, spawnPoint.position, spawnPoint.rotation);
        mainCam.GetComponent<CameraFollow>().target = currentCapsule.transform;
        minimapCam.GetComponent<MinimapCamera>().target = currentCapsule.transform;
        healthBar.value = healthBar.value = currentCapsule.GetComponent<HealthManager>().health;
        mothership.GetComponentInChildren<MothershipDocking>().player = currentCapsule;
        mothership.GetComponentInChildren<MothershipDocking>().canDock = true;
        mothership.GetComponentInChildren<MothershipDocking>().Dock(false);
        GUI.SetActive(true);
    }

    // public void DamageCapsule(int dmg) {
    //     currentCapsule.GetComponent<HealthManager>().Damage(dmg, 2);
    //     healthBar.value = currentCapsule.GetComponent<HealthManager>().health;
    //     if (currentCapsule.GetComponent<HealthManager>().health <= 0) {
    //         DestroyCapsule();
    //     }
    // }

    public void SelfDestructCapsule() {
        currentCapsule.GetComponent<HealthManager>().Damage(Int32.MaxValue, 1);
    }

    public IEnumerator DestroyCapsule() {
        AudioManager.Instance.PlayExplosion(transform.position, 1f);
        GameObject particles = Instantiate(explosionParticles, currentCapsule.transform.position, explosionParticles.transform.rotation);
        Destroy(particles, 3f);
        Destroy(currentCapsule);

        yield return new WaitForSeconds(deathWaitDuration);

        currentCapsule = null;
        Camera.main.GetComponent<CameraFollow>().target = null;
        deathGUI.SetActive(true);
        GUI.SetActive(false);
    }

    public void RestartLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}