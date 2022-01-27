using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class PlanetScript : MonoBehaviour {
    public bool deadly = false;

    [Header("Gravity Settings:")]
    public AnimationCurve airResistanceCurve;

    public CircleCollider2D hitbox;
    public CircleCollider2D gravityRadius;

    public bool debugMode;
    public float angle;

    public Vector2 velocity;

    public float radius = 10;
    public float surfaceGravity = 10f;

    [Range(0f, 1f)]
    public float planetDrag;

    //[Range(3, 256)]
    //public int lineSegments = 128;
    public GameObject centerPrefab;
    public Transform center;

    [FormerlySerializedAs("rotationRadius")]
    public float orbitRadius;

    public float speed;
    public float rotationSpeed;

    private Vector2 oldpos;
    private Vector2 newpos;

    [HideInInspector]
    public Rigidbody2D rb;

    private void Awake() {
        debugMode = false;
    }

    private void Start() {
        if (center != null){
            RenderOrbit(1, 0);
            RenderOrbit(20, 13);
        }

        rb = GetComponent<Rigidbody2D>();
        oldpos = transform.position;
    }

    private void FixedUpdate() {
        //Calculate velocity of kinematic Rigidbody2D
        newpos = transform.position;
        velocity = (newpos - oldpos) / Time.fixedDeltaTime;
        oldpos = newpos;

        //calculate speed based on orbit size
        if (center != null){
            float radAngle = angle * Mathf.Deg2Rad;
            float angularSpeed = speed / orbitRadius;
            float posX = center.position.x + Mathf.Cos(radAngle) * orbitRadius;
            float posY = center.position.y + Mathf.Sin(radAngle) * orbitRadius;
            rb.MovePosition(new Vector2(posX, posY)); //move in circles
            angle += +Time.fixedDeltaTime * angularSpeed * Mathf.Rad2Deg;
            if (angle > 360) angle = 0f; //if angle reached 2PI radians, reset it
        }
        rb.MoveRotation(transform.rotation.eulerAngles.z + rotationSpeed * Time.fixedDeltaTime);
    }

    //edit planets in in editor
    private void Update() {
        if (debugMode){
            if (center != null){
                RenderOrbit(1, 0);
                RenderOrbit(20, 13);
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

    private void OnCollisionEnter2D(Collision2D other) {
        if (deadly){
            CustomTag ct = other.gameObject.GetComponent<CustomTag>();
            if (other.gameObject.CompareTag("Player")){
                GameMaster.Instance.DestroyCapsule();
            }else if (ct != null && ct.HasTag("killable")){
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D attracted) {
        //add this planet to the list of attractors in attracted
        GravityApplier gravScript = attracted.GetComponent<GravityApplier>();
        //TODO: find out why the planet gets added 2 times if building indicator hits player
        if (gravScript != null && !gravScript.planets.Contains(this)){
            gravScript.planets.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D attracted) {
        //remove this planet from the list of attractors in attracted   
        GravityApplier gravScript = attracted.GetComponent<GravityApplier>();
        if (gravScript != null){
            gravScript.planets.Remove(this);
        }
    }


    public float currentDrag;
    
    private void OnTriggerStay2D(Collider2D attracted) {
        //Check if attracted is already in trigger (happens if spawned in atmosphere)
        GravityApplier gravScript = attracted.GetComponent<GravityApplier>();
        if (attracted.CompareTag("Rocket")){
            gravScript = attracted.transform.parent.GetComponent<GravityApplier>();
        }
        //TODO: find out why the planet gets added 2 times if building indicator hits player
        if (gravScript != null && !gravScript.planets.Contains(this)){
            gravScript.planets.Add(this);
        }

        //calculate and apply current drag to attracted based on its distance from the planet
        //GravityApplier gravScript = attracted.GetComponent<GravityApplier>();
        if (gravScript != null){
            if (!gravScript.planets.Contains(this)){
                gravScript.planets.Add(this);
            }

            float scale = transform.localScale.x;
            float distance = Vector2.Distance(this.transform.position, attracted.transform.position);
            float attractedHeight = distance - hitbox.radius * scale;
            float t = attractedHeight / (gravityRadius.radius * scale - hitbox.radius * scale);
            //currentDrag = Mathf.Lerp(0, planetDrag, airResistanceCurve.Evaluate(t));

            gravScript.drag = Mathf.Lerp(0, planetDrag, airResistanceCurve.Evaluate(t));
            gravScript.angularDrag = Mathf.Lerp(0, planetDrag / 2f, airResistanceCurve.Evaluate(t));
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