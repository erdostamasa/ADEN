using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class Mothership : MonoBehaviour {
    public bool debugMode;
    public float angle;

    public GameObject centerPrefab;
    public Transform center;
    [FormerlySerializedAs("rotationRadius")]
    public float orbitRadius;
    public float speed;

    private Rigidbody2D rb;
    
    private void Awake() {
        debugMode = false;
        rb = GetComponent<Rigidbody2D>();
    }

    //Actual movement
    private void FixedUpdate() {

        //calculate speed based on orbit size
        if (center != null){

            float angleRad = angle * Mathf.Deg2Rad;
            
            float angularSpeed = speed / orbitRadius;
            float posX = center.position.x + Mathf.Cos(angleRad) * orbitRadius;
            float posY = center.position.y + Mathf.Sin(angleRad) * orbitRadius; 
            rb.MovePosition(new Vector2(posX, posY)); //move in circles

            //Calculate movement direction (merőleges a sugárra)
            Vector2 directionToCenter = (center.position - transform.position).normalized;
            float rotX = directionToCenter.x * Mathf.Cos(-90 * Mathf.Deg2Rad) -
                         directionToCenter.y * Mathf.Sin(-90 * Mathf.Deg2Rad);
            float rotY = directionToCenter.x * Mathf.Sin(-90 * Mathf.Deg2Rad) -
                         directionToCenter.y * Mathf.Cos(-90 * Mathf.Deg2Rad);
            Vector2 movementDirection = new Vector2(rotX, rotY);
            float lookAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Quaternion.AngleAxis(lookAngle, Vector3.forward));
            
            angle += +Time.fixedDeltaTime * angularSpeed * Mathf.Rad2Deg;
            if (angle > 360) angle = 0f; //if angle reached 2PI radians, reset it    
        }
    }
    
    //DEBUG IN EDITOR
    private void Update() {
        if (debugMode){
            if (center != null){
                RenderOrbit(1, 0);
                RenderOrbit(10, 13);                
            }

            float angleRad = angle * Mathf.Deg2Rad;
            //move in circles
            float posX = center.position.x + Mathf.Cos(angleRad) * orbitRadius;
            float posY = center.position.y + Mathf.Sin(angleRad) * orbitRadius;
            transform.position = new Vector2(posX, posY);

            Vector2 directionToCenter = (center.position - transform.position).normalized;
            float rotX = directionToCenter.x * Mathf.Cos(-90 * Mathf.Deg2Rad) -
                         directionToCenter.y * Mathf.Sin(-90 * Mathf.Deg2Rad);
            float rotY = directionToCenter.x * Mathf.Sin(-90 * Mathf.Deg2Rad) -
                         directionToCenter.y * Mathf.Cos(-90 * Mathf.Deg2Rad);
            Vector2 movementDirection = new Vector2(rotX, rotY);
            float lookAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            transform.rotation = (Quaternion.AngleAxis(lookAngle, Vector3.forward));
            if (angle > 360) angle = 0f; //if angle reached 2PI radians, reset it
        }
    }
    
    //draw ships' orbit using LineRenderer
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