using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[ExecuteInEditMode]
public class BackgroundPlanet : MonoBehaviour
{
    public bool debugMode;
    public float angle;
    
    public float radius = 10;

    public GameObject centerPrefab;
    public Transform center;
    public float orbitRadius;
    public float speed;
    public float rotationSpeed;
    
    private void Awake() {
        debugMode = false;
    }
    
    private void Start() {
        RenderOrbit(1, 0);
        RenderOrbit(10, 13);
    }

    private void FixedUpdate() {
        //calculate speed based on orbit size
        if (center != null){
            float radAngle = angle * Mathf.Deg2Rad;
            float angularSpeed = speed / orbitRadius;
            float posX = center.position.x + Mathf.Cos(radAngle) * orbitRadius;
            float posY = center.position.y + Mathf.Sin(radAngle) * orbitRadius;
            
            //transform.Translate(new Vector2(posX, posY));
            transform.position = new Vector2(posX, posY);

            //rb.MovePosition(new Vector2(posX, posY)); //move in circles
            angle += +Time.fixedDeltaTime * angularSpeed * Mathf.Rad2Deg;
            if (angle > 360) angle = 0f; //if angle reached 2PI radians, reset it
            
            transform.Rotate(0, 0,  rotationSpeed * Time.fixedDeltaTime);
            
            //transform.Rotate(transform.rotation.eulerAngles.z + rotationSpeed * Time.fixedDeltaTime);
            
            //rb.MoveRotation(transform.rotation.eulerAngles.z + rotationSpeed * Time.fixedDeltaTime);
        }
    }
    
    private void Update() {
        if (debugMode){
            if (center != null){
                RenderOrbit(1, 0);
                RenderOrbit(10, 13);               
            }

            float radAngle = angle * Mathf.Deg2Rad;
            //set planet size
            transform.localScale = new Vector3(radius, radius, 1);
            //move in circles
            float posX = center.position.x + Mathf.Cos(radAngle) * orbitRadius;
            float posY = center.position.y + Mathf.Sin(radAngle) * orbitRadius;
            transform.position = new Vector2(posX, posY);
            if (angle > 360) angle = 0f; //if angle reached 2PI radians, reset it
        }
    }

    //draw planets' orbit using LineRenderer
    private void RenderOrbit(float lineWidth, int layer) {
        //if the planet doesn't already have a center GameObject with a LineRenderer, create one
        bool hasCircle = false;
        GameObject centerObject = null;
        foreach (Transform child in center){
            if (child.CompareTag("orbitCircle") && child.name == ("orbit" + this.name + layer)){
                hasCircle = true;
                centerObject = child.gameObject;
            }
        }

        //gives "center" a child with a lineRenderer and sets its name to 'orbit' + this planets name
        if (!hasCircle){
            centerObject = Instantiate(centerPrefab, center.position, Quaternion.identity);
            centerObject.layer = layer;
            centerObject.name = "orbit" + this.name + layer;
            centerObject.transform.parent = center.transform;
            centerObject.transform.localPosition = new Vector3(0, 0, 20);
        }

        LineRenderer orbitRenderer = centerObject.GetComponent<LineRenderer>();
        int lineSegments = 1000;

        orbitRenderer.positionCount = lineSegments + 1;
        orbitRenderer.useWorldSpace = false;
        orbitRenderer.widthMultiplier = lineWidth;
        
        float deltaTheta = (float) (2.0 * Mathf.PI) / lineSegments;
        float theta = 0f;
        //draw the circle from given number of line segments
        for (int i = 0; i < lineSegments + 1; i++){
            float x = orbitRadius * Mathf.Cos(theta);
            float y = orbitRadius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, 1);
            orbitRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
    
}
