using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {

    public Transform target;


    public float minSize = 200f;
    public float maxSize = 2000f;

    void Update() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            GetComponent<Camera>().orthographicSize -= Input.mouseScrollDelta.y * 100f;
        }

        if (GetComponent<Camera>().orthographicSize < minSize) {
            GetComponent<Camera>().orthographicSize = minSize;
        }
        if (GetComponent<Camera>().orthographicSize > maxSize) {
            GetComponent<Camera>().orthographicSize = maxSize;
        }

        if (target != null){
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);    
        }
    }
}
