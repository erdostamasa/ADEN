using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target = null;
    public float yOffset = 0f;

    public float minSize = 1f;
    public float maxSize = 10f;

    public GameObject grid;
    public float gridRange = 80f;
    
    private bool rotateWithPlayer;
    private Camera cam;
    private Rigidbody2D targetRb;

    private void Start() {
        rotateWithPlayer = false;
        cam = GetComponent<Camera>();
        
        if (target == null) return;
        targetRb = target.GetComponent<Rigidbody2D>();
    }

    public void SwitchCamera() {
        if (rotateWithPlayer){
            transform.rotation = Quaternion.identity;
        }

        rotateWithPlayer = !rotateWithPlayer;
    }

    void Update() {
        if (target == null) return;
        if (targetRb == null){
            targetRb = target.GetComponent<Rigidbody2D>();
        }
        
        transform.position =
            new Vector3(targetRb.worldCenterOfMass.x, targetRb.worldCenterOfMass.y, transform.position.z);

        if (!Input.GetKey(KeyCode.LeftControl)){
            if (Input.GetKey(KeyCode.LeftShift)){
                cam.orthographicSize -= Input.mouseScrollDelta.y * 10f;
            } else{
                cam.orthographicSize -= Input.mouseScrollDelta.y * 2f;    
            }
        }

        if (cam.orthographicSize < minSize){
            cam.orthographicSize = minSize;
        }

        if (cam.orthographicSize > maxSize){
            cam.orthographicSize = maxSize;
        }

        if (cam.orthographicSize > gridRange){
            grid.SetActive(false);
        } else{
            grid.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.C)){
            SwitchCamera();
        }


        if (rotateWithPlayer){
            transform.rotation = target.transform.rotation;
        }
    }

    private IEnumerator RotateCamera(float seconds, Quaternion startRotation, Quaternion endRotation) {
        for (float t = 0; t < seconds; t += Time.deltaTime){
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / seconds);
            yield return null;
        }

        transform.rotation = endRotation;
    }
}