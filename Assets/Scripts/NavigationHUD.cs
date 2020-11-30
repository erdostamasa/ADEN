using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NavigationHUD : MonoBehaviour {
    public float lineLengthMultiplier;
    public float displacement;
    public float minVelocity;

    public GameObject target;
    public Transform triangle;
    public Transform circle;
    public float width;

    private Rigidbody2D rb;
    private SpriteRenderer lineRenderer;
    private SpriteRenderer triangleRenderer;
    private SpriteRenderer circleRenderer;
    private GravityApplier gravScript;
    private SelectorScript selector;

    private Vector2 direction;

    [HideInInspector]
    public PlanetScript planet;

    public float aaAaaaaaaaAAAAAAAAAAAAAAAAAAAAAAAAAA = 1f;

    public Vector2 relativeVelocity;

    private void Awake() {
        rb = transform.root.GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<SpriteRenderer>();
        triangleRenderer = triangle.GetComponent<SpriteRenderer>();
        circleRenderer = circle.GetComponent<SpriteRenderer>();
        gravScript = target.GetComponent<GravityApplier>();
        selector = transform.root.GetComponent<SelectorScript>();
    }

    public Vector2 surfaceVelocity;
    private void Update() {
        //if the target is not orbiting, show vector relative to
        //closest planet (if any)
        if (!selector.orbiting){
            if (gravScript.planets.Count == 0){
                direction = rb.velocity;
                surfaceVelocity = Vector2.zero;
            } else if (gravScript.planets.Count == 1){
                Vector2 relativeVelocity = (gravScript.planets[0].velocity - rb.velocity);
                
                //calculate surface velocity
                Vector2 directionToCenter = (gravScript.planets[0].transform.position - transform.position).normalized;
                float rotX = directionToCenter.x * Mathf.Cos(-90 * Mathf.Deg2Rad) -
                             directionToCenter.y * Mathf.Sin(-90 * Mathf.Deg2Rad);
                float rotY = directionToCenter.x * Mathf.Sin(-90 * Mathf.Deg2Rad) -
                             directionToCenter.y * Mathf.Cos(-90 * Mathf.Deg2Rad);
                Vector2 surfaceVelocityDirection = new Vector2(rotX, rotY);

                float korPerSec = gravScript.planets[0].rotationSpeed / 360;
                float szogsebesseg = 2 * Mathf.PI * korPerSec;
                float felsziniSeb = (szogsebesseg * gravScript.planets[0].hitbox.radius *gravScript.planets[0].transform.localScale.x); // <- ez a sugár

                surfaceVelocity = surfaceVelocityDirection.normalized * felsziniSeb;

                //Subtract surface velocity from relative velocity so HUD size is 0 when landed
                direction = (-1 * relativeVelocity) - surfaceVelocity;
            } else{
                //find the closest planet
                PlanetScript closest = gravScript.planets[0];
                foreach (PlanetScript planet in gravScript.planets){
                    if (Vector2.Distance(transform.position, planet.transform.position) <
                        Vector2.Distance(transform.position, closest.transform.position)){
                        closest = planet;
                    }
                }

                Vector2 relativeVelocity = closest.velocity - rb.velocity;
                
                //calculate surface velocity
                Vector2 directionToCenter = (gravScript.planets[0].transform.position - transform.position).normalized;
                float rotX = directionToCenter.x * Mathf.Cos(-90 * Mathf.Deg2Rad) -
                             directionToCenter.y * Mathf.Sin(-90 * Mathf.Deg2Rad);
                float rotY = directionToCenter.x * Mathf.Sin(-90 * Mathf.Deg2Rad) -
                             directionToCenter.y * Mathf.Cos(-90 * Mathf.Deg2Rad);
                Vector2 surfaceVelocityDirection = new Vector2(rotX, rotY);

                float korPerSec = gravScript.planets[0].rotationSpeed / 360;
                float szogsebesseg = 2 * Mathf.PI * korPerSec;
                float felsziniSeb = (szogsebesseg * gravScript.planets[0].hitbox.radius *gravScript.planets[0].transform.localScale.x); // <- ez a sugár

                surfaceVelocity = surfaceVelocityDirection.normalized * felsziniSeb;
                
                
                direction = (-1 * relativeVelocity) - surfaceVelocity;
            } //if the target is orbiting, show vector relative to orbit target
        } else{
            relativeVelocity = (planet.velocity - selector.kinematicVelocity);
            direction = -1 * relativeVelocity;
        }

        //if target is standing still, hide direction vector
        if (direction.magnitude < minVelocity){
            lineRenderer.enabled = false;
            triangleRenderer.enabled = false;
            circleRenderer.enabled = false;
        } else{
            lineRenderer.enabled = true;
            triangleRenderer.enabled = true;
            circleRenderer.enabled = true;
        }

        Vector2 displacementVector = direction.normalized * displacement;
        //Set line position and rotation
        lineRenderer.size = new Vector2(direction.magnitude * lineLengthMultiplier, width);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position = target.transform.position + new Vector3(displacementVector.x, displacementVector.y, -5);
        //transform.localPosition = new Vector3(displacementVector.x, displacementVector.y, 1);
        //Set triangle position and rotation
        triangle.position = transform.position +
                            new Vector3(direction.x * lineLengthMultiplier, direction.y * lineLengthMultiplier, 0);
        triangle.rotation = transform.rotation;
        //Set circle rotation
        circle.rotation = transform.rotation;
    }
}