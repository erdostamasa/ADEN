using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityApplier : MonoBehaviour {
    //planet(script)s that this GameObject is attracted to
    public List<PlanetScript> planets = new List<PlanetScript>();
    public float gConstant = 2f; //unused (cancels out)

    public float drag = 0f;
    public float angularDrag = 0f;

    public Rigidbody2D rb;

    public Vector2 currentVelocity;

    private void Start() {
        //Get list of planet scripts // DONE BY PLANETS
        //planets = FindObjectsOfType<PlanetScript>();

        rb = GetComponent<Rigidbody2D>();
        currentVelocity = Vector3.zero;
    }

    private void FixedUpdate() {
        //for every planet in the vicinity
        foreach (var planet in planets) {
            //calculate and apply attraction
            
            float sqrDst = (planet.rb.position - rb.worldCenterOfMass).sqrMagnitude;
            Vector2 forceDir = (planet.rb.position - rb.worldCenterOfMass).normalized;
            float planetMass = planet.surfaceGravity * planet.radius * planet.radius;
            Vector2 acceleration = forceDir * (gConstant * planetMass) / sqrDst * (5000 * rb.mass);
            currentVelocity = acceleration * Time.fixedDeltaTime;
            rb.AddForce(currentVelocity * Time.fixedDeltaTime);

            //calculate and apply drag
            rb.AddForce((planet.velocity - rb.velocity) * drag);
            //angular drag
            rb.angularDrag = angularDrag;

        }
    }
}