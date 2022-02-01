using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public static EventManager instance;

    void Awake() {
        if (instance != null) {
            Destroy(this);
        }

        instance = this;
    }

    public event Action onPlayerDied;
    public void PlayerDied() {
        onPlayerDied?.Invoke();
    }
}