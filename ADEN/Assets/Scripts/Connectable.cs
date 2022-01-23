using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Connectable : MonoBehaviour {
    public Vector3 offset;
    public float mass;
    public abstract void Control(IDictionary<string, bool> inputs);
    
    
    public float mainPower;
    public float sidePower;
}