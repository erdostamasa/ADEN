using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LockRotation : MonoBehaviour {
    private void Update() {
        transform.rotation = quaternion.identity;
    }
}