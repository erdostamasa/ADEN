using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectorScript : MonoBehaviour {
    public float orbitSpeed;

    public GameObject centerPrefab;

    [HideInInspector]
    public bool orbiting;

    private float angle;
    private float distanceFromPlanet;

    private Rigidbody2D rb;
    private NavigationHUD hudScript;

    public Transform planetToOrbit;
    private GravityApplier gravScript;

    public Vector2 kinematicVelocity;
    private Vector2 oldpos;
    private Vector2 newpos;
    
    private void Awake() {
        orbiting = false;
        rb = transform.root.GetComponent<Rigidbody2D>();
        hudScript = FindObjectOfType<NavigationHUD>();
        gravScript = GetComponent<GravityApplier>();
    }

    private GameObject orbitLineObject;

    private void Update() {
        //Draw line from player to planet
        /*if (orbiting){
            Debug.DrawLine(transform.position, transform.position + (planetToOrbit.position - transform.position));
            line.SetPosition(0, transform.position + new Vector3(0, 0, 1));
            line.SetPosition(1,
                transform.position + (planetToOrbit.position - transform.position) + new Vector3(0, 0, 1));
        }*/

        //Set orbited planet
        if (Input.GetKeyDown(KeyCode.E)){
            if (gravScript.planets.Count == 0) return;
            PlanetScript planet = gravScript.planets[0];

            CircleCollider2D hitbox = null;
            CircleCollider2D gravityRadius = null;
            foreach (CircleCollider2D col in planet.gameObject.GetComponents<CircleCollider2D>()){
                if (col.isTrigger){
                    gravityRadius = col;
                } else{
                    hitbox = col;
                }
            }

            float scale = planet.transform.localScale.x;
            float distance = Vector2.Distance(planet.transform.position, transform.position);
            float attractedHeight = distance - hitbox.radius * scale;
            float t = attractedHeight / (gravityRadius.radius * scale - hitbox.radius * scale);
            float currentDrag = Mathf.Lerp(0, planet.planetDrag, planet.airResistanceCurve.Evaluate(t));

            if (currentDrag <= 0.02f){
                rb.bodyType = RigidbodyType2D.Kinematic;
                orbiting = true;
                planetToOrbit = planet.transform;
                hudScript.planet = planetToOrbit.GetComponent<PlanetScript>();

                //transform.rotation.SetLookRotation( );
                
                
                //calculate starting angle of player from the planets' perspective
                Vector2 difference = transform.position - planetToOrbit.position;
                angle = (float) Math.Atan2(difference.y, difference.x);

                distanceFromPlanet = Vector2.Distance(transform.position, planetToOrbit.position);

                //render the players orbit and save its reference
                orbitLineObject = RenderOrbit(planetToOrbit);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Q)){
            orbiting = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = kinematicVelocity;
            planetToOrbit = null;
            Destroy(orbitLineObject);
        }
    }

    

    private void FixedUpdate() {
        //Calculate velocity of kinematic Rigidbody2D
        newpos = transform.position;
        kinematicVelocity = (newpos - oldpos) / Time.fixedDeltaTime;
        oldpos = newpos;

        if (orbiting){
            rb.SetRotation(Quaternion.LookRotation(hudScript.relativeVelocity.normalized, Vector3.forward));
            //Calculate speed based on orbit radius
            float angularSpeed = orbitSpeed / distanceFromPlanet;
            float posX = planetToOrbit.position.x + Mathf.Cos(angle) * distanceFromPlanet;
            float posY = planetToOrbit.position.y + Mathf.Sin(angle) * distanceFromPlanet;
            rb.MovePosition(new Vector2(posX, posY));
            angle += Time.fixedDeltaTime * angularSpeed;
            if (angle > Mathf.PI * 2) angle = 0f;
        }
    }

    private GameObject RenderOrbit(Transform center) {
        //if the player doesnt already have a center GameObject with a LineRenderer, create one
        bool hasCircle = false;
        GameObject centerObject = null;
        foreach (Transform child in center){
            if (child.CompareTag("orbitCircle") && child.name == ("orbit" + this.name)){
                hasCircle = true;
                centerObject = child.gameObject;
                break;
            }
        }

        //gives "center" a child with a lineRenderer and sets its name to 'orbit' + this objects name
        if (!hasCircle){
            centerObject = Instantiate(centerPrefab, center.position, Quaternion.identity);
            centerObject.name = "orbit" + this.name;
            centerObject.transform.parent = center.transform;
        }

        LineRenderer orbitRenderer = centerObject.GetComponent<LineRenderer>();
        int lineSegments = 1000;

        orbitRenderer.positionCount = lineSegments + 1;
        orbitRenderer.useWorldSpace = false;

        float deltaTheta = (float) (2.0 * Mathf.PI) / lineSegments;
        float theta = 0f;
        //draw the circle from given number of line segments
        for (int i = 0; i < lineSegments + 1; i++){
            float x = distanceFromPlanet * Mathf.Cos(theta);
            float y = distanceFromPlanet * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, 1);
            orbitRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }

        return centerObject;
    }
}