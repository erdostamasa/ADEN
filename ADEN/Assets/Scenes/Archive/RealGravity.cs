using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RealGravity : MonoBehaviour {
    public float mass;
    public float radius;
    public Vector2 initialVelocity;
    Vector2 velocity;

    Rigidbody2D rb;

    private void Awake() {
        velocity = initialVelocity;
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateVelocity(RealGravity[] allBoides, float timeStep) {
        foreach(var otherBody in allBoides) {
            if(otherBody != this) {
                /*Debug.Log("other: " + otherBody);
                float sqrDistance = Mathf.Pow(Vector2.Distance(rb.position, otherBody.rb.position), 2);
                Vector2 forceDir = (otherBody.rb.position - rb.position).normalized;
                Vector2 force = forceDir * 0.0001f * mass * (otherBody.mass) / sqrDistance;
                Vector2 acceleration = force / mass;
                currentVelocity += acceleration * timeStep;*/

                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector2 forceDir = (otherBody.rb.position - rb.position).normalized;

                Vector2 acceleration = forceDir * 0.0001f * otherBody.mass / sqrDst;
                velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep) {
        rb.position += velocity * timeStep;
    }

}
