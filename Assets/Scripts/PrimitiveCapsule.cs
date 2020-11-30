using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveCapsule : MonoBehaviour {
    public ThrusterScript thruster;
    public ThrusterScript right;
    public ThrusterScript left;
    public float power = 6f;
    public float sidePower = 3f;


    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.W)){
            thruster.Fire(power);
        }

        if (Input.GetKey(KeyCode.A)){
            right.Fire(sidePower);
        }

        if (Input.GetKey(KeyCode.D)){
            left.Fire(sidePower);
        }
    }
}